using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SinkMinigame : Minigame
{
    [SerializeField] private Button[] taps;
    [SerializeField] private Image[] waterStreams;
    [SerializeField] private Button clog;
    [SerializeField] private Image water;
    [SerializeField][Tooltip("Min and max amount of turns")] private Vector2Int turnRange = new Vector2Int(2, 5);
    [SerializeField][Tooltip("Min and max amount of time to unclog")] private Vector2Int clogTimeRange = new Vector2Int(1, 3);
    [SerializeField] private float clogShakeSpeed;

    private SoundComponent soundComponent;
    private float clogTimer;
    private bool clogIsHeld = false;
    private int[] tapTurns;
    private float clogRotationDegree = 5;

    private void Start()
    {
        tapTurns = new int[taps.Length];
        soundComponent = GetComponent<SoundComponent>();
        Reset();
    }

    private void Update()
    {
        if (clogIsHeld)
        {
            clogTimer -= Time.deltaTime;
            if (clogTimer <= 0)
            {
                clog.gameObject.SetActive(false);
                ReleaseClog();
            }
        }
        Shake();
    }

    private void Shake()
    {
        clog.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.PingPong(Time.time * (clogIsHeld ? clogShakeSpeed*2 : clogShakeSpeed), clogRotationDegree*2) - clogRotationDegree));
    }
    public void TurnTap(Button tap)
    {
        for (int i = 0; i < taps.Length; i++)
        {
            if (taps[i] == tap)
            {
                if (tapTurns[i] > 0)
                {
                    tap.transform.Rotate(new Vector3(0,0,-45));
                    tapTurns[i]--;
                    soundComponent.PlaySound("Turn tap");
                }
                if (tapTurns[i] <= 0)
                {
                    waterStreams[i].gameObject.SetActive(false);
                }

                CheckForWin();
                return;
            }
        }
    }

    public void PressClog()
    {
        clogIsHeld = true;
    }

    public void ReleaseClog()
    {
        clogIsHeld = false;
        CheckForWin();
        if (clogTimer > 0)
        {
            ResetClogTimer();
        }
        else
        {
            water.gameObject.SetActive(false);
            soundComponent.PlaySound("Drain water");
        }
    }
    private void CheckForWin()
    {
        foreach (int turns in tapTurns)
        {
            if (turns > 0)
            {
                return;
            }
        }

        if (clogTimer > 0)
        {
            return;
        }

        StartCoroutine(Finish());
    }

    private void Reset()
    {
        clog.gameObject.SetActive(true);
        foreach (Image stream in waterStreams)
        {
            stream.gameObject.SetActive(true);
        }
        water.gameObject.SetActive(true);
        ResetClogTimer();
        for(int i = 0; i < tapTurns.Length; i++)
        {
            tapTurns[i] = Random.Range(turnRange.x, turnRange.y);
            taps[i].transform.rotation = quaternion.identity;
            taps[i].transform.Rotate(new Vector3(0,0,45 * tapTurns[i]));
        }
    }

    private void ResetClogTimer()
    {
        clogTimer = Random.Range(clogTimeRange.x, clogTimeRange.y);
    }
    
    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(.2f);
        Reset();
        Finished();
    }
}
