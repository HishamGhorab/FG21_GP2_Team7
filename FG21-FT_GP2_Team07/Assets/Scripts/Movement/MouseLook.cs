using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivityX = 5f;
    public float mouseSensitivityY = 0.5f;
    private float mouseX, mouseY;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private float xClamp = 85f;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

     void Update()
     {
         if (Cursor.lockState == CursorLockMode.None)
             return;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        transform.Rotate(Vector3.up, mouseX);
        
       Vector3 targetRotation = transform.eulerAngles;
       targetRotation.x = xRotation;
       playerCamera.eulerAngles = targetRotation;
       
       
     }
    
    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * mouseSensitivityX;
        mouseY = mouseInput.y * mouseSensitivityY;
    }

    
    }

