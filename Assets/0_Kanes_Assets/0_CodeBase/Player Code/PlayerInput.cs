using FishNet.Object;
using UnityEngine;

public class PlayerInput : NetworkBehaviour
{
    private PlayerController player;
    private PlayerInteraction interactions;
    private Vector2 movementInput;
    private Vector2 lookInput;

    private bool jumpPressed;
    private bool crouchPressed;
    private bool sprintPressed;
    private bool actionPressed;
    private bool interactPressed;
    private bool emotePressed;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        interactions = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (!base.IsOwner) return; // Only process input for the local player

        HandleInput();
        SendInputToCharacter();
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
        emotePressed = Input.GetKeyDown(KeyCode.Q);
    }

    private void SendInputToCharacter()
    {
        player.Movement(movementInput);
        player.CameraLook(lookInput);

        if (jumpPressed) player.Jump();
        if (crouchPressed) player.Crouch();
        if (actionPressed) player.Actions();
        if (interactPressed) interactions.TryInteract();
        if (emotePressed) player.Emote();
        if (sprintPressed) player.SetSpeedSprint();
        else { player.SetSpeedWalk(); }
    }
}
