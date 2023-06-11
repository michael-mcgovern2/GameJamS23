using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInBehaviour : MonoBehaviour
{
    public Transform teleportOut;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame (fixed intervals for fixed update)
    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        // check for specific type, or is this good enough?
        collision.transform.SetPositionAndRotation(teleportOut.position, teleportOut.rotation);
    }
}
