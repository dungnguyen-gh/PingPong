using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float reactionDelay = 0.1f;

    private float nextUpdateTime;
    private float targetZ;

    private void FixedUpdate()
    {
        if (Time.time >= nextUpdateTime)
        {
            targetZ = ball.position.z;
            nextUpdateTime = Time.time + reactionDelay;
        }

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, targetZ);
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

        float clampedZ = Mathf.Clamp(transform.position.z, -1.4f, 1.4f);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
        
    }
}
