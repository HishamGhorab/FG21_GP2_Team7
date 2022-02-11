using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ClockHappenings : MonoBehaviour
{
    [SerializeField] private int hourTime;
    [SerializeField] private GameObject hourAnnouncement;
    [SerializeField] private int visibleTime;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private TMP_Text hourText;
    [SerializeField] private TMP_Text hoursToGoText;
    private Image background;
    public int hourIndex = 0;
    public static event Action<int> s_UpdateHour;
    private SoundComponent soundComponent;

    private void Start()
    {
        soundComponent = GetComponent<SoundComponent>();
        background = hourAnnouncement.GetComponentInChildren<Image>();
        FunctionTimer.Create(FirstHour, 0); 
        FunctionTimer.Create(SecondHour, hourTime);
        FunctionTimer.Create(ThirdHour, 2 * hourTime);
        FunctionTimer.Create(FourthHour, 3 * hourTime);
        FunctionTimer.Create(FifthHour, 4 * hourTime);
        FunctionTimer.Create(SixthHour, 5 * hourTime);
        FunctionTimer.Create(lastHour, 6 * hourTime);
    }
    
    private void FirstHour()
    {
        hourIndex = 0;
        StartCoroutine(Fade(hourText, Color.clear, Color.white, fadeInTime));
        StartCoroutine(Fade(hoursToGoText, Color.clear, Color.white, fadeInTime));
        ShowHourAnnouncement(18);
    }

    private void SecondHour()
    {
        hourIndex = 1;
        StartCoroutine(Fade(hourText, hourText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(hoursToGoText, hoursToGoText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(background, background.color, Color.black, fadeInTime));
        ShowHourAnnouncement(19);
    } 
    private void ThirdHour()
    {
        hourIndex = 2;
        StartCoroutine(Fade(hourText, hourText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(hoursToGoText, hoursToGoText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(background, background.color, Color.black, fadeInTime));
        ShowHourAnnouncement(20);
    }
    private void FourthHour()
    {
        hourIndex = 3;
        StartCoroutine(Fade(hourText, hourText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(hoursToGoText, hoursToGoText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(background, background.color, Color.black, fadeInTime));
        ShowHourAnnouncement(21);
    }
    private void FifthHour()
    {
        hourIndex = 4;
        StartCoroutine(Fade(hourText, hourText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(hoursToGoText, hoursToGoText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(background, background.color, Color.black, fadeInTime));
        ShowHourAnnouncement(22);
    }

    private void SixthHour()
    {
        hourIndex = 5;
        StartCoroutine(Fade(hourText, hourText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(hoursToGoText, hoursToGoText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(background, background.color, Color.black, fadeInTime));
        ShowHourAnnouncement(23);
    }

    private void lastHour()
    {
        StartCoroutine(Fade(hourText, hourText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(hoursToGoText, hoursToGoText.color, Color.white, fadeInTime));
        StartCoroutine(Fade(background, background.color, Color.black, fadeInTime));
        ShowHourAnnouncement(24);
    }

    private void DeactivateHourAnnouncement() => hourAnnouncement.SetActive(false);

    private void ShowHourAnnouncement(int hour)
    {
        if (hourIndex != 0)
        {
            soundComponent.PlaySound("Hour announcement effect");
            hoursToGoText.text = $"{6 - hourIndex} HOURS UNTIL MIDNIGHT";
        }
            

        if(hour != 24)
        {
            s_UpdateHour?.Invoke(hourIndex);
            Time.timeScale = 0f;
            hourText.text = $"{hour}:00";
            
            hourAnnouncement.SetActive(true);
            StartCoroutine(HideHourAnnouncement());
        }
        else
        {
            hourText.text = "YOU MADE IT";
            hoursToGoText.text = "";
            hourAnnouncement.SetActive(true);
            Invoke("BackToMenu", visibleTime);
        }
        
    }
    private IEnumerator HideHourAnnouncement()
    {
        yield return new WaitForSecondsRealtime(visibleTime);
        Time.timeScale = 1f;
        StartCoroutine(Fade(hourText, hourText.color, Color.clear, fadeOutTime));
        StartCoroutine(Fade(background, background.color, Color.clear, fadeOutTime));
        StartCoroutine(Fade(hoursToGoText, hoursToGoText.color, Color.clear, fadeOutTime));
        Invoke("DeactivateHourAnnouncement", fadeOutTime);
    }
    private void BackToMenu() => SceneManager.LoadScene(0);
    private IEnumerator Fade(TMP_Text text, Color from, Color to, float duration)
    {
        float time = 0f;
        text.color = from;

        while (time <= duration)
        {
            time += Time.unscaledDeltaTime;
            text.color = Color.Lerp(from, to, time / duration);
            yield return null;
        }
    }
    private IEnumerator Fade(Image image, Color from, Color to, float duration)
    {
        float time = 0f;
        image.color = from;

        while (time <= duration)
        {
            time += Time.unscaledDeltaTime;
            image.color = Color.Lerp(from, to, time / duration);
            yield return null;
        }
    }
}
