using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private List<DialogueEntry> DialogueData;
    [SerializeField] private TMP_Text nameField;
    [SerializeField] private TMP_Text lineField;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject backbutton;
    [SerializeField] private float timeToSkip;
    [SerializeField] private Image cover;
    [SerializeField] private Image background;
    [SerializeField] private Image father;
    [SerializeField] private Image mother;

    public enum Names {YOU, PARENTS, FATHER, MOTHER}
    private int index;
    public static event Action s_StartGame;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
    }
    public void LoadLine()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            nextButton.GetComponentInChildren<TMP_Text>().text = "NEXT";
            StartCoroutine(Fade(father, Color.clear, Color.white, 1f));
            StartCoroutine(Fade(mother, Color.clear, Color.white, 1f));
        }

        nameField.text = GetDialogueEntry(index).Name.ToString() + ":";
        lineField.text = GetDialogueEntry(index).Line;
        StartCoroutine(Fade(nameField, new Color(1f, 1f, 1f, 0f), Color.white, 0.5f));
        StartCoroutine(Fade(lineField, new Color(1f, 1f, 1f, 0f), Color.white, 0.5f));
        StartCoroutine(SkipCooldown(timeToSkip));
    }
    public void Next()
    {
        nextButton.SetActive(false);

        if (index + 1 < DialogueData.Count)
        {
            StartCoroutine(Fade(nameField, nameField.color, new Color(1f, 1f, 1f, 0f), 0.5f));
            StartCoroutine(Fade(lineField, lineField.color, new Color(1f, 1f, 1f, 0f), 0.5f));
            index++;
            Invoke("LoadLine", .5f);
        }
        else
        {
            StartCoroutine(Fade(background, background.color, Color.black, 1f));
            StartCoroutine(Fade(cover, Color.clear, new Color(0f, 0f, 0f, 1f), 1f));
            StartCoroutine(Fade(father, father.color, Color.clear, 1f));
            StartCoroutine(Fade(mother, father.color, Color.clear, 1f));
            Invoke("LoadGameScene", 1f);
        }
    }
    private void ShowSkipPrompt() => nextButton.SetActive(true);
    public void LoadGameScene()
    {
        s_StartGame?.Invoke();
        ResetDialogue();
    }
    public void ResetDialogue()
    {
        gameObject.SetActive(false);
        index = 0;
    }
    public void SkipToGame()
    {
        StartCoroutine(Fade(background, background.color, Color.black, 1f));
        StartCoroutine(Fade(cover, Color.clear, new Color(0f, 0f, 0f, 1f), 1f));
        StartCoroutine(Fade(father, father.color, Color.clear, 1f));
        StartCoroutine(Fade(mother, father.color, Color.clear, 1f));
        Invoke("LoadGameScene", 1f);
    }
    private DialogueEntry GetDialogueEntry(int i) => DialogueData[i];
    private IEnumerator SkipCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        if(index + 1 < DialogueData.Count)
            ShowSkipPrompt();
        else
        {
            ShowSkipPrompt();
            nextButton.GetComponentInChildren<TMP_Text>().text = "START";
        }
    }
    private IEnumerator Fade(TMP_Text text, Color from, Color to, float duration)
    {
        float time = 0f;
        text.color = from;

        while(time <= duration)
        {
            time += Time.deltaTime;
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
            time += Time.deltaTime;
            image.color = Color.Lerp(from, to, time / duration);
            yield return null;
        }
    }
    [Serializable] private struct DialogueEntry
    {
        public Names Name;
        public string Line;

        private DialogueEntry(Names name, string line)
        {
            Name = name;
            Line = line;
        }
    }
}
