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
    private float chargeSpeed = 1f;

    [SerializeField]
    private LineRenderer directionRenderer;
    [SerializeField]
    private float laserWidth = 0.1f;
    [SerializeField]
    private float laserStartLength = 5f;
    private float currentLaserLength;

    private float currentJumpForce;
    private Rigidbody rigidbody;
    private Animator animator;
    private Vector3 jumpDirection;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentJumpForce = minJumpForce;
        directionRenderer.enabled = false;
        directionRenderer.startWidth = laserWidth;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("current jump force: " + currentJumpForce);
        if (Input.GetButton("Jump") && isGrounded)
        {
            currentJumpForce += chargeSpeed * Time.deltaTime;
            currentLaserLength += chargeSpeed * Time.deltaTime;
            currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce); //Wert begrenzen zwischen min und maxForce
            currentLaserLength = Mathf.Clamp(currentLaserLength, laserStartLength, maxJumpForce); //tweak to represent distance to jumo

            float horizontalInput = Input.GetAxis("Joystick horizontal");
            float verticalInput = Input.GetAxis("Joystick vertical");
            jumpDirection = new Vector3(horizontalInput, 0, -verticalInput).normalized;

            directionRenderer.SetPosition(1, jumpDirection * currentLaserLength);
            directionRenderer.enabled = true;
        }
        if (Input.GetButtonUp("Jump") && isGrounded)
        {
            rigidbody.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            rigidbody.AddForce(jumpDirection, ForceMode.Impulse); 

            directionRenderer.enabled = false;
            isGrounded = false;
            //reset fórce
            currentJumpForce = minJumpForce;
            currentLaserLength = laserStartLength;
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
