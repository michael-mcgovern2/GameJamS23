using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private IActivatable module;
    public GameObject moduleGameObject;

    // Start is called before the first frame update
    void Start()
    {
        module = moduleGameObject.GetComponent<IActivatable>();
    }

    // Update is called once per frame (fixed intervals for fixed update)
    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // check for specific type, or is this good enough?
        module.Activate();
    }
}
