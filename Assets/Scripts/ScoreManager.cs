using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int player1Score = 0;
    [SerializeField] private int player2Score = 0;
    [SerializeField] private TextMeshProUGUI player1Text;
    [SerializeField] private TextMeshProUGUI player2Text;
    [SerializeField] private BallController ball;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winText;

    [SerializeField] private GameManager gameManager;
    private int winScore = 5;
    
    public void Player1Scored()
    {
        player1Score++;
        HandleScore();
    }
    public void Player2Scored() 
    { 
        player2Score++;
        HandleScore();
    }
    private void HandleScore()
    {
        UpdateUI();
        ball.ResetBall();
        
        if (CheckWin(out string winner))
        {
            ShowWin(winner);
        }
        else
        {
            CountDownUI.instance.StartCountdown();
        }
    }
    public void UpdateUI()
    {
        player1Text.text = player1Score.ToString();
        player2Text.text = player2Score.ToString();
    }
    private bool CheckWin(out string winnerText)
    {
        if (player1Score >= winScore)
        {
            winnerText = "Player 1 Wins!";
            return true;
        }
        else if (player2Score >= winScore)
        {
            if (gameManager.IsAI) winnerText = "AI Wins!";
            else winnerText = "Player 2 Wins!";

            return true;
        }
        else
        {
            winnerText = null;
            return false;
        }
    }
    private void ShowWin(string message)
    {
        ball.StopBall();
        winScreen.SetActive(true);
        winText.text = message;
    }
}
