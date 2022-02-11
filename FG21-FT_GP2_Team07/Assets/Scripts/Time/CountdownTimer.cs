using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float maxTimerValue;
    public UnityEvent countDownCalledEvent;
    public UnityEvent countDownEndConditionEvent;
    public bool Active { get => active; set => active = value; }
    public bool TimerPaused { get => timerPaused; set => timerPaused = value; }
    public bool CanCallCalledEvent { get => canCallCalledEvent; set => canCallCalledEvent = value; }
    public bool CanCallEndEvent { get => canCallEndEvent; set => canCallEndEvent = value;}

    public float TimeValue { get => timeValue; set => timeValue = value; }

    public float MaxTimerValue
    {
        get => maxTimerValue;
        set => maxTimerValue = value;
    }

    [Header("Debugging")]
    [SerializeField] private float timeValue;
    private bool active;
    private bool timerPaused;
    private bool canCallCalledEvent;
    private bool canCallEndEvent;

    void Start()
    {
        ResetTimer(false);
        timerPaused = false;
        
        canCallCalledEvent = true;
        canCallEndEvent = true;
    }

    private void Update()
    {
        if (!active || timerPaused)
            return;
        
        if (IsTimerReached())
        {
            CountDownEndCondition();
            ResetTimer(false);
        }
    }

    public void CountDownCalledEvent()
    {
        if (canCallCalledEvent)
            countDownCalledEvent.Invoke();
        
    }
    private void CountDownEndCondition()
    {
        if(canCallEndEvent)
            countDownEndConditionEvent.Invoke();
    }

    private bool IsTimerReached()
    {
        timeValue -= Time.deltaTime;

        if (timeValue <= 0)
            return true;
        return false;
    }

    public void ResetTimer(bool keepActive)
    {
        timeValue = maxTimerValue;
        
        if (keepActive) {
            active = true;
        }
        else {
            active = false;
        }
    }
}
