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
    [InfoBox("The card to search for when movement finishes.")]
    private CardController targetedCard;

    [SerializeField]
    [InfoBox("How close the targeted card must be to be captured.")]
    private float captureRadius = 2.0f;

    [Space]
    [ReadOnly]
    [SerializeField]
    [InfoBox("Cards currently stacked on top of this automation card via the capture system.")]
    private List<CardController> capturedCards = new List<CardController>();

    // State to handle pausing and resuming
    private Vector3? activeTargetPosition = null;
    private float calculatedSpeed = 0f;
    private Coroutine currentMoveRoutine;

    [Title("Debug Movement")]
    [SerializeField]
    [InfoBox("Set the target position and click the button below to test movement in Play Mode.")]
    private Vector3 debugTargetPosition;

    [SerializeField]
    private float debugMoveDuration = 0.5f;

    [Button("Move To Debug Position", ButtonSizes.Large)]
    [DisableInEditorMode]
    private void DebugMoveToPosition()
    {
        if (!Application.isPlaying) return;
        if (targetedCard != null) 
        {
            MoveToPosition(targetedCard.transform.position, debugMoveDuration);
        }
        else
        {
            MoveToPosition(debugTargetPosition, debugMoveDuration);
        }
    }

    // -------------------------------------------------------------------------
    // OPTIMIZED: Coroutine Monitoring instead of Update()
    // -------------------------------------------------------------------------

    private void OnEnable()
    {
        StartCoroutine(MonitorCapturedCards());
    }

    private IEnumerator MonitorCapturedCards()
    {
        WaitForSeconds waitInterval = new WaitForSeconds(0.5f);

        while (true)
        {
            // Iterate backwards so we can remove items safely
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
                    Debug.Log($"[Automation] Card {card.name} left the stack. Removing from list.");
                    capturedCards.RemoveAt(i);
                }
            }

            yield return waitInterval;
        }
    }

    private bool IsCardInMyStack(CardController cardToCheck)
    {
        if (cardToCheck == this) return true;

        CardController currentParent = cardToCheck.StackedOnCard;

        while (currentParent != null)
        {
            if (currentParent == this)
            {
                return true;
            }
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
                Debug.Log($"[Automation] Target Card too far: {dist} > {captureRadius}");
            }
        }
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