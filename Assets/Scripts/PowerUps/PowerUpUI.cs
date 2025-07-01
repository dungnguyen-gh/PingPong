using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpUI : MonoBehaviour
{
    public static PowerUpUI instance {  get; private set; }
    public TextMeshProUGUI messageText;
    private void Awake()
    {
        instance = this;
        messageText.gameObject.SetActive(false);
    }
    public void Show(string msg, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(msg, duration));
    }
    private IEnumerator ShowRoutine(string msg, float duration)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        messageText.gameObject.SetActive(false);
    }
}
