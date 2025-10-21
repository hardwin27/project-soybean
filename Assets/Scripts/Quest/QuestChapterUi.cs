using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;

public class QuestChapterUi : MonoBehaviour
{
    [SerializeField] private RectTransform chapterRect;
    [SerializeField] private TextMeshProUGUI chapterTitleTitleText;
    [SerializeField] private float offset;

    public void AssignChapterTitle(string title, bool isFirstChapter = true)
    {
        chapterTitleTitleText.text = title;

        chapterTitleTitleText.ForceMeshUpdate();

        float height = chapterTitleTitleText.preferredHeight;
        if (!isFirstChapter) 
        {
            height += offset;
        }

        chapterRect.sizeDelta = new Vector2(chapterRect.sizeDelta.x, height);
    }
}
