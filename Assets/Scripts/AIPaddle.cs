using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    [SerializeField] private Transform ball;
    private Rigidbody ballRb;

    [SerializeField] private float baseSmoothness = 6f;
    [SerializeField] private float nervousSmoothnessFactor = 0.2f;
    [SerializeField] private float reactionDelay = 0.1f;
    [SerializeField] private float predictionError = 0.1f;

    private float nextUpdateTime;
    private float targetZ;

    private void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (ballRb.velocity.x < 0f) return;

        // Simulate nervousness
        float ballSpeed = ballRb.velocity.magnitude;
        float smoothness = baseSmoothness - (ballSpeed * nervousSmoothnessFactor);
        smoothness = Mathf.Clamp(smoothness, 1f, baseSmoothness);

        if (Time.time >= nextUpdateTime)
        {
            targetZ = ball.position.z + Random.Range(-predictionError, predictionError);
            nextUpdateTime = Time.time + reactionDelay;
        }

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, targetZ);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothness * Time.deltaTime);

        float clampedZ = Mathf.Clamp(transform.position.z, -1.4f, 1.4f);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
    }
}
