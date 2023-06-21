using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BounceBehaviour : MonoBehaviour
{
    PlayerBehaviour playerScript;
    public float launchForce = 10f;
    public Transform launchDirection;
    public Rigidbody2D rigidBody;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        playerScript = GameObject.FindObjectOfType<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void launchPlayer()
    {
        Debug.Log("Player Launched!");
        Vector2 dub = (Vector2)(launchDirection.position);
        velocity = dub - rigidBody.position;
        velocity.Normalize();
        velocity *= launchForce;
        rigidBody.velocity = velocity;
        playerScript.isBouncing = true;
    }



    void OnTriggerEnter2D(UnityEngine.Collider2D trigger)
    {
        if (trigger.GetComponent<PlayerBehaviour>() & (playerScript.isBouncing == false))
        {
            Debug.Log("Trigger");
            launchPlayer();
        }
    }



    //void MoveGameObject()
    //{
    //    if (transform.position == NextPos.position)
    //    {
    //        NextPosIndex++;
    //        if (NextPosIndex >= Positions.Length)
    //        {
    //            NextPosIndex = 0;
    //        }
    //        NextPos = Positions[NextPosIndex];
    //    }
    //    else
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, NextPos.position, ObjectSpeed * Time.deltaTime);
    //        float radius = 1;

    //        if (Physics.CheckSphere(Positions[Positions.Length].position, radius))
    //        {
    //            return;
    //        }
    //    }
    //}
}

//    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
//    {
//        if (tag == "Player")
//        {
//            float step = speed * Time.deltaTime;
//            // move sprite towards the target location
//            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
 //       }
//    }
//}
