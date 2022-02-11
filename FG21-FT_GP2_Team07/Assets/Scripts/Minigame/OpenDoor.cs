using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private bool positiveRotation;
    void Update()
    {
        int dir = positiveRotation ? 1 : -1;
        if (GetComponentInChildren<ActivateMinigame>().Active)
        {
            transform.rotation = Quaternion.Euler(0, 10 * dir, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
