using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text explanationField;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetExplanation(string explanation)
    {
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            explanationField.text = explanation;
            Invoke("BackToMenu", 3f);
        }
    }

    private void BackToMenu()
    {
        Debug.Log("Called");
        StartCoroutine(Fade(gameOverText, gameOverText.color, Color.clear, 2f));
        StartCoroutine(Fade(explanationField, explanationField.color, Color.clear, 2f));
        Invoke("LoadMainMenu", 2f);
    }
    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        transform.GetChild(0).gameObject.SetActive(false);
    }
        
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
