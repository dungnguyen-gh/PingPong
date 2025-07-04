using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBallPowerUp : PowerUp
{
    protected override IEnumerator ApplyEffect(GameObject collector, GameObject opponent)
    {
        var ball = FindObjectOfType<BallController>();

        ball.transform.localScale = ball.OriginalScale * 2f;
        
        yield return new WaitForSeconds(duration);

        ball.ResetEffects();
    }
}
