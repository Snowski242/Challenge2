using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CappiController : MonoBehaviour
{
    [Header("Controller Additions")]
    public CharacterController characterController;
    public Transform cam;

    
    public Animator animator;

    public string state = "idle";

    //[HideInInspector]
    public bool canMove = true;
    public bool canAttack = true;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool hanging = false;

    public float speed = 6f;
    public float jump = 12f;
    public float rollSpeed = 8f;
    private bool canDodgeRoll = true;
    public float dashTime;

    [Header("Ground Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f; //checks if there is a ground
    public LayerMask groundMask;
    //[HideInInspector]
    public bool isGrounded;

    [Header("Simulated Gravity Settings")]
    public Vector3 transformVelocity; //this applies the gravity and causes the player to fall
    public float gravity = -15.81f; //how heavy gravity gets
    [HideInInspector]
    public float turnSmoothTime = 0.1f; //allows for smoother turning of the model
    float turnSmoothVelocity;

    [Header("Animations")]
    private string currentState;
    public const string PLAYER_IDLE = "CappiStance";
    const string PLAYER_BATTLE_IDLE = "CappiBMStance";
    const string PLAYER_RUN = "CappiWalk";
    const string PLAYER_JUMP = "CappiJumpStart";
    const string PLAYER_FALLING = "CappiJump";
    const string PLAYER_ROLL = "CappiRoll";
    const string PLAYER_AIR_ATTACK = "Player_air_attack";

    void Start()
    {
        //animator = GetComponent<Animator>();
    }


    
    void Update()
    {
        //ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && transformVelocity.y < 0)
        {
            transformVelocity.y = -10f;
        }

        //movement (hanging prevents movement)
        if(!hanging && canDodgeRoll)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            //handles turning
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (canMove)
            {
                if (direction.magnitude >= 0.1f)
                {
                    float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    characterController.Move(moveDir.normalized *Time.deltaTime * speed);
                }

            }
        }
        
        States();
        
        LedgeGrab();
        

        if(!hanging)
        {
            transformVelocity.y += gravity * Time.deltaTime;
        }
        
        if(canDodgeRoll && canMove)
        {
            characterController.Move(transformVelocity * Time.deltaTime);
        }
        

        if(Input.GetButtonDown("Jump") && isGrounded && canDodgeRoll && PlayerManager.instance.dontJump == false)
        {
                transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                state = "jump";
        }
        else if(Input.GetButtonDown("Jump") && hanging && canDodgeRoll && PlayerManager.instance.dontJump == false)
        {
            hanging = false;
            
            transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
            state = "jump";
        }
    }

    private void States()
    {
        if (!hanging && canDodgeRoll)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal == 0 && vertical == 0 && isGrounded && !isAttacking && canAttack)
            {
                state = "idle";
                
            }
            if (horizontal != 0 && isGrounded|| vertical != 0 && isGrounded)
            {
                state = "walk";
                
            }

            if(state == "walk" && Input.GetMouseButtonDown(1) && isGrounded && canDodgeRoll)
            {
                canDodgeRoll = false;
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                characterController.SimpleMove(forward * rollSpeed);
                StartCoroutine(Rolling());
            }
        }
        
        

        
        if (!isGrounded && !isAttacking && transformVelocity.y > 0)
        {
            state = "jump";
        }

        if (!isGrounded && !isAttacking && transformVelocity.y < 0)
        {
            state = "fall";
        }

        if (isGrounded && isAttacking && canDodgeRoll)
        {
            state = "attack ground";
        }
    }

    private void Animations()
    {
        if (state == "idle")
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Jump", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Walk", false);

        }

        if (state == "walk")
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Jump", false);
            animator.SetBool("Falling", false);
            animator.SetBool("Idle", false);

        }

        if (state == "attack ground")
        {
            
            
        }

        if (state == "jump")
        {
            animator.SetBool("Jump", true);
        }
        

        if (state == "fall")
        {
            animator.SetBool("Falling", true);
            animator.SetBool("Jump", false);
        }
       
    }

  

    private IEnumerator Rolling()
    {
        canDodgeRoll = false;
        canAttack = false;
        canMove = false;
        animator.SetBool("Rolling", true);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            characterController.SimpleMove(forward * rollSpeed);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.01f);
        animator.SetBool("Rolling", false);
        canMove = true;
        canDodgeRoll = true;
        canAttack = true;

    }
    void LedgeGrab()
    {
        if(transformVelocity.y < 0 && !hanging && !isGrounded)
        {
            RaycastHit downHit;
            Vector3 lineDownStart = transform.position + Vector3.up * 8.5f + transform.forward * 2.1f;
            Vector3 lineDownEnd = transform.position + Vector3.up * 7.7f + transform.forward * 2.1f;
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, LayerMask.GetMask("Ground"));
            Debug.DrawLine(lineDownStart, lineDownEnd);

            //shoots raycast forward to see if theres a raycast hit
            if(downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y-0.1f, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward * 4f;
                Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, LayerMask.GetMask("Ground"));
                Debug.DrawLine(lineFwdStart, lineFwdEnd);

                if (fwdHit.collider != null) //shoots a smaller raycast forward to see if its a ledge
                {
                    transformVelocity.y = 0;
                    transformVelocity = Vector3.zero;

                    hanging = true;
                    state = "hanging";
                    
                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                }
            }
        }
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }

        animator.Play(newState);

        currentState = newState;
    }
}
