using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelExit : MonoBehaviour
{
    GameManager gm;

    public int nextLevel;

    public bool dooropen;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            doorState();
            if (getDoorState())
            {
                gm.LoadNextLevel(nextLevel);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        bool check = (gm != null);
    }

    // Update is called once per frame
    public void doorState()
    {
        dooropen = true;
    }

    public bool getDoorState()
    {
        return dooropen;
    }
}
