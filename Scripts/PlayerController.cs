using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Dictionary<int, PlayerController> Players = new Dictionary<int, PlayerController>();

    internal float MoveSpeed = 4f;
    public bool canMove = true;
    internal bool isMoving = false;
    private float rotateSpeed = 7f;
    internal bool isSpearActive = true;

    private float gravity = 30f;

    private CharacterController characterController;
    private Rigidbody rigid;
    
    internal Animator anim;

    public virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        Players.Add(0, gameObject.GetComponent<PlayerController>());
    }

    public virtual void Update()
    {
        Movement();
    }

    internal void Movement()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            isMoving = (horizontal != 0 || vertical != 0) ? true : false;
            anim.SetBool("isMoving", isMoving);

            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            if (moveDirection.magnitude > 0)
            {
                transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
                Vector3 offset = moveDirection * MoveSpeed * Time.deltaTime;

                if (!characterController.isGrounded)
                {
                    offset.y -= gravity * Time.deltaTime;
                }

                characterController.Move(offset);
            }
            else
            {
                if (!characterController.isGrounded)
                {
                    Vector3 gravityOffset = new Vector3(0, -gravity * Time.deltaTime, 0);
                    characterController.Move(gravityOffset);
                }
            }
        }
    }

    public void MovementCooldown()
    {
        canMove = true;
    }
}
