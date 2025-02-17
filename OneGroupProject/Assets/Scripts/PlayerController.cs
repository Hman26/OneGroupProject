using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    public float walkSpeed = 7f;
    public float jumpSpeed = 7f;
    public float gravity = 10f;

    // Camera Variables
    public Camera playerCamera;
    public float lookSpeed = -1f;
    public float lookLimitX = 10f;

    private float rotationX = 1; 

    // Player Variables
    public int health;
    //public GameObject player;

    // Enemy Variables
    public int collectedPickups; 

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        // Local Vector Variables used to store
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Local Float Variables to store the current Horizontal direction of our player
        float currSpeedX = walkSpeed * Input.GetAxis("Vertical");
        float currSpeedY = walkSpeed * Input.GetAxis("Horizontal");

        // Local Floart Variable to store the current vertical direction of our player 
        float jumpDirectionY = moveDirection.y;

        // Calculate movement vector based on our speed varaibles for moving forward and side to side
        moveDirection = (forward * currSpeedX) + (right * currSpeedY);

        //Add vertical movement to our player if the player is on the ground and pressed the jump
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        // Stop adding vertical movement while the player is not jumping 
        else
        {
            moveDirection.y = jumpDirectionY;
        }

        // If the player is not on the ground subtract our gravity force from vertical movement 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else if (characterController.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = 0;
        }

        // Apply our final move directon to the player in game using the built in character controller move function 
        characterController.Move(moveDirection * Time.deltaTime);
        
        // ---------------- Camera --------------------
        if (Input.GetKeyDown(KeyCode.G))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
        }

        rotationX += Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookLimitX, lookLimitX);

        playerCamera.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0); 
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            collectedPickups += 1; 
            Debug.Log(collectedPickups.ToString());
            other.gameObject.SetActive(false); 
            // why is Destroy function not workin? 
            Destroy(other);
        }
    }
}
