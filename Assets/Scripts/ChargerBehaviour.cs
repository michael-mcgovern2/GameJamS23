using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerBehaviour : BaseEnemyBehaviour
{
    public float chargeDelay; // Time between spotting the player and charging towards them
    public float chargeSpeed; // Speed at which enemy charges towards the player
    public float chargeCooldown; // Time between consecutive charge attacks
    public float bounceForce; // Force applied when charger crashes into a wall

    private float cooldownTimer = 0f; // Counts time between charges
    private float delayTimer = 0f; // Counts time until charge to the destination
    private Vector2 chargeDest; // Position where player was detected and where charger will dash to

    private Rigidbody2D rigidBody;
    private List<RaycastHit2D> castResults = new List<RaycastHit2D>();

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Charger enemy was initialized without rigidbody component");
        }
    }
    // Update is called once per frame
    new protected void FixedUpdate()
    {
        base.FixedUpdate();

        if (cooldownTimer == 0) // Look for player
        {
            // raycast to player to decide whether or not to charge
            Vector2 playerPos = PlayerManager.Instance.GetPlayerPosition();
            Vector2 chargerPos = transform.position;

            int numHits = rigidBody.Cast(
                playerPos - chargerPos,
                castResults
            );

            if (numHits > 0)
            {
                castResults.Sort((hit1, hit2) => hit1.distance.CompareTo(hit2.distance));
                RaycastHit2D firstHit = castResults[0];

                if (firstHit.collider.gameObject.tag == "Player")
                {
                    // Queue a dash towards this location after chargeDelay seconds
                    chargeDest = playerPos;
                    delayTimer = chargeDelay; // delay the launch
                    cooldownTimer = chargeCooldown; // disable charge
                }
            }
        }
        else if (delayTimer == 0) // Start dash towards player
        {
            Vector2 dir = chargeDest - (Vector2)transform.position;
            dir.Normalize();
            rigidBody.velocity = dir * chargeSpeed;
            delayTimer = cooldownTimer; // Temporarily disable dash (will be re-enabled once next charge is queued)
        }
        else // Wait for either timer
        {
            cooldownTimer = Mathf.Max(cooldownTimer - Time.fixedDeltaTime, 0f);
            delayTimer = Mathf.Max(delayTimer - Time.fixedDeltaTime, 0f);
        }
    }

    new protected void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.gameObject.tag == "Wall")
        {
            Vector2 vel = rigidBody.velocity;

            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(-bounceForce * vel.normalized, ForceMode2D.Force); // Just opposite direction of travel for now
        }
    }
}
