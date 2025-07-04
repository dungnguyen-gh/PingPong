using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player2;
    [SerializeField] private BallController ballController;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button btnAI, btnFriend;

    [SerializeField] private PowerUpSpawner powerUpSpawner;

    private bool isAI = false;
    public bool IsAI => isAI;

    public bool isPlaying = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void Start()
    {
        mainMenu.SetActive(true);
        btnAI.onClick.AddListener(() => StartGame(true));
        btnFriend.onClick.AddListener(() => StartGame(false));
    }
    void StartGame(bool playWithAI)
    {
        AudioManager.instance.PlayClick();

        isPlaying = true;

        isAI = playWithAI;

        mainMenu.SetActive(false);
        
        if (playWithAI)
        {
            Destroy(player2.GetComponent<PlayerController>());
        }
        else
        {
            Destroy(player2.GetComponent<AIPaddle>());
        }

        ballController.ResetBall();
        //ballController.LaunchBall();

        CountDownUI.instance.StartCountdown();
        
        powerUpSpawner.StartSpawning();
    }
}

public interface IPowerUpResettable
{
    void ResetPowerUpEffect();
}
public interface IPaddleController : IPowerUpResettable
{
    float Speed { get; set; }
    float MinZ { get; set; }
    float MaxZ { get; set; }
}
