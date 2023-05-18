using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;

    [Header("Ground Check & Gravity")]
    public LayerMask groundMask;
    public CharacterController controller;
    //public GameObject comboScript;
    public Transform groundCheck;
    public float groundDist = 0.1f;
    public float gravity = -9.8f;
    private Fight _comboControler;
    public bool isGrounded;

    [Header("Speed Settings")]
    public float speed = 6;
    public float turnSmooth = 0.1f;
    public float smoothSpeed;
    public float _startSpeed;

    [Header("Dash Settings")]
    public float dashMultiplier;
    public float dashTime;
    [SerializeField] bool dashing;
    [SerializeField] GameObject dashTrail;

    [Header("Attack")]
    public bool attack;

    //Movement Variable
    private Vector3 direction;
    private Vector3 velocity;
    Vector3 moveDirection;
    private float targetAngle;
    private float angle;

    private float horizontal, vertical;
    private float _regularSpeed;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        dashTrail.SetActive(false);
        _comboControler = GetComponent<Fight>();
        _startSpeed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        if (_comboControler.canMove == true)
        {
            InputPlayer();
            Move();
            animator.SetBool("Dash", dashing);
        }
    }
    public void InputPlayer()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetFloat("Speed", speed);
        }
        else
            animator.SetFloat("Speed", 0);
    }
    public void SetSpeed(float addSpeed)
    {
        speed += addSpeed;
        if(speed<=_startSpeed)
        {
            speed = _startSpeed;
        }
    }
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    public void Move()
    {
        direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            //Move From Cam View
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothSpeed, turnSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        //animator.SetFloat("Speed", speed);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void Dash()
    {
        StartCoroutine(ActivateDash());
    }
    IEnumerator ActivateDash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            dashing = true;
            dashTrail.SetActive(true);
            controller.Move(moveDirection.normalized * (speed* dashMultiplier) * Time.deltaTime);
            //animator.SetTrigger("Dashing");
            yield return null;
        }
        dashing = false;
        dashTrail.SetActive(false);
    }
}
