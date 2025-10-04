using UnityEngine;

[CreateAssetMenu(menuName = "Stat/Stat Data")]
public class StatData : ScriptableObject
{
    [SerializeField] private string statName;

    public string StatName { get => statName; }
}
