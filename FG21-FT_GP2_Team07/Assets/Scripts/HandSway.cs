using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSway : MonoBehaviour
{
    public RectTransform uiHand;
    public Vector2 startPos;
    public Vector2 endPos;
    [SerializeField] private AnimationCurve swayXCurve;
    [SerializeField] private AnimationCurve swayYCurve;
    private PlayerMovement player;
    private Vector2 top;
    private Vector2 bottom;
    private float time;
    private float timeMultiplier = .7f;
    private float savedTime;
    private bool backforth = true;

    private void Start()
    {
        player = transform.root.gameObject.GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        if (backforth)
        {
            time += Time.deltaTime + player.playerSpeed * Time.deltaTime;
            if (time >= 1f / timeMultiplier)
            {
                backforth = false;
            }
        }
        else
        {
            time -= Time.deltaTime + player.playerSpeed * Time.deltaTime;
            if (time <= 0f / timeMultiplier)
            {
                backforth = true;
            }
        }
        
        
        if (player.playerSpeed > 0f)
        {
            Vector2 pos = new Vector2(Mathf.Lerp(startPos.x, endPos.x, swayXCurve.Evaluate(time * timeMultiplier)), startPos.y + (swayXCurve.Evaluate(time * timeMultiplier) * 20f));
            uiHand.anchoredPosition = pos;
        }
        else
        {
            Vector2 pos = new Vector2(Mathf.Lerp(startPos.x, endPos.x, swayXCurve.Evaluate(savedTime)), startPos.y + (swayXCurve.Evaluate(savedTime) * 35f));
            uiHand.anchoredPosition = pos;
        }
        

    }
}
