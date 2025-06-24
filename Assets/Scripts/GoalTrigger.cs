using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private bool isLeftGoal; // True = Player2 scores, False = Player1 scores
    [SerializeField] private ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BallController ball = other.GetComponent<BallController>();

            if (ball != null) 
            {
                ball.OnScored();
            }

            if (isLeftGoal)
                scoreManager.Player2Scored();
            else 
                scoreManager.Player1Scored();
        }
    }
}
