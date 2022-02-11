using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    public SpriteRenderer rend;
    public float minSeconds = 2.0f;
    public float maxSeconds = 5.0f;
    private float timer = 0.0f;
    private float visibilityTimer;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = true;
        visibilityTimer = Random.Range(minSeconds, maxSeconds);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= visibilityTimer)
        {
            rend.enabled = !rend.enabled;
            visibilityTimer = Random.Range(minSeconds, maxSeconds);
            timer = .0f;
        }
    }


    IEnumerator ToggleVisibility()
    {
        if (rend.enabled == true)
        {

            yield return new WaitForSeconds(Random.Range(minSeconds, maxSeconds));
            rend.enabled = false;


        }
        else if (rend.enabled == false)
        {
            yield return new WaitForSeconds(Random.Range(minSeconds, maxSeconds));
            rend.enabled = true;
        }

    }
}
