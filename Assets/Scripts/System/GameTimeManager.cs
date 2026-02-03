using System;
using UnityEngine;
using SingletonSystem;
using ReadOnlyEditor;

public class GameTimeManager : Singleton<GameTimeManager>
{
    [Header("Game Time State")]
    [SerializeField, ReadOnly] private int currentDay = 0;
    [SerializeField, ReadOnly] private int currentWeek = 1;
    [SerializeField, ReadOnly] private float dayTimer;
    [SerializeField, ReadOnly] private bool isDayActive;

    [Header("Settings")]
    [SerializeField] private float dayDuration = 60f;

    public int CurrentDay => currentDay;
    public int CurrentWeek => currentWeek;
    public float DayTimer => dayTimer;
    public float DayDuration => dayDuration;
    public bool IsDayActive => isDayActive;

    public Action OnDayStarted;
    public Action OnDayEnded;

    private void Start()
    {
        /*StartNextDay();*/
    }

    private void Update()
    {
        if (isDayActive)
        {
            dayTimer += Time.deltaTime;

            if (dayTimer >= dayDuration)
            {
                EndDay();
            }
        }
    }

    public void StartNextDay()
    {
        if (isDayActive)
        {
            Debug.LogWarning("Day is already active!");
            return;
        }

        currentDay++;
        if (currentDay > 7)
        {
            currentDay = 1;
            currentWeek++;
        }

        dayTimer = 0;

        isDayActive = true;

        OnDayStarted?.Invoke();

        Debug.Log($"Day Started: Week {currentWeek}, Day {currentDay}");
    }

    public void EndDay()
    {
        if (!isDayActive) return;

        isDayActive = false;
        dayTimer = 0;
        OnDayEnded?.Invoke();

        Debug.Log("Day Ended");
    }
}