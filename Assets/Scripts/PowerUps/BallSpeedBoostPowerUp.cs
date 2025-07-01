using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpeedBoostPowerUp : PowerUp
{
    protected override IEnumerator ApplyEffect(GameObject collector, GameObject opponent)
    {
        var ball = FindObjectOfType<BallController>();
        float original = ball.GetSpeed();

        ball.SetSpeed(original * 1.5f);
        yield return new WaitForSeconds(duration);
        ball.SetSpeed(original);
    }
}
