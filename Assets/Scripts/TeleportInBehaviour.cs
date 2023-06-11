using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInBehaviour : MonoBehaviour
{
    public Transform teleportOut;
    public bool isWall = false;

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
        string tag = collision.tag;

        if (tag == "Player" || tag == "Enemy" || (isWall && tag == "Projectile"))
        {
            collision.transform.SetPositionAndRotation(teleportOut.position, teleportOut.rotation);
        }
    }
}
