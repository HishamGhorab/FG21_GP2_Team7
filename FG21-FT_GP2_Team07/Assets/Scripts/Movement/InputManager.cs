using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   [SerializeField] private PlayerMovement playermovement; //reference to movement script
   [SerializeField] private MouseLook mouseLook; //reference to mouselook script
   
   private PlayerControls controls;
   private PlayerControls.GroundMovementActions groundMovement; 

   private Vector2 horizontalInput;
   private Vector2 mouseInput;

   private Vector2 move;
   private Vector2 look;

   

   void Awake()
   {
      controls = new PlayerControls();
      groundMovement = controls.GroundMovement;
      
      //add action groundMovement.[action].performed += ctx => do someting

      groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
      
      groundMovement.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
      groundMovement.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
      
      //controller

      groundMovement.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
      groundMovement.Move.canceled += ctx => move = Vector2.zero;

      groundMovement.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
      groundMovement.Look.canceled += ctx => look = Vector2.zero;


   }

   void Update()
   {
      playermovement.ReceiveInput(horizontalInput);
      mouseLook.ReceiveInput(mouseInput);

      Vector2 m = new Vector2(-move.x, move.y) * Time.deltaTime;
      transform.Translate(m, Space.World);

      Vector2 l = new Vector2(-look.y, -look.x) * 100f * Time.deltaTime;
      transform.Rotate(l, Space.World);

   }

   private void OnEnable()
   {
      controls.Enable(); //enable playercontrols needed in new input system
   }

   private void OnDisable()
   {
      controls.Disable(); // same
   }
}
