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

    protected bool CanSeePlayer(Rigidbody2D castBody)
    {
        List<RaycastHit2D> castResults = new List<RaycastHit2D>();
        // raycast to player to decide whether or not to charge
        Vector2 playerPos = PlayerManager.Instance.GetPlayerPosition();
        Vector2 enemyPos = transform.position;

        int numHits = castBody.Cast(
            playerPos - enemyPos,
            castResults
        );

        if (numHits > 0)
        {
            castResults.Sort((hit1, hit2) => hit1.distance.CompareTo(hit2.distance));
            RaycastHit2D firstHit = castResults[0];

            if (firstHit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }
}
