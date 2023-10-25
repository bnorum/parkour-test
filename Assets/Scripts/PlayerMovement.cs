using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem runDust;
    public ParticleSystem jumpDust;

    public ParticleSystem slamDust;

    [Header("Movement")]
    CharacterController Controller;
    private float ogSOffset;
    public float Speed;
    public Transform Cam;
    private Vector3 Movement;

    [Header("Jumping")]
    public float jumpForce = 5.0f; 
    public float gravity = 9.81f; // Gravity force
    public float groundedRaycastDistance = 0.9f; 
    public bool isGrounded = true;
    public int maxJumps = 2; //doesnt include initial jump
    private int jumpsRemaining;
    public mushJump MushJump;
    private Vector3 vertVelocity;

    

    [Header("Dash")]
    private bool isDashing = false;
    public float dashSpeedMultiplier = 2.0f; 
    public float dashDuration = 0.5f; 
    public float dashCooldown = 2.0f; 
    public GameObject dashlines;
    public Slider visualCooldownDash;
    public LayerMask groundMask;
    [Header("Slam")]
    public GameObject slamHitbox;
    public float slamSpeed;

    [Header("Animation")]
    public Animator rightLeg;
    public Animator leftLeg;
    public Transform modelFaceDir;

    [Header("Audio")]
    public PlayerSound playerSound;

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
            CreateDust();
            rightLeg.SetBool("isMoving", true);
            leftLeg.SetBool("isMoving", true);
        } else {
            rightLeg.SetBool("isMoving", false);
            leftLeg.SetBool("isMoving", false);
        } //leg animation

        Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical; //w relation to camera
        Movement.y = 0f; //reset y bc camera has y value

        if (Horizontal != 0 || Vertical != 0) {
            modelFaceDir.rotation = Quaternion.LookRotation(Movement);
            modelFaceDir.Rotate(0, 90, 0);
        }//animate rotation
        Controller.Move(Movement); //horizontal movement

        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<CameraMove>().sensivity * Time.deltaTime);
            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
        }//character rotation

        bool isSlamming = Physics.Raycast(transform.position, Vector3.down, 1.2f);
        if (isSlamming && vertVelocity.y < -20) StartCoroutine(SlamFX());

        



        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundedRaycastDistance, groundMask);
        if (isGrounded)
        {
            if (!MushJump.getInMush()) vertVelocity.y = -0.5f;
            Controller.stepOffset = ogSOffset;
            jumpsRemaining = maxJumps;
            rightLeg.SetBool("isJumping", false);
            leftLeg.SetBool("isJumping", false);
        }//reset jumps
        else
        {
            vertVelocity.y -= gravity * Time.deltaTime;
            Controller.stepOffset = 0;
            rightLeg.SetBool("isJumping", true);
            leftLeg.SetBool("isJumping", true);
        }//grav and jump reset

        if (Input.GetButtonDown("Jump"))
        {
            Jump(false);
        }

        if (dashCooldown < 2) dashCooldown += Time.deltaTime;
        visualCooldownDash.value = dashCooldown;

        if (Input.GetButtonDown("Fire1") && dashCooldown >= 2) StartCoroutine(Dash());
    
        if(Input.GetButtonDown("Fire2") && !isGrounded) Slam();
            
        Controller.Move(vertVelocity * Time.deltaTime);
    }//update
    
    public void Jump(bool isMush)
    {   
        if (jumpsRemaining >= 0){
            int mushbonus = 0;
            playerSound.PlayJumpSound(!isGrounded);
            if (!isGrounded) CreateJumpDust();
            if (isMush)  mushbonus = 10; 
            vertVelocity.y = Mathf.Sqrt(2 * jumpForce * gravity + mushbonus);
            isGrounded = false;
            jumpsRemaining--;
        }
    }//jump

    public void Slam() {
        vertVelocity.y = -slamSpeed;
    }
    public IEnumerator SlamFX() {
        Debug.Log("Bang");
        slamHitbox.SetActive(true);
        slamDust.Play();
        yield return new WaitForSeconds(.2f);
        slamHitbox.SetActive(false);
    }

    private IEnumerator Dash()
    {
        if (dashCooldown >= 2) {
            dashCooldown = 0;
            float originalSpeed = Speed;
            Speed *= dashSpeedMultiplier;
            dashlines.SetActive(true);
            isDashing = true;
            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
            dashlines.SetActive(false);
            Speed = originalSpeed;
        }
    }//dash

    void CreateDust(){
        runDust.Play();
    }
    void CreateJumpDust(){
        jumpDust.Play();
    }

    public float getVelX(){
        return Movement.x;
    }
    public float getVelZ(){
        return Movement.z;
    }
    public bool getIsGrounded(){
        return isGrounded;
    }
    public bool getIsDashing(){
        return isDashing;
    }

}

