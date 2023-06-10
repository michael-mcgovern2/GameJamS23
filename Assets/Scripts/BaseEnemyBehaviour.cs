using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBehaviour : MonoBehaviour
{
    public int health; // Number of bullets it takes to kill the enemy
    public float invincibilityDuration; // Time between impacts where enemy is immune to damage

    private float invincibilityTimer = 0f;

    protected void FixedUpdate()
    {
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer = Mathf.Max(invincibilityTimer - Time.fixedDeltaTime, 0f);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Projectile" && invincibilityTimer == 0) // We dont distinguish between player and enemy projectiles
        {
            Destroy(other.gameObject); // Remove projectile which impacted

            health--;
            invincibilityTimer = invincibilityDuration; // Start invicibility period

            if (health == 0)
            {
                Destroy(gameObject); // Remove enemy
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            // TODO: kill player, restart level
        }
    }
}
