using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    CharacterController Controller;
    private float ogSOffset;
    public float Speed;
    public Transform Cam;

    public float jumpForce = 5.0f; 
    public float gravity = 9.81f; // Gravity force
    public float groundedRaycastDistance = 0.9f; 
    public bool isGrounded = true;
    public int maxJumps = 2; //doesnt include initial jump
    private int jumpsRemaining;
    public mushJump MushJump;

    private Vector3 velocity;

    //dash
    public float dashSpeedMultiplier = 2.0f; 
    public float dashDuration = 0.5f; 
    public float dashCooldown = 2.0f; 
    public GameObject dashlines;
    public Slider visualCooldownDash;
    public LayerMask groundMask;

    public Animator rightLeg;
    public Animator leftLeg;
    public Transform modelFaceDir;

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        ogSOffset = Controller.stepOffset;
    }

    void Update()
    {
        //movemnt
        float Horizontal = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        if ((Horizontal != 0 || Vertical != 0) && isGrounded) {
            rightLeg.SetBool("isMoving", true);
            leftLeg.SetBool("isMoving", true);
        } else {
            rightLeg.SetBool("isMoving", false);
            leftLeg.SetBool("isMoving", false);
        }

        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;
        Movement.y = 0f;
        if (Horizontal != 0 || Vertical != 0) {
            modelFaceDir.rotation = Quaternion.LookRotation(Movement);
            modelFaceDir.Rotate(0, 90, 0);
        }
        Controller.Move(Movement);

        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraMove>().sensivity * Time.deltaTime);

            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
        }//character rotation

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundedRaycastDistance, groundMask);
        if (isGrounded)
        {
            if (!MushJump.getInMush()) velocity.y = -0.5f;
            Controller.stepOffset = ogSOffset;
            jumpsRemaining = maxJumps;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
            Controller.stepOffset = 0;
        }//grav and jump reset

        if (Input.GetButtonDown("Jump"))
        {
            Jump(false);
            jumpsRemaining--;
        }

        if (dashCooldown < 2) {
            dashCooldown += Time.deltaTime;
            
        }
        visualCooldownDash.value = dashCooldown;

        if (Input.GetButtonDown("Fire1") && dashCooldown >= 2)
        {
            StartCoroutine(Dash());
        }//dash input
        Controller.Move(velocity * Time.deltaTime);
    }//update
    
    public void Jump(bool isMush)
    {   
        int mushbonus = 0;
        if (isMush)  {mushbonus = 10; jumpsRemaining --;}
        if (jumpsRemaining >= 0 || isMush){
            velocity.y = Mathf.Sqrt(2 * jumpForce * gravity + mushbonus);
            isGrounded = false;
        }
    }//jump

    private IEnumerator Dash()
    {
        if (dashCooldown >= 2) {
            dashCooldown = 0;
            float originalSpeed = Speed;
            Speed *= dashSpeedMultiplier;
            dashlines.SetActive(true);
            yield return new WaitForSeconds(dashDuration);
            dashlines.SetActive(false);
            Speed = originalSpeed;
        }
    }//dash




}



