using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform cam;

    [Header("Enemies Look At")]
    [SerializeField] public List<Transform> enemies;
    [SerializeField] public int pickAnEnemy;
    [SerializeField] public float radiusOffset = 2;
    [SerializeField] LayerMask _enemyMask;

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
    public float dashCD = 2f;
    public float dashCdTimer = 2f;
    [SerializeField] float _IFrameCoolDown;
    [SerializeField] bool dashing;
    [SerializeField] GameObject dashTrail;

    [Header("Attack")]
    public bool attack;
    public Animator animator;

    //Movement Variable
    private Vector3 direction;
    private Vector3 velocity;
    Vector3 moveDirection;
    private float targetAngle;
    private float angle;

    private float horizontal, vertical;
    private float _regularSpeed;
    //public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        dashTrail.SetActive(false);
        _comboControler = GetComponent<Fight>();
        _startSpeed = speed;
        dashCdTimer = dashCD;
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
        //Liron 4/6/2023
        if (_comboControler.baseAttacking)
        {
            LookAtMouse();
        }
        // CheckGround();
        PickEnemy();

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
        if (speed <= _startSpeed)
        {
            speed = _startSpeed;
        }
    }

    //unused
    void PickEnemy()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (pickAnEnemy < enemies.Count)
            {
                pickAnEnemy++;
            }
            else
                pickAnEnemy = 0;
        }
    }

    public void LookAtEnemy(Transform enemyTransform)
    {
        Vector3 enemyDirrect = enemyTransform.position - transform.position;
        Vector3 newDirrect = Vector3.RotateTowards(transform.forward, enemyDirrect, 20, 0f);
        transform.rotation = Quaternion.LookRotation(newDirrect);
    }

    //Liron 4/6/2023
    public void LookAtMouse()
    {
        var yPlane = new Plane(inNormal: transform.up, inPoint: transform.position);
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!yPlane.Raycast(mouseRay, out var entered))
        {
            return;
        }

        Vector3 point = mouseRay.GetPoint(entered);

        if (!CheckEnemyFromRaycast(point))
        {
            Vector3 dirrect = (point - transform.position).normalized;
            float angle = Mathf.Atan2(dirrect.x, dirrect.z) * Mathf.Rad2Deg; //AngleBetweenTwoPoints(point, transform.position) * -1;

            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    public bool CheckEnemyFromRaycast(Vector3 point)
    {
        Collider[] targetsInFieldView = Physics.OverlapSphere(point, radiusOffset, _enemyMask);

        if (targetsInFieldView.Length > 0)
        {
            LookAtEnemy(targetsInFieldView[0].transform);
            return true;
        }
        return false;
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
        if (Time.time > dashCD)
        {
            StartCoroutine(ActivateDash());
            dashCD = Time.time + dashCdTimer;
        }
    }

    IEnumerator ActivateDash()
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            controller.detectCollisions = false;
            dashing = true;
            dashTrail.SetActive(true);
            controller.Move(moveDirection.normalized * (speed * dashMultiplier) * Time.deltaTime);
            //animator.SetBool("Dash",dashing);
            yield return null;
        }
        StartCoroutine(ExtraTimeEvade());
        dashing = false;
        dashTrail.SetActive(false);
    }

    IEnumerator ExtraTimeEvade()
    {
        float t = Time.time;
        while (Time.time < t + _IFrameCoolDown)
        {
            yield return null;
        }
        controller.detectCollisions = true;
    }
}


