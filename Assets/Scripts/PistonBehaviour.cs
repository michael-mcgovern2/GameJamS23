using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Piston : MonoBehaviour, IActivatable
{
    private bool expanded = false;
    private bool moving = false;

    public float scaleWhenExpanded = 2.0f;
    public float expansionSpeed = 1.0f;

    public void Activate()
    {
        moving = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame (fixed intervals for fixed update)
    void FixedUpdate()
    {
        if (moving)
        {
            if (expanded)
            {
                transform.localScale -= new Vector3(expansionSpeed * Time.fixedDeltaTime, 0, 0);

                if (transform.localScale.x <= 1.0f)
                {
                    transform.localScale = Vector3.one;
                    moving = false;
                    expanded = false;
                }
            }
            else
            {
                transform.localScale += new Vector3(expansionSpeed * Time.fixedDeltaTime, 0, 0);

                if (transform.localScale.x >= scaleWhenExpanded)
                {
                    transform.localScale = new Vector3(scaleWhenExpanded, 1.0f, 1.0f);
                    moving = false;
                    expanded = true;
                }
            }
        }
    }
}
