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

    private bool isAI = false;
    public bool IsAI => isAI;

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

        isAI = playWithAI;

        mainMenu.SetActive(false);
        player2.GetComponent<AIPaddle>().enabled = playWithAI;
        player2.GetComponent<PlayerController>().enabled = !playWithAI;
        ballController.ResetBall();
        //ballController.LaunchBall();
        CountDownUI.instance.StartCountdown();
    }
}
