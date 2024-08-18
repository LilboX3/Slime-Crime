using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField]
    private float minJumpForce = 2f;
    [SerializeField]
    private float maxJumpForce = 6f;
    [SerializeField]
    private float chargeSpeed = 4f;

    [SerializeField]
    private LineRenderer directionRenderer;
    [SerializeField]
    private float laserWidth = 0.1f;
    [SerializeField]
    private float laserStartLength = 0.5f;
    private float currentLaserLength;
    [SerializeField]
    private float directionModifierSpeed = 0.1f;
    [SerializeField]
    private float maxDirectionModifier = 2f;

    private float currentJumpForce;
    private float currentDirectionModifier;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 jumpDirection;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentJumpForce = minJumpForce;
        directionRenderer.enabled = false;
        directionRenderer.startWidth = laserWidth;
        currentDirectionModifier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            currentJumpForce += chargeSpeed * Time.deltaTime;
            currentLaserLength += chargeSpeed * Time.deltaTime;
            currentDirectionModifier += directionModifierSpeed * Time.deltaTime;

            currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce); //Wert begrenzen zwischen min und maxForce
            currentLaserLength = Mathf.Clamp(currentLaserLength, laserStartLength, maxJumpForce); //tweak to represent distance to jump
            currentDirectionModifier = Mathf.Clamp(currentDirectionModifier, 1, maxDirectionModifier);

            float horizontalInput = Input.GetAxis("Joystick horizontal");
            float verticalInput = Input.GetAxis("Joystick vertical");
            jumpDirection = new Vector3(horizontalInput, 0, -verticalInput).normalized;

            directionRenderer.SetPosition(1, jumpDirection * (currentLaserLength/4));
            directionRenderer.enabled = true;
        }
        if (Input.GetButtonUp("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            rb.AddForce(jumpDirection * currentDirectionModifier, ForceMode.Impulse);

            if (jumpDirection != Vector3.zero) //Rotate slime in jumpdirection
            {
                Quaternion toRotation = Quaternion.LookRotation(jumpDirection, Vector3.up);
                transform.GetChild(0).rotation = toRotation;
            }

            directionRenderer.enabled = false;
            isGrounded = false;
            //reset fórce
            currentJumpForce = minJumpForce;
            currentLaserLength = laserStartLength;
            currentDirectionModifier = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6) //check if ground was hit
        {
            isGrounded = true;
        }
    }
}
