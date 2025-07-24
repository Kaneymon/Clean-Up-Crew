using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using Steamworks;
public class PlayerController : NetworkBehaviour, IDamageable
{
    private CharacterClass characterClass;
    private PlayerInventory inventory;

    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private float speed = 6f;
    private float speedFactor = 1f;
    [SerializeField]
    private float jumpModifier = 10f;
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    LayerMask groundLayer;

    float horMovement;
    float verMovement;
    float mouseX;
    float mouseY;
    float mouseSensitivity = 100;
    bool grounded;
    bool canJump = true;
    float health = 100;


    //------------------------------- Initialization stuff --------------------------------

    private void Start()
    {
        Setup();
    }

    public override void OnStartClient()
    {
        if (base.IsOwner)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }
    }


    // Call this from network spawn logic
    public void Setup()
    {
        characterClass = CharacterClass.GetRandomClass();
        UpdateClassStats();
        rigidBody = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
    }

    private void UpdateClassStats()
    {
        speed = characterClass.WalkSpeed * speedFactor;
        health = characterClass.Health * speedFactor;
    }

    public void SetPlayerClass(CharacterClass newClass)
    {
        characterClass = newClass;
        UpdateClassStats();
        print($"player class is now {newClass.Name}! stats-updated.");
    }

    //-------------------- Getters ---------------------------
    public PlayerInventory GetInventory() { return inventory; }


    //------------------------------------ MOVEMENT METHODS ------------------------------------
    public void Jump()
    {

            JumpLocal();

    }

    [ServerRpc]
    private void JumpServer()
    {
        JumpLocal();
    }
    private void JumpLocal()
    {
        if (!canJump) { return; }
        rigidBody.AddForce(playerBody.transform.up * CalculateJumpForce(), ForceMode.Impulse);
        canJump = false;
        Invoke("ResetJump", 0.5f);
    }

    private void ResetJump()
    {
        canJump = true;
    }
    public void Movement(Vector2 input)
    {

            MovementLocal(input);

    }

    [ServerRpc]
    private void MovementServer(Vector2 input)
    {
        MovementLocal(input);
    }
    private void MovementLocal(Vector2 input)
    {
        Vector3 move = playerBody.transform.right * input.x + playerBody.transform.forward * input.y;

        if (!rigidBody.SweepTest(move.normalized, out RaycastHit hit, 0.5f)) //here to prevent Tunnelling on lowFPS
        {
            rigidBody.MovePosition(playerBody.transform.position + (move * speed * Time.fixedDeltaTime));
        }
        else            
        {
            Vector3 slideVector = Vector3.ProjectOnPlane(move, hit.normal);

            if (!rigidBody.SweepTest(slideVector.normalized, out RaycastHit hitTwo, 0.5f)) //prevent tunnelling in actual movement vector
            {
                rigidBody.MovePosition(playerBody.transform.position + (slideVector * speed * Time.fixedDeltaTime));
            }
        }
    }



    private float xRotation = 0f;

    public void CameraLook(Vector2 lookInput)
    {
        if (!IsOwner) return;

        // Rotate the camera up/down (pitch)
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the body left/right (yaw) using physics
        Quaternion deltaRotation = Quaternion.Euler(0f, lookInput.x, 0f);
        rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
    }

    public void Crouch()
    {
        // Camera + animation changes
    }

    public void SetSpeedSprint()
    {
        speed = characterClass.RunSpeed * speedFactor;
    }

    public void SetSpeedWalk()
    {
        speed = characterClass.WalkSpeed * speedFactor;
    }


    //-------------------------------- ACTION METHODS / MISC BEHAVIOUR STUFF -----------------------
    //----------------------------------------------------------------------------------------------
    public void Interact()
    {
        // Raycast or spherecast to detect IInteractable
    }

    public void Actions()
    {
        // Use IActionable interface from equipped item
    }


    public void Emote()
    {
        // Open emote wheel or trigger emote animation
    }



    //------------------------------------Calculation methodssss!--------------------------------------

    private float CalculateJumpForce()
    {
        // Return jump force considering encumberment
        return characterClass.JumpForce * jumpModifier * (1f - GetEncumbermentFactor());
    }

    private float GetEncumbermentFactor()
    {
        // Get this from CharacterInventory system
        return 0f; // Placeholder
    }




    
    //-------------------------------------DEATH AND DAMAGEEEE----------------------------------------
    bool isDead = false;
    [ServerRpc]
    public void TakeDamage(float damage)
    {
        // IDamageable implementation, reduce health, sync over network
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    [Server]
    private void Die()
    {
        isDead = true;
        //change player to spectator.
        HandleDeathEffects();
    }

    [ObserversRpc]
    private void HandleDeathEffects()
    {
        //play any sfx or vfx.
        //maybe turn player into ragdoll.
    }


    //-----------------------------SERVER STUFF---------------------------------
    public override void OnStartServer()
    {
        base.OnStartServer();
        PlayerData thisPlayersData = new PlayerData(OwnerId, this, SteamFriends.GetPersonaName().ToString());
        PlayerManager.Instance?.RegisterPlayer(OwnerId, thisPlayersData);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        PlayerManager.Instance?.UnregisterPlayer(OwnerId);
    }
}








