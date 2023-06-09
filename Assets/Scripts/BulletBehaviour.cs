using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float numBounces;
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
        if (damageTimer == 0)
        {
            numBounces--;
            if (numBounces == 0)
            {
                Destroy(gameObject);
            }

            damageTimer = damageDelay;
        }
    }
}
