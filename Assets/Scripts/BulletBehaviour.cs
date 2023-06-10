using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public int bulletHealth;
    public float damageDelay; // Invincibility after contacting wall before projectile takes more damage
    private float damageTimer;
    public Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (damageTimer > 0)
        {
            damageTimer = Mathf.Max(damageTimer - Time.fixedDeltaTime, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            WallBehaviour wallScript = other.gameObject.GetComponent<WallBehaviour>();

            if (wallScript == null)
            {
                Debug.LogWarning("Failed to get WallBehaviour script on bullet collision with wall - make sure all walls have this script or subclass script");
                return;
            }

            if (damageTimer == 0)
            {
                bulletHealth -= wallScript.damageToBullet;
                if (bulletHealth <= 0)
                {
                    Destroy(gameObject);
                }

                damageTimer = damageDelay;
            }
        }
        
    }
}
