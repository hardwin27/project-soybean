using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class AutomationCardController : CardController
{
    [Title("Automation State")]
    [ReadOnly]
    [SerializeField] protected bool isAutoMoving = false;

    [Title("Auto-Stacking Configuration")]
    [SerializeField]
    [InfoBox("Add CardData assets here. The automation will hunt for cards matching this data.")]
    private List<CardData> requiredCards = new List<CardData>();

    [SerializeField]
    [InfoBox("The card currently being hunted.")]
    private CardController targetedCard;

    [SerializeField]
    private float scanRadius = 5.0f;

    [SerializeField]
    private float captureRadius = 2.0f;

    [SerializeField]
    private float moveDuration = 0.5f;

    [Space]
    [ReadOnly]
    [SerializeField]
    [InfoBox("Cards currently stacked on top of this automation card via the capture system.")]
    private List<CardController> capturedCards = new List<CardController>();

    private Vector3? activeTargetPosition = null;
    private float calculatedSpeed = 0f;
    private Coroutine currentMoveRoutine;

    [Title("Debug Movement")]
    [SerializeField]
    private Vector3 debugTargetPosition;

    [Button("Move To Debug Position", ButtonSizes.Large)]
    [DisableInEditorMode]
    private void DebugMoveToPosition()
    {
        if (!Application.isPlaying) return;
        if (targetedCard != null)
        {
            MoveToPosition(targetedCard.transform.position, moveDuration);
        }
        else
        {
            MoveToPosition(debugTargetPosition, moveDuration);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(MonitorCapturedCards());
        StartCoroutine(ScanForTargets());
    }

    private IEnumerator MonitorCapturedCards()
    {
        WaitForSeconds waitInterval = new WaitForSeconds(0.5f);

        while (true)
        {
            for (int i = capturedCards.Count - 1; i >= 0; i--)
            {
                CardController card = capturedCards[i];

                if (card == null || card.gameObject == null)
                {
                    capturedCards.RemoveAt(i);
                    continue;
                }

                if (!IsCardInMyStack(card))
                {
                    // Debug.Log($"[Automation] Card {card.name} left the stack.");
                    capturedCards.RemoveAt(i);
                }
            }
            yield return waitInterval;
        }
    }

    private IEnumerator ScanForTargets()
    {
        WaitForSeconds scanInterval = new WaitForSeconds(1.0f); // Scan once per second

        while (true)
        {
            if (!isAutoMoving && targetedCard == null && requiredCards.Count > 0 && !IsOnProcess)
            {
                FindBestTarget();
            }
            yield return scanInterval;
        }
    }

    private void FindBestTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, scanRadius);

        float closestDist = float.MaxValue;
        CardController bestCandidate = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out CardController candidate))
            {
                if (candidate == this) continue;

                if (candidate.IsDragged) continue;

                if (capturedCards.Contains(candidate)) continue;

                bool isRequiredType = false;
                if (candidate.CardData != null)
                {
                    foreach (CardData reqData in requiredCards)
                    {
                        if (reqData != null && reqData == candidate.CardData)
                        {
                            isRequiredType = true;
                            break;
                        }
                    }
                }

                if (isRequiredType)
                {
                    float dist = Vector3.Distance(transform.position, candidate.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        bestCandidate = candidate;
                    }
                }
            }
        }

        if (bestCandidate != null)
        {
            Debug.Log($"[Automation] Found Target: {bestCandidate.name}");
            targetedCard = bestCandidate;
            MoveToPosition(targetedCard.transform.position, moveDuration);
        }
    }

    private bool IsCardInMyStack(CardController cardToCheck)
    {
        if (cardToCheck == this) return true;
        CardController currentParent = cardToCheck.StackedOnCard;

        while (currentParent != null)
        {
            if (currentParent == this) return true;
            currentParent = currentParent.StackedOnCard;
        }
        return false;
    }

    public void MoveToPosition(Vector3 targetWorldPosition, float duration, Action onComplete = null)
    {
        if (IsOnProcess) return;

        float distance = Vector3.Distance(transform.position, targetWorldPosition);
        calculatedSpeed = distance > 0 ? distance / duration : 0;

        activeTargetPosition = targetWorldPosition;
        StartMoveRoutine(targetWorldPosition, duration, onComplete);
    }

    private void StartMoveRoutine(Vector3 targetPos, float duration, Action onComplete)
    {
        if (currentMoveRoutine != null) StopCoroutine(currentMoveRoutine);
        currentMoveRoutine = StartCoroutine(MoveRoutine(targetPos, duration, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, float duration, Action onComplete)
    {
        isAutoMoving = true;

        StackWithCard(null); 

        ToggleDragSorting(true, ZOrder + 10);

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);

            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, t);
            transform.position = new Vector3(nextPos.x, nextPos.y, transform.position.z);
            lastPost = transform.position;

            OnCardPosDragged?.Invoke();

            yield return null;
        }

        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        lastPost = transform.position;
        OnCardPosDragged?.Invoke();

        ToggleDragSorting(false);
        CheckAndCaptureTargetCard();

        isAutoMoving = false;
        activeTargetPosition = null;
        onComplete?.Invoke();
    }

    private void CheckAndCaptureTargetCard()
    {
        if (targetedCard != null && targetedCard.gameObject.activeInHierarchy)
        {
            float dist = Vector3.Distance(transform.position, targetedCard.transform.position);

            if (dist <= captureRadius)
            {
                Debug.Log($"[Automation] Captured Target Card: {targetedCard.name}");

                targetedCard.StackWithCard(this.TopCard);

                if (!capturedCards.Contains(targetedCard))
                {
                    capturedCards.Add(targetedCard);
                }

                targetedCard = null;
            }
            else
            {
                Debug.Log($"[Automation] Target Card too far or moved: {dist} > {captureRadius}");
                targetedCard = null; // Clear target so we can try again or find a new one
            }
        }
        else
        {
            targetedCard = null;
        }

        targetedCard = null;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanBeDragged) return;

        if (isAutoMoving)
        {
            if (currentMoveRoutine != null) StopCoroutine(currentMoveRoutine);
            isAutoMoving = false;
        }

        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (activeTargetPosition.HasValue)
        {
            Vector3 target = activeTargetPosition.Value;
            float distanceLeft = Vector3.Distance(transform.position, target);

            float remainingDuration = calculatedSpeed > 0 ? distanceLeft / calculatedSpeed : 0.5f;
            remainingDuration = Mathf.Max(0.1f, remainingDuration);

            StartMoveRoutine(target, remainingDuration, null);
        }
        else
        {
            ToggleDragSorting(false);
            StackWithCard(null);
        }
    }
}