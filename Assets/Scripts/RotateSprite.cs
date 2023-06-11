using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    public float initialAngle = 0f;

    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();

        if (rigidBody == null)
        {
            Debug.LogError("Sprite rotator failed to find rigidbody");
            Destroy(this);
            return;
        }

        Rotate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        // From https://answers.unity.com/questions/757118/rotate-towards-velocity-2d.html
        Vector2 v = rigidBody.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - initialAngle, Vector3.forward);
    }
}
