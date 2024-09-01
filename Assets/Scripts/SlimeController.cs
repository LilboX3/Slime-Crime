using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    [SerializeField]
    private float minJumpForce = 2f;
    [SerializeField]
    private float maxJumpForce = 6f;
    [SerializeField]
    private float chargeSpeed = 4f;
    [SerializeField]
    private int health = 3;

    [SerializeField]
    private LineRenderer directionRenderer;
    [SerializeField]
    private float laserWidth = 0.1f;
    [SerializeField]
    private float laserStartLength = 0.5f;
    private float currentLaserLength;
    [SerializeField]
    private float directionModifierSpeed = 1.5f;
    [SerializeField]
    private float maxDirectionModifier = 3f;
    public int score = 0;

    private float currentJumpForce;
    private float currentDirectionModifier;
    private Rigidbody rb;
    private Animator animator;
    private Transform floorIndicator;
    private Transform slimeObject;
    private Vector3 jumpDirection;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        floorIndicator = gameObject.transform.GetChild(2);
        slimeObject = gameObject.transform.GetChild(0);
        currentJumpForce = minJumpForce;
        directionRenderer.enabled = false;
        directionRenderer.startWidth = laserWidth;
        currentDirectionModifier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: make charge look faster ...?
        if (Input.GetButton("Jump") && isGrounded)
        {
            currentJumpForce += chargeSpeed * Time.deltaTime;
            currentLaserLength += directionModifierSpeed * Time.deltaTime;
            currentDirectionModifier += directionModifierSpeed * Time.deltaTime;

            currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce); //Wert begrenzen zwischen min und maxForce
            currentLaserLength = Mathf.Clamp(currentLaserLength, laserStartLength, maxDirectionModifier);
            currentDirectionModifier = Mathf.Clamp(currentDirectionModifier, 1, maxDirectionModifier);

            float horizontalInput = Input.GetAxis("Joystick horizontal");
            float verticalInput = Input.GetAxis("Joystick vertical");
            jumpDirection = new Vector3(-horizontalInput, 0, verticalInput).normalized;

            Vector3 lineDirection = new Vector3(horizontalInput, 0, -verticalInput).normalized;
            directionRenderer.SetPosition(1, lineDirection * (currentLaserLength/3)); //tweak to match distance jumped?
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
        DrawFloorIndicator();
    }

    void DrawFloorIndicator()
    {
        Vector3 origin = slimeObject.position;
        Vector3 direction = Vector3.down;
        RaycastHit hit;
        float maxDistance = 100f;

        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            Vector3 hitPosition = hit.point;
            floorIndicator.position = hitPosition;
        }
    }

    public void AddPoints(int amount)
    {
        score += amount;
        if(health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Die()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6) //check if ground was hit
        {
            isGrounded = true;
        }
    }
    
}
