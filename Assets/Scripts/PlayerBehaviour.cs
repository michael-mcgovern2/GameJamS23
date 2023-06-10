using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Editor player settings
    [Header("Gameplay Settings")]
    public float dashSpeed = 1f;
    public float dashCooldown = 0.5f;
    [Tooltip("Acceleration stats for dash as: (accel, accel time, decel time)")]
    public Vector3 dashDynamics = Vector3.zero;
    public float fireCooldown = 0.5f;
    
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
    Vector2 velocity = Vector2.zero; // Direction of travel due of player
    float dashAccel = 0f; // Acceleration to be added in the direction of velocity each update
    float accelTimer = 0f; // Timing counting down acceleration and deceleration stages of the dash

    ActionTimer dashTimer;
    ActionTimer fireTimer;
    
    List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        Init();
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

        ApplyAcceleration(Time.fixedDeltaTime);
    }

    void UpdateTimers()
    {
        dashTimer.Update(Time.fixedDeltaTime);
        fireTimer.Update(Time.fixedDeltaTime);
    }

    void ApplyAcceleration(float deltaTime)
    {
        if (accelTimer == 0f || rigidBody.IsTouching(contactFilter)) // Dont apply acceleration if timer is up or if the player is colliding with anything
        {
            accelTimer = 0f;
            dashAccel = 0f;
            return;
        }

        accelTimer = Mathf.Max(accelTimer - deltaTime, 0f);

        if (accelTimer > dashDynamics[2]) // Accelerating at beginning of dash
        {
            dashAccel = dashDynamics[0];
        }
        else // Deceleration to actual speed
        {
            dashAccel = - dashDynamics[0] * dashDynamics[1] / dashDynamics[2]; // FIXME: This should return to dashSpeed by the end of the timer, but it does not
        }

        Vector2 accelVector = velocity.normalized * dashAccel * deltaTime; // Apply acceleration in direction of travel
        rigidBody.AddForce(accelVector, ForceMode2D.Impulse);
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
            velocity = lookDir - rigidBody.position;
            velocity.Normalize();
            velocity *= dashSpeed;

            rigidBody.velocity = velocity;
            // Put dash on cooldown
            dashTimer.Start();
            // Start dash dynamics
            accelTimer = dashDynamics[1] + dashDynamics[2];
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);

            // TODO: kill player, restart level
        }
        else
        {
            accelTimer = 0f;
            dashAccel = 0f;
        }
    }
}
