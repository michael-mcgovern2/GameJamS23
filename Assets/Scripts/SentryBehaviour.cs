using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryBehaviour : BaseEnemyBehaviour
{
    public float fireRate;
    public float launchOffset;
    public float projectileSpeed;

    public GameObject projectilePrefab;

    private float fireTimer = 0f;

    // Update is called once per frame
    new protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (fireTimer == 0)
        {
            // Launch a bullet towards the player
            Vector2 playerPos = PlayerManager.Instance.GetPlayerPosition();
            Vector2 direction = playerPos - (Vector2)transform.position;
            direction.Normalize();

            Vector2 spawnPos = (Vector2) transform.position + direction * launchOffset;

            GameObject p = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            Rigidbody2D projectileRB = p.GetComponent<Rigidbody2D>();
            if (projectileRB != null)
            {
                projectileRB.velocity = direction * projectileSpeed;
            }

            fireTimer = fireRate; // Start timer for next bullet
        }
        else
        {
            fireTimer = Mathf.Max(fireTimer - Time.fixedDeltaTime, 0f);
        }
    }
}
