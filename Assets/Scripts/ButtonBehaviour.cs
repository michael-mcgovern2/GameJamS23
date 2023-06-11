using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private IActivatable module;
    public GameObject moduleGameObject;
    public bool isWall = false;

    // Start is called before the first frame update
    void Start()
    {
        module = moduleGameObject.GetComponent<IActivatable>();
    }

    // Update is called once per frame (fixed intervals for fixed update)
    void FixedUpdate()
    {
        GetComponent<BoxCollider2D>().isTrigger = !isWall;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Player" || tag == "Enemy" || (isWall && tag == "Projectile"))
        {
            module.Activate();
        }
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        string tag = collision.tag;

        if (tag == "Player" || tag == "Enemy")
        {
            module.Activate();
        }
    }
}
