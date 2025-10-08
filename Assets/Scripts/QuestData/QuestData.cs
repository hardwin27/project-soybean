using UnityEngine;

/*[CreateAssetMenu(fileName = "QuestData", menuName = "Quest/Quest Data")]*/
public class QuestData : ScriptableObject
{
    [SerializeField] protected string questTitle;

    public string QuestTitle { get => questTitle;  }
}
