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
    public float fireCooldown = 0.5f;

    [Header("Physics Settings")]
    public float raycastDist = 1f;
    public ContactFilter2D contactFilter;
    public Rigidbody2D rigidBody; // Represents the players location for physics and collisions

    // Private player fields
    private Camera camera;
    Vector2 lookDir = Vector2.zero; // position of mouse in room
    Vector2 velocity = Vector2.zero;

    bool dashInput = false; // Player has requested a dash this update
    ActionTimer dashTimer;

    bool fireInput = false; // Player has requested a fire this update
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
        camera = Camera.main;
    }

    // Update is called once per frame (fixed intervals for fixed update)
    void FixedUpdate()
    {
        UpdateTimers();

        if (dashTimer.actionAllowed && dashInput)
        {
            // Perform dash
            velocity = lookDir - rigidBody.position;
            velocity.Normalize();
            // Put dash on cooldown
            dashTimer.Start();
        }

        if (fireTimer.actionAllowed && fireInput)
        {
            // Perform fire

            fireTimer.Start();
        }

        // Currently acceleration is not considered
        if (velocity != Vector2.zero)
        {
            int collisionCount = rigidBody.Cast(
                velocity,
                contactFilter,
                raycastHits,
                dashSpeed * Time.fixedDeltaTime + raycastDist
            );

            if (collisionCount == 0) // Only move if not going to collide
            {
                rigidBody.MovePosition(rigidBody.position + dashSpeed * Time.fixedDeltaTime * velocity);
            }
            else
            {
                velocity = Vector2.zero; // TODO: move to impact point instead of stopping early
            }
        }

        // Reset input variables
        dashInput = false;
        fireInput = false;
    }

    void UpdateTimers()
    {
        dashTimer.Update(Time.fixedDeltaTime);
        fireTimer.Update(Time.fixedDeltaTime);
    }

    void OnFire(InputValue input) // When the player clicks to fire
    {
        fireInput = true;
    }

    void OnLook(InputValue input) // Follow the mouse to aim the player's dash and firing
    {
        Vector2 screenPos = input.Get<Vector2>();
        lookDir = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
    }

    void OnDash(InputValue input) // When the player clicks to dash
    {
        dashInput = true;
    }
}
