using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;

    public string name;

    public float runSpeed;

    public float crosshairOffset;

    public float maxHealth;

    public float health;

    public float nextShot;

    public float shootSpeed;

    public PowerUp powerUp;

    public float jumpStrength;

    public LevelController levelController;

    public Texture2D cursor;

    public ProjectileLauncher launcher;

    public PowerUpType startingPowerUp = PowerUpType.NONE;

    public int startingPowerUpStacks = 1;

    public CameraController cameraController;

    public NavMeshAgent navAgent;

    public bool isHuman;

    public bool isBoss;

    public int numberOfLives;

    public GameObject targetPlayer;

    public Renderer modelRenderer;

    public Vector3 distanceFromPlayer;

    public bool playerInRange;

    public float maxShotDistanceAI;

    public float aimOffsetHeight;

    public float rotationSpeedAI;

    public float hitColorTime;

    public float timeToDestroy;

    private float destroyTime;

    private float hitColorNextTime;

    public bool isAirborne;

    public float aggroDistanceAI;

    private bool isRunning;

    private float currentMoveSpeed;

    private LineRenderer lineRenderer;

    private Rigidbody rb;

    private Animator animator;

    private Vector3 vector3;

    private Vector3 crosshairPosition;

    private Vector3 horizontalAxis;

    private Vector3 verticalAxis;

    private Vector3 aimDirection;

    void Start()
    {
        if (isHuman)
        {
            Time.timeScale = 1;
            Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.Auto);
            if (startingPowerUp.Equals(PowerUpType.SPLIT))
            {
                powerUp = new PowerUp(startingPowerUp);
                launcher.projectileType = ProjectileType.getSpread(0.1f, startingPowerUpStacks);
            }
            else if (startingPowerUp.Equals(PowerUpType.TRACKING))
            {
                powerUp = new PowerUp(startingPowerUp);
                launcher.projectileType = ProjectileType.getSpread(0.1f, startingPowerUpStacks);
                launcher.isSeeking = true;
            }
            else
            {
                launcher.projectileType = ProjectileType.getRegular(0.1f);
            }
            horizontalAxis = Vector3.Normalize(Vector3.right + (Vector3.forward * -1));
            verticalAxis = Vector3.Normalize(Vector3.right + Vector3.forward);
            isRunning = false;
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            currentMoveSpeed = walkSpeed;
            aimDirection = launcher.transform.forward;
            isAirborne = false;
        }
        else
        {
            if (startingPowerUp.Equals(PowerUpType.SPLIT))
            {
                launcher.projectileType = ProjectileType.getSpread(0.1f, startingPowerUpStacks);
            }
            else if (startingPowerUp.Equals(PowerUpType.TRACKING))
            {
                launcher.projectileType = ProjectileType.getSpread(0.1f, startingPowerUpStacks);
                launcher.isSeeking = true;
            }
            else
            {
                launcher.projectileType = ProjectileType.getRegular(0.1f);
            }
            navAgent = GetComponent<NavMeshAgent>();
            isRunning = false;
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            isAirborne = false;
            playerInRange = false;
        }
    }

    public void PowerUp(PowerUpType powerUpType)
    {
        if (powerUpType.Equals(PowerUpType.HEALTH))
        {
            float newHealth = health + 20;
            if (newHealth > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health = newHealth;
            }
        }
        else if (powerUp == null || !powerUp.getType().Equals(powerUpType))
        {
            powerUp = new PowerUp(powerUpType);
            if (powerUpType.Equals(PowerUpType.SPLIT))
            {
                launcher.isSeeking = false;
                launcher.projectileType = ProjectileType.getSpread(0.1f, 1);
            }
            if (powerUpType.Equals(PowerUpType.TRACKING))
            {
                launcher.isSeeking = true;
                launcher.projectileType = ProjectileType.getSpread(0.1f, 0);
            }
        }
        else
        {
            if (powerUp.getStackCount() < 3)
            {
                if (powerUpType.Equals(PowerUpType.SPLIT))
                {
                    launcher.projectileType = ProjectileType.getSpread(0.1f, ++powerUp.stackCount);
                }
                else if (powerUpType.Equals(PowerUpType.TRACKING))
                {
                    launcher.projectileType = ProjectileType.getSpread(0.1f, ++powerUp.stackCount - 1);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && !isAirborne)
        {
            isAirborne = true;
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }

    void Update()
    {
        if (health > 0)
        {
            if (isHuman)
            {
                CheckAndHandleSprint();
                HandleShooting();
                ReadPlayerMovementInput();
                LookAtCursorPosition();
            }
            else
            {
                AIFindPlayer();
                AIShootAtPlayer();
            }
        }
    }

    void LateUpdate()
    {
        if (health <= 0)
        {
            modelRenderer.material.color = Color.gray;
            if (Time.time > destroyTime)
            {
                if (!isHuman)
                {
                    if (isBoss)
                    {
                        levelController.HandleLevelComplete();
                    }
                    DestroyImmediate(gameObject.transform.parent.gameObject);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
        else
        {
            if (Time.time > hitColorNextTime)
            {
                modelRenderer.material.color = Color.white;
            }
            else
            {
                modelRenderer.material.color = Color.red;
            }
        }
    }

    public void Hit(float damage)
    {
        if (health > damage)
        {
            health -= damage;
            hitColorNextTime = Time.time + hitColorTime;
        }
        else
        {
            HandleDeath();
        }
        //if (!isHuman)
        //{
        //    StaggerAI();
        //}
    }

    void HandleDeath()
    {
        destroyTime = Time.time + timeToDestroy;
        if (isHuman)
        {
            cameraController.isFrozen = true;
            lineRenderer.positionCount = 0;
        }
        health = 0;
        animator.speed = 0;
        rb.drag = 20;
        GetComponent<CapsuleCollider>().enabled = false;
        navAgent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = true;
    }

    void HandleGameOver()
    {

    }

    void StaggerAI()
    {
        if (!isBoss)
        {
            nextShot = Time.time + shootSpeed;
        }
    }

    void AIFindPlayer()
    {
        if (navAgent.velocity.magnitude > 0)
        {
            if (isRunning || navAgent.velocity.y > 0)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
            }
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        distanceFromPlayer = targetPlayer.transform.position - launcher.getWorldSpawnLocation();
        if (distanceFromPlayer.magnitude <= aggroDistanceAI)
        {
            navAgent.SetDestination(targetPlayer.transform.position);
            playerInRange = distanceFromPlayer.magnitude <= maxShotDistanceAI;
        }
    }

    void AIShootAtPlayer()
    {
        if (Time.timeScale > 0 && playerInRange)
        {
            aimDirection = Vector3.Normalize(targetPlayer.transform.position - launcher.getWorldSpawnLocation());
            Quaternion targetRotation = Quaternion.LookRotation(targetPlayer.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeedAI * Time.deltaTime);
            RaycastHit raycastHit;
            if (Physics.Raycast(launcher.getWorldSpawnLocation(), distanceFromPlayer + new Vector3(0, aimOffsetHeight, 0), out raycastHit) && raycastHit.transform.tag == "Player" && Time.time > nextShot)
            {
                nextShot = Time.time + shootSpeed;
                launcher.Launch(aimDirection + new Vector3(0, aimOffsetHeight, 0));
            }
        }
    }

    void HandleShooting()
    {
        if (Time.timeScale > 0 && Input.GetMouseButton(0) && Time.time > nextShot)
        {
            nextShot = Time.time + shootSpeed;
            launcher.Launch(aimDirection + new Vector3(0, aimOffsetHeight, 0));
        }
    }

    void LookAtCursorPosition()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(cameraRay, out raycastHit) && Time.timeScale > 0)
        {
            transform.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, launcher.getWorldSpawnLocation());
            lineRenderer.SetPosition(1, raycastHit.point);
            aimDirection = Vector3.Normalize(raycastHit.point - launcher.getWorldSpawnLocation());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isAirborne = false;
        }
    }

    void ReadPlayerMovementInput()
    {
        Vector3 horizontalMovement = Input.GetAxis("Horizontal")  * horizontalAxis;
        Vector3 verticalMovement = Input.GetAxis("Vertical") * verticalAxis;
        Vector3 totalMovement = Vector3.Normalize(horizontalMovement + verticalMovement) * currentMoveSpeed;
        if (totalMovement.Equals(Vector3.zero))
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
        }
        else
        {
            if (isRunning)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
            }
        }
        totalMovement.y = rb.velocity.y;
        rb.velocity = totalMovement;
        if (Input.GetKey(KeyCode.Escape))
        {
            levelController.OpenPauseMenu();
        }
    }

    void CheckAndHandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            isRunning = true;
            currentMoveSpeed = runSpeed;
        }
        else
        {
            isRunning = false;
            currentMoveSpeed = walkSpeed;
        }
    }
}
