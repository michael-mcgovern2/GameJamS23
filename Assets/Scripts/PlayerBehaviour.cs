using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerBehaviour : MonoBehaviour
{
    // Editor player settings
    [Header("Gameplay Settings")]
    public float dashSpeed = 1f;
    public float dashCooldown = 0.5f; // Cooldown after arriving at a the destination before player can dash again
    [Tooltip("Acceleration stats for dash as: (accel, accel time, decel time)")]
    public Vector3 dashDynamics = Vector3.zero;
    public float fireCooldown = 0.5f;
    public Transform respawnPoint;
    public float initialAngle = 90f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float spawnOffset;
    public float launchSpeed;

    [Header("Physics Settings")]
    public float raycastDist = 1f;
    public ContactFilter2D contactFilter;
    public Rigidbody2D rigidBody; // Represents the players location for physics and collisions



    // Private player fields
    private Camera cam;
    Vector2 lookDir = Vector2.zero; // Position of mouse in room
    Vector2 dashDest = Vector2.zero; // Position at which dash should end
    Vector2 velocity = Vector2.zero; // Direction of travel due of player
    public bool isDashing = false;
    public bool isBouncing = false;

    public ActionTimer dashTimer;
    public ActionTimer fireTimer;
    
    List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        anim = gameObject.GetComponent<Animator>();
    }

    void Init()
    {
        dashTimer = new ActionTimer(dashCooldown);
        fireTimer = new ActionTimer(fireCooldown);
        cam = Camera.main;
    }

    // Update is called once per frame (fixed intervals for fixed update)
    void FixedUpdate()
    {
        UpdateTimers();

        if (fireTimer.timeRemaining == 0f)
        {
            anim.SetBool("isFiring", false);
        }

        if (rigidBody.velocity.magnitude > 0 && (rigidBody.position - dashDest).magnitude < 0.1f) // If dash has been stopped or arrived at destination
        {
            rigidBody.velocity = Vector2.zero;
            dashTimer.Start(); // pause here until dash cooldown is over
            isDashing = false;
        }

        // Rotate the sprite towards the mouse
        Vector2 v = lookDir - rigidBody.position;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - initialAngle, Vector3.forward);
    }

    void UpdateTimers()
    {
        dashTimer.Update(Time.fixedDeltaTime);
        fireTimer.Update(Time.fixedDeltaTime);
    }

    void OnFire(InputValue input) // When the player clicks to fire
    {
        if (fireTimer.actionAllowed)
        {
            // Perform fire
            if (projectilePrefab != null)
            {
                GameObject g = Instantiate(projectilePrefab, rigidBody.position, Quaternion.identity);
                
                Rigidbody2D projectileRB = g.GetComponent<Rigidbody2D>();
                if (projectileRB != null)
                {
                    Vector2 fireDirection = lookDir - rigidBody.position;
                    fireDirection.Normalize();
                    projectileRB.position = rigidBody.position + fireDirection * spawnOffset;
                    projectileRB.velocity = fireDirection * launchSpeed;
                }
                else
                {
                    Debug.LogWarning("Failed to initialize projectile rigidbody");
                }

                anim.SetBool("isFiring", true);
            }
            fireTimer.Start();
        }
    }

    void OnLook(InputValue input) // Follow the mouse to aim the player's dash and firing
    {
        Vector2 screenPos = input.Get<Vector2>();
        lookDir = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
    }

    void OnDash(InputValue input) // When the player clicks to dash
    {
        if (dashTimer.actionAllowed)
        {
            // Perform dash
            dashDest = lookDir;
            velocity = lookDir - rigidBody.position;
            velocity.Normalize();
            velocity *= dashSpeed;

            rigidBody.velocity = velocity;
            // Prevent dashing
            dashTimer.actionAllowed = false;
            isDashing = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);

            KillPlayer();
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (dashTimer.timeRemaining == 0 && !dashTimer.actionAllowed)
        {
            dashTimer.Start(); // Allow player to redash if they hit something
            isDashing = false;
        }
    }

    public void KillPlayer()
    {
        if (isDashing)
        {
            return;
        }
        
        rigidBody.position = respawnPoint.position;
        rigidBody.velocity = Vector2.zero;
        dashTimer.actionAllowed = true;
    }
}
