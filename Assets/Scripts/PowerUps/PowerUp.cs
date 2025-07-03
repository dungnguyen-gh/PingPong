using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float duration = 3f;
    
    public string displayName = string.Empty;

    public void Collect(GameObject collector, GameObject opponent, PowerUpSpawner spawner)
    {
        transform.position = Vector3.one * 1000f;

        AudioManager.instance.PlayCollectPowerUp();

        // trigger UI
        
        PowerUpUI.instance.Show($"{CheckHitter(collector)} got {displayName}!", duration);

        // apply effect
        StartCoroutine(EffectCoroutine(collector, opponent));

        // collect item
        spawner.PowerUpCollected(this.gameObject);

    }
    private string CheckHitter(GameObject collector)
    {
        if (collector.GetComponent<PlayerIdentifier>().playerType == PlayerIdentifier.PlayerType.Player1)
        {
            return "Player 1";
        }

        else if (collector.GetComponent<PlayerIdentifier>().playerType == PlayerIdentifier.PlayerType.Player2)
        {
            if (collector.TryGetComponent<AIPaddle>(out var aiPaddle) && aiPaddle.isActiveAndEnabled)
            {
                return "AI";
            }
            else
            {
                return "Player 2";
            }
        }
        else
        {
            return collector.name;
        }
    }
    private IEnumerator EffectCoroutine(GameObject collector, GameObject opponent)
    {
        yield return ApplyEffect(collector, opponent);
        Destroy(this.gameObject);
    }
    protected abstract IEnumerator ApplyEffect(GameObject collector, GameObject opponent);
}
