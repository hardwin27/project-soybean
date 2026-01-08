using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeUi : MonoBehaviour
{
    private GameTimeManager gameTimeManager;

    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private Slider timeSlider;

    private void Awake()
    {
        gameTimeManager = GameTimeManager.Instance;

        if (gameTimeManager != null)
        {
            gameTimeManager.OnDayStarted += HandleOnDayStarted;
        }
    }

    private void Start()
    {
        timeSlider.minValue = 0;
        timeSlider.maxValue = 1;   
    }

    private void Update()
    {
        if (gameTimeManager != null) 
        {
            if (gameTimeManager.IsDayActive)
            {
                timeSlider.value = gameTimeManager.DayTimer / gameTimeManager.DayDuration;
            }
        }
    }     

    private void HandleOnDayStarted()
    {
        dateText.text = $"Day {gameTimeManager.CurrentDay} Week {gameTimeManager.CurrentWeek}";
    }
}
