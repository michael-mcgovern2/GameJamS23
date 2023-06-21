using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPointBehaviour : MonoBehaviour
{
    PlayerBehaviour playerScript;
    public Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerBehaviour>();
    }
    void OnTriggerEnter2D(UnityEngine.Collider2D trigger)
    {
        if (trigger.GetComponent<PlayerBehaviour>() & (playerScript.isBouncing == true))
        {
            playerScript.isBouncing = false;
            rigidBody.velocity = Vector2.zero;
        }
    }
}
