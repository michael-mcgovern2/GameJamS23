using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    public GameObject player { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        player = gameObject;
    }

    public Vector2 GetPlayerPosition()
    {
        if (player == null)
        {
            return Vector2.zero;
        }

        return player.transform.position;
    }

    public void KillPlayer()
    {
        PlayerBehaviour pb = player.GetComponent<PlayerBehaviour>();
        pb.KillPlayer();
    }
}
