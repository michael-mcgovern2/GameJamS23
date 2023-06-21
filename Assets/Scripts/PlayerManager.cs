using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public static float startTime = 100f;

    public GameObject player { get; private set; }
    public static float gameTimer { get; private set; } = startTime; // Time until player loses this run, stored in seconds
    public float restartTime = 5f; // Number of seconds on gameover screen before game restarts

    private UIManager currentUI;
    private float restartTimer;

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

    private void FixedUpdate()
    {
        if (restartTimer > 0)
        {
            restartTimer -= Time.fixedDeltaTime;

            if (restartTimer <= 0)
            {
                gameTimer = startTime;
                LoadNextLevel(1);
            }

            return;
        }

        gameTimer -= Time.fixedDeltaTime;

        if (gameTimer <= 0)
        {
            var uiManagers = FindObjectsOfType<UIManager>();
            if (uiManagers.Length > 0)
            {
                currentUI = uiManagers[0];
            }
            else
            {
                Debug.LogWarning("Failed to find UI Manager for level for game over");
                return;
            }

            currentUI.timer.SetActive(false);
            currentUI.gameOver.SetActive(true);
            restartTimer = restartTime;
        }
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

    public void LoadNextLevel(int x)
    {
        SceneManager.LoadScene(x);
    }

    public void AddTime(float bonusTime)
    {
        gameTimer += bonusTime;
    }
}
