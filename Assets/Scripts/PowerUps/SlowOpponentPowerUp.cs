using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOpponentPowerUp : PowerUp
{
    protected override IEnumerator ApplyEffect(GameObject collector, GameObject opponent)
    {
        var paddle = opponent.GetComponent<IPaddleController>();
        if (paddle != null)
        {
            float original = paddle.Speed;
            paddle.Speed = original * 0.5f;

            yield return new WaitForSeconds(duration);

            paddle.Speed = original;
        }
    }
}
