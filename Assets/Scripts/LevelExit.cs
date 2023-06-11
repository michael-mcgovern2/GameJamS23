using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelExit : MonoBehaviour
{
    public int nextLevel;
    public float bonusTime;
    public bool dooropen;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            doorState();
            if (getDoorState())
            {
                PlayerManager.Instance.AddTime(bonusTime);
                PlayerManager.Instance.LoadNextLevel(nextLevel);
            }
        }
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
