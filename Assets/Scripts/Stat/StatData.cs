using UnityEngine;

[CreateAssetMenu(menuName = "Stat/Stat Data")]
public class StatData : ScriptableObject
{
    [SerializeField] private string statName;
    [SerializeField] private Sprite statIndicatorSprite;
    [SerializeField] private Vector3 statIndicatorPos;
    [SerializeField] private Vector3 statIndicatorSize;

    public string StatName { get => statName; }
    public Sprite StatIndicatorSprite { get => statIndicatorSprite;}
    public Vector3 StatIndicatorPos { get => statIndicatorPos; }
    public Vector3 StatIndicatorSize { get => statIndicatorSize; }
}
