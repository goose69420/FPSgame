using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float currentHP = 100f;
    public float maxHP = 100f;
    public bool alive = true;


    public CharacterController controller;
    public float defaultSpeed = 12f;
    [SerializeField] private float speed;
    public float sprint;
    public float crounch;
    public Vector3 move;
    

    public float gravity = -9.81f;
    public float jumpHeight = 5f;

    public Transform groundCheck;
    public Transform headCheck;
    public float groundDistant = 0.4f;
    public float standCheck = 0.5f;
    public LayerMask groundMask;

    Vector3 velocity;
    [SerializeField] float state;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isHeadBumped;
    [SerializeField] bool canStand;

    float height = 1;
    [SerializeField] bool crounched;
    [SerializeField] bool sprinting;
    [SerializeField] bool wasCrounched;
    [SerializeField] bool wasCrounchedPreviousFrame;
    [SerializeField] bool wasSprinting;

    public GameObject gameManager;
    

    // Update is called once per frame
    void Update()
    {
        Mathf.Clamp(currentHP, 0, maxHP);
        if (gameManager.GetComponent<GameControl>().paused || !alive) return;
        if (currentHP <= 0)
        {
            alive = false;
            gameManager.GetComponent<GameControl>().paused = true;
            return;
        }
        canStand = true;

        height = transform.localScale.y;
        transform.localScale = new Vector3(1, 1f, 1);
        speed = defaultSpeed;


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");



        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(1, 0.5f, 1);
            crounched = true;
            wasCrounchedPreviousFrame = true;
        }
        sprinting = (Input.GetKey(KeyCode.LeftShift) && !crounched);
        headCheck.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        isGrounded = GetComponent<CharacterController>().isGrounded;
        isHeadBumped = Physics.CheckSphere(headCheck.position, groundDistant, groundMask);
        if (isGrounded)
        {
            state = 1;
        }
        RaycastHit hit;
        canStand = !Physics.Raycast(transform.position, transform.up, out hit, 2);
        move = transform.right * x + transform.forward * z;
        if (isGrounded && !crounched)
        {
            wasCrounched = false;
        }
        if (isGrounded && !sprinting)
        {
            wasSprinting = false;
        }
        if (!Input.GetKey(KeyCode.LeftControl) && !isHeadBumped)
        {
            crounched = false;
        }

        if (!crounched && !wasCrounched)
        {
            speed = defaultSpeed;
        }

        if (!canStand && wasCrounchedPreviousFrame)
        {

            crounched = true;
            wasCrounchedPreviousFrame = true;
            transform.localScale = new Vector3(1, 0.5f, 1);
        }
        if (!isHeadBumped && !crounched)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if ((!crounched && sprinting && isGrounded) || wasSprinting)
        {
            speed = defaultSpeed * sprint;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            if (crounched)
            {
                wasCrounched = true;
                wasCrounchedPreviousFrame = true;
            }
            if (sprinting)
            {
                wasSprinting = true;
                wasCrounchedPreviousFrame = true;
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //move.x *= bunnyHopForce.x;
            //move.y *= bunnyHopForce.y;
            //move.z *= bunnyHopForce.z;
        }
        if (isHeadBumped)
        {
            velocity.y = -2f;
        }


        if (transform.position.y <= -1)
        {
            currentHP = 0;
        }
        if ((crounched && isGrounded) || wasCrounched || (crounched && isHeadBumped) || wasCrounchedPreviousFrame)
        {
            speed = defaultSpeed * crounch;
        }
        if (!isGrounded && !wasCrounched)
        {
            speed = defaultSpeed;
        }
        if (!isGrounded && wasSprinting)
        {
            speed = defaultSpeed * sprint;
        }

        if (crounched && wasCrounchedPreviousFrame && !canStand)
        {
            speed = defaultSpeed * crounch;
        }
        controller.Move(move * speed * Time.deltaTime);
        //velocityYPreviousFrame = velocity.y;

        velocity.y += gravity * Time.deltaTime;
        if (!isGrounded && Physics.Raycast(transform.position, Vector3.down, 2.5f) && state == 1 && velocity.y <= -20)
        {
            state = 0;
            currentHP += velocity.y * 2.5f;
        }


        controller.Move(velocity * Time.deltaTime);
        if (transform.localScale.y != 0.5f)
        {
            wasCrounchedPreviousFrame = false;
        }
    }
}
