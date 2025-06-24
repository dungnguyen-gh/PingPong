using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownUI : MonoBehaviour
{
    public static CountDownUI instance { get; private set; }
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private BallController ballController;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }
    IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }
        countdownText.text = "GO";
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);
        ballController.LaunchBall();
    }
}