/*using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

public class PlayerControllerDeprecated : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private float speed = 6f;
    private float speedFactor = 1f;
    [SerializeField]
    private float jumpForce = 35f;
    [SerializeField]
    private bool clientAuth = true;
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    LayerMask groundLayer;

    float horMovement;
    float verMovement;
    float mouseX;
    float mouseY;
    float mouseSensitivity = 100;
    bool grounded;
    bool canJump = true;


    private void Awake()
    {
        Debug.Log(transform.position);
    }
    public override void OnStartClient()
    {
        if (base.IsOwner)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }
    }

    private void Update()
    {
        if (!base.IsOwner)
            return;

        GetInput();
        GroundCheck();

        /* If ground cannot be found for 20 units then bump up 3 units. 
         * This is just to keep player on ground if they fall through
         * when changing scenes. */

/*
        if (clientAuth || (!clientAuth && base.IsServerStarted))
        {
            if (!Physics.Linecast(transform.position + new Vector3(0f, 0.3f, 0f), transform.position - (Vector3.one * 20f)))
                transform.position += new Vector3(0f, 10f*Time.deltaTime, 0f);
        }

        if (clientAuth)
        {
            Move(horMovement, verMovement);
            Look(mouseX, mouseY);
        }
        else
        {
            ServerMove(horMovement, verMovement);
            ServerLook(mouseX, mouseY);
        }

    }

    [ServerRpc]
    private void ServerMove(float _hor, float _ver)
    {
        Move(_hor, _ver);
    }

    [ServerRpc]
    private void ServerLook(float _x, float _Y)
    {
        Look(_x, _Y);
    }

    [ServerRpc]
    private void ServerJump(float jumpForce)
    {
        Jump(jumpForce);
    }

    private void Move(float _hor, float _ver)
    {
        //implement add force movement here
        //forward = camer.transform.forward, 


        Vector3 move = playerBody.transform.right * _hor + playerBody.transform.forward * _ver;
        rigidBody.MovePosition( playerBody.transform.position + (move * speed * speedFactor * Time.deltaTime));
    }

    float xRotation = 0;
    private void Look(float _x, float _Y)
    {
        //implement camera and body rotation here
        xRotation -= _Y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.transform.Rotate(Vector3.up * _x);
    }

    private void GetInput()
    {
        //get movement input:
        horMovement = Input.GetAxisRaw("Horizontal");
        verMovement = Input.GetAxisRaw("Vertical");
        //get camera input:
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Input key events:
        if (Input.GetButton("Jump") && canJump && grounded){

            if (clientAuth)
                Jump(jumpForce);
            else
                ServerJump(jumpForce);
        }

        if (Input.GetButtonDown("Sprint"))
        {
            speedFactor = 1.6f;
        }
        else
        {
            speedFactor = 1;
        }
    }

    private void Jump(float _jumpForce)
    {
        if (!canJump){ return; }

        rigidBody.AddForce(playerBody.transform.up * _jumpForce, ForceMode.Impulse);
        canJump = false;
        Invoke("ResetJump", 0.5f);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(playerBody.transform.position + new Vector3(0,0.1f,0), Vector3.down, 0.5f, groundLayer);
        print(grounded + " " + canJump);
    }

}

*/