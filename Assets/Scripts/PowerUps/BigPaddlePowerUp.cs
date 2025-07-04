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

            // Apply effect based on controller type
            if (paddle is AIPaddle ai)
            {
                ai.ApplyBigPaddleEffect();
                print("ai hit");
            }
            else if (paddle is PlayerController pc)
            {
                pc.ApplyBigPaddleEffect();
                print("pc hit");
            }

            yield return new WaitForSeconds(duration);

            paddle.ResetPowerUpEffect();
        }
    }
}
