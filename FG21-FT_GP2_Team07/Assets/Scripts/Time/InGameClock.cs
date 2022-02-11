using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameClock : MonoBehaviour
{
    [SerializeField] private float hourTime = 30;
    [SerializeField] private Transform arrowHour;
    [SerializeField] private Transform arrowMinutes;
    private int hours = 0;
    private float sHours = 0;
    private float minutes = 0;
    private float time = 0;

    public void Update()
    {
        time += Time.deltaTime;

        if (time >= hourTime)
        {
            time = 0;
            minutes = 0;
            hours++;
        }

        TimeClock();
    }

    private void TimeClock()
    {
        minutes = time / hourTime;
        sHours = hours + (time / hourTime);
        arrowHour.rotation = Quaternion.Euler(0f, 0f, 180 - (30 * sHours));
        arrowMinutes.rotation = Quaternion.Euler(0f, 0f, -minutes * 360);
    }
}

