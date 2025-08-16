using FishNet.Object;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : NetworkBehaviour
{
    private PlayerController player;
    private PlayerInteraction interactions;
    //private ChatBehaviour chatBehaviour;
    private Vector2 movementInput;
    private Vector2 lookInput;

    private bool jumpPressed;
    private bool crouchPressed;
    private bool sprintPressed;
    private bool actionPressed;
    private bool interactPressed;
    private bool emotePressed;
    private bool openChatPressed;
    private bool sendMessagePressed;
    private bool closeMenusPressed;
    private bool OpenSettingsPressed;

    private bool inputEnabled = true;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        interactions = GetComponent<PlayerInteraction>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (!base.IsOwner) return; // Only process input for the local player

        HandleInput();
        SendInputToCharacter();
        SendInputToMisc();
    }

    private void FixedUpdate()
    {
        TogglePlayerInput();
        if (!inputEnabled) return;

        if (!base.IsOwner) return;

        player.Movement(movementInput); // seeing if this reduces tunelling, RigidBody.Moveposition is designed for fixed updated appaz
    }
    private void HandleInput()
    {
        // Movement and Look
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Actions
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        crouchPressed = Input.GetKey(KeyCode.LeftControl);
        sprintPressed = Input.GetKey(KeyCode.LeftShift);
        actionPressed = Input.GetMouseButtonDown(0); // Primary action
        actionPressed = Input.GetMouseButtonDown(1); // Primary action
        interactPressed = Input.GetKeyDown(KeyCode.E);
        emotePressed = Input.GetKeyDown(KeyCode.G);
        openChatPressed = Input.GetKeyDown(KeyCode.T);
        sendMessagePressed = Input.GetKeyDown(KeyCode.Return);
        closeMenusPressed = Input.GetKeyDown(KeyCode.Escape);
        OpenSettingsPressed = Input.GetKeyDown(KeyCode.P);
    }

    private void SendInputToCharacter()
    {
        if(!inputEnabled)
        {
            DisableInputBools();
            return;
        }

        
        player.CameraLook(lookInput);

        if (jumpPressed) player.Jump();
        if (crouchPressed) player.Crouch();
        if (actionPressed) player.Actions();
        if (interactPressed) interactions.TryInteract();
        if (emotePressed) player.Emote();
        if (sprintPressed) player.SetSpeedSprint();
        else { player.SetSpeedWalk(); }
        
    }

    private void SendInputToMisc()
    {
        if (openChatPressed && !ChatBehaviour.instance.menuActiveState)
        {
            UiManager.instance.OpenMenu(ChatBehaviour.instance);
        }
        if (sendMessagePressed)
        {
            ChatBehaviour.instance.TrySendMessage();
        }
        if (closeMenusPressed)
        {
            UiManager.instance.CloseTopMenu();      
        }
        if (OpenSettingsPressed) { UiManager.instance.OpenSettingsMenu(); }
    }

    private void DisableInputBools()
    {
        jumpPressed = false;
        crouchPressed = false;
        sprintPressed = false;
        actionPressed = false;
        actionPressed = false;
        interactPressed = false;
        emotePressed = false;
        //openCloseChatPressed = false; this shouldnt be disabled.
    }

    public void SetActivePlayerInputs(bool state)
    {
        inputEnabled = state;
        Cursor.visible = !state;
    }

    private void TogglePlayerInput()
    {
        //keep adding Active state getter checks to the first condition.
        if (UiManager.instance.IsAFreezingMenuOpen())
        {
            inputEnabled = false;
            Cursor.visible = true;
        }
        else
        {
            inputEnabled = true;
            Cursor.visible = false;
        }
    }
}
