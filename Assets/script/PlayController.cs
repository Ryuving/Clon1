using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followCamera;

    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;



    public Animator animator;

    public static PlayController instance;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (ChekWinner.instance.isWinner)
        {
            case true:
                animator.SetBool("Victory", ChekWinner.instance.isWinner); 
                break;
            case false:
                Movement();
                break;
        }
    }

    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if(characterController.isGrounded && playerVelocity.y < -2)
        {
            playerVelocity.y = -1;
        }

        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0,followCamera.transform.eulerAngles.y,0)
                                 * new Vector3(HorizontalInput,0,VerticalInput);

        Vector3 movementDirection = movementInput.normalized;

        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);

        if(movementDirection != Vector3.zero) { 
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,desiredRotation,rotationSpeed * Time.deltaTime);
        }

        if(Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
            animator.SetTrigger("Jump");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", characterController.isGrounded);
            
    }
}
