using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialVisibility : MonoBehaviour
{
    public SpriteRenderer rend;
    public float visibleTime;
    private float visibleTimer = 0.0f;
    public Vector2 goneTimeLimits;
    public float goneTime;
    private float goneTimer = 0.0f;
  
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = false;
        goneTime = Random.Range(goneTimeLimits.x, goneTimeLimits.y); 
    }

    private void Update()
    {
        goneTimer += Time.deltaTime;

        if (goneTimer >= goneTime)
        {
            Flicker();
        }
    }

    private void Flicker()
    {
        rend.enabled = true;
        visibleTimer += Time.deltaTime;

        if (visibleTimer >= visibleTime)
        {
            rend.enabled = false;
            goneTimer = 0.0f;
            visibleTimer = 0.0f;
            goneTime = Random.Range(goneTimeLimits.x, goneTimeLimits.y);
        }
    }


}
