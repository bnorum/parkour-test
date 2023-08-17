using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController Controller;
    public float Speed;
    public Transform Cam;

    public float jumpForce = 5.0f; 
    public float gravity = 9.81f; // Gravity force
    public float groundedRaycastDistance = 0.9f; 
    public bool isGrounded = true;
    public int maxJumps = 2; //doesnt include initial jump
    private int jumpsRemaining;

    private Vector3 velocity;


    public float dashSpeedMultiplier = 2.0f; 
    public float dashDuration = 0.5f; 
    public float dashCooldown = 2.0f; 
    private float lastDashTime = -999f;


    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //movemnt
        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;

        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        Movement.y = 0f;

        Controller.Move(Movement);

        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraMove>().sensivity * Time.deltaTime);

            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
        }//if
        //end movement

        // Jumping
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundedRaycastDistance);
        if (isGrounded)
        {
            velocity.y = -0.5f;
            jumpsRemaining = maxJumps;

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;

            if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
            {
                Jump();
                jumpsRemaining--;
            }
        }
        //end jumping

        // Dash
        if (Input.GetButtonDown("Fire1") && Time.time > lastDashTime + dashCooldown)
        {
            lastDashTime = Time.time;
            StartCoroutine(Dash());
        }
        //enddash


        

        Controller.Move(velocity * Time.deltaTime);
    }//update

   
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(2 * jumpForce * gravity);
    }
    private IEnumerator Dash()
    {
        float originalSpeed = Speed;
        Speed *= dashSpeedMultiplier;

        yield return new WaitForSeconds(dashDuration);

        Speed = originalSpeed;
    }//dash

}
