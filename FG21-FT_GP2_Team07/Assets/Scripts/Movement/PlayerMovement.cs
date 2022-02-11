using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;


public class PlayerMovement : MonoBehaviour
{
    public float SpeedModifier { get => speedModifier; set => speedModifier = value; }

    [SerializeField] private CharacterController controller;
    [SerializeField] private float defaultSpeed = 5f;
    
    
    [SerializeField] float gravity = -30f;
    [SerializeField] private LayerMask groundMask;
    
    private AudioSource audioSource;
    private bool IsMoving;
    private SoundComponent soundComponent;
     
    
    private Vector2 horizontalInput;
    Vector3 verticalVelocity = Vector3.zero;

    private bool jump;
    private bool isGrounded;
    
    private float speedModifier;
    public float playerSpeed;
    

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        soundComponent = GetComponent<SoundComponent>();
    }
    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;
        
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * (defaultSpeed + speedModifier);
        controller.Move(horizontalVelocity * Time.deltaTime);
        playerSpeed = (horizontalInput.y * (defaultSpeed + speedModifier)) / (defaultSpeed + speedModifier);

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
        Footsteps();
    }

    public void ReceiveInput(Vector2 inputHorizontal) // recevises the horiozontalinput 
    {
        horizontalInput = inputHorizontal;
        //if (horizontalInput != Vector2.zero)
        // print(horizontalInput); 
    }
    

    void Footsteps()
    {
        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 )
        {
            IsMoving = true; 
        }
        else IsMoving = false;


        if (IsMoving && !audioSource.isPlaying) //&& !audioSource.isPlaying 
        {
            audioSource.volume = Random.Range(0.5f, 7f);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            soundComponent.PlaySound("FootstepsWood");
        }

        if (!IsMoving)
        {
            audioSource.Stop();
        }
    }

}