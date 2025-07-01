using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigPaddlePowerUp : PowerUp
{
    protected override IEnumerator ApplyEffect(GameObject collector, GameObject opponent)
    {
        var paddle = collector.GetComponent<IPaddleController>();
        if (paddle != null)
        {
            MonoBehaviour paddleScript = (MonoBehaviour)paddle;
            Vector3 originalScale = paddleScript.transform.localScale;

            paddleScript.transform.localScale = new Vector3(originalScale.x, originalScale.y, 2.4f);

            // Adjust movement bounds while big
            paddle.MinZ = -0.8f;
            paddle.MaxZ = 0.8f;

            // Keep original clamp and let the controller handle clamping based on current scale
            yield return new WaitForSeconds(duration);

            paddle.ResetPowerUpEffect();
        }
    }
}
