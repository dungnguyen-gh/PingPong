using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float duration = 3f;
    public void Collect(GameObject collector, GameObject opponent, PowerUpSpawner spawner)
    {
        transform.position = Vector3.one * 1000f;

        AudioManager.instance.PlayCollectPowerUp();

        // trigger UI
        PowerUpUI.instance.Show($"{collector.name} got {GetType().Name.Replace("PowerUp", "")}!", duration);

        // apply effect
        StartCoroutine(EffectCoroutine(collector, opponent));

        // collect item
        spawner.PowerUpCollected(this.gameObject);

    }
    private IEnumerator EffectCoroutine(GameObject collector, GameObject opponent)
    {
        yield return ApplyEffect(collector, opponent);
        Destroy(this.gameObject);
    }
    protected abstract IEnumerator ApplyEffect(GameObject collector, GameObject opponent);
}
