using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class CountdownClock : MonoBehaviour
{
    [SerializeField] private float hourTime = 30;
    [SerializeField] private RectTransform arrowHour;
    [SerializeField] private RectTransform arrowMinutes;
    [SerializeField] private Image handSprite;
    [SerializeField] private Vector2 handColor;
    private int hours = 0;
    private float sHours = 0;
    private float minutes = 0;
    private float time = 0;
    private SplitToning darkness;
    [SerializeField] private int hoursToDarkness;
    private Material skybox;
    [SerializeField] private float skyboxStart;


    private void Start()
    {
        transform.root.GetComponent<Volume>().profile?.TryGet<SplitToning>(out darkness);
        skybox = RenderSettings.skybox;
    }

    public void Update()
    {
        time += Time.deltaTime;

        if(time >= hourTime)
        {
            time = 0;
            minutes = 0;
            hours++;
        }

        TimeClock();
        DarknessProgress();
    }

   private void TimeClock()
    {
        minutes = time / hourTime;
        sHours = hours + (time / hourTime);
        arrowHour.rotation = Quaternion.Euler(0f, 0f, 180 - (30 * sHours));
        arrowMinutes.rotation = Quaternion.Euler(0f, 0f, -minutes * 360);
    }

    private void DarknessProgress()
    {
        float t = (sHours * hourTime) / (hoursToDarkness * hourTime);
        float c = Mathf.Lerp(handColor.x, handColor.y, t) / 255f;
        handSprite.color = new Color(c, c, c, 1f);
        darkness.balance.value = Mathf.Lerp(-100, 100, t);
        skybox.SetFloat("Exponent", Mathf.Lerp(skyboxStart, 3f, t));
    }
}

