using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbortMinigame : MonoBehaviour
{
    private Canvas canvas;
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void ExitMinigame()
    {
        canvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
