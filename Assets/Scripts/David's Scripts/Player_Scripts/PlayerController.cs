using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    protected static PlayerController s_Instance;
    public static PlayerController instance { get { return s_Instance; } }

    public bool respawning { get { return m_Respawning; } }

    public float moveSpeed;

    public float slopeForce;
    public float slopeForceRayLenght;

    public CharacterController controller;
    public float jumpForce;
    public float gravityScale;

    private const int maxJump = 2;
    private int currentJump = 0;
    private Vector3 moveDirection;

    //Animation
    public Animator anim;
    public Transform pivot;
    public float rotateSpeed;
    public GameObject playerModel;

    //detec ground and slope
    private bool isGrounded; 
    public float slideFriction = 0.3f; 
    private Vector3 hitNormal;

    protected bool m_Respawning; //is respawning?

    // Use this for initialization
    void Awake()
    {
        s_Instance = this;
        controller = GetComponent<CharacterController>();

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private bool OnSlope()
    {
        if (currentJump > 0)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLenght))
        {
            if (hit.normal != Vector3.up)
                return true;
        }

        return false;
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * slopeForceRayLenght, Color.yellow);
        // moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);
// Debug.Log("currentJump" + currentJump);
        float yStore = moveDirection.y;

        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal") );
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        //isGrounded = Vector3.Angle(Vector3.up, hitNormal) <= controller.slopeLimit;

        if (controller.isGrounded)
        {
            currentJump = 0;
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
                currentJump++;
            }

        }
        else
        {

            moveDirection.x += (1f - hitNormal.y) * hitNormal.x * (moveSpeed - slideFriction);
            moveDirection.z += (1f - hitNormal.y) * hitNormal.z * (moveSpeed - slideFriction);
            switch (currentJump)
            {
                case 0:
                    if (maxJump > currentJump)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            moveDirection.y = jumpForce;
                            currentJump=2;
                            anim.SetTrigger("DubleJump");
                        }
                    }
                    break;
                case 1:
                    if (maxJump > currentJump)
                    {
                        if (Input.GetButtonDown("Jump"))
                        {
                            moveDirection.y = jumpForce;
                            currentJump++;
                            anim.SetTrigger("DubleJump");
                        }
                    }
                    break;
            }
        }



        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);

       if((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && OnSlope())
        {
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
        }

        //attack
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }

        //Move the player in different direction based on the camera direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed*Time.deltaTime);
        }

        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("SpeedProcent", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }

    //public void RespawnFinished()
    //{
    //    m_Respawning = false;
    //    m_Damageable.isInvulnerable = false;
    //}
}
