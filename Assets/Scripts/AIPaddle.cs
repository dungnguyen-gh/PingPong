using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour, IPaddleController
{
    [SerializeField] private Transform ball;
    private Rigidbody ballRb;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float basePredictionError = 0.05f;
    [SerializeField] private float baseReactionDelay = 0.1f;
    [SerializeField] private float maxPredictionError = 0.4f;
    [SerializeField] private float maxReactionDelay = 0.4f;
    [SerializeField] private float smoothTimeMin = 0.05f;
    [SerializeField] private float smoothTimeMax = 0.4f;

    private float minZ = -1.4f;
    private float maxZ = 1.4f;
    private float nextUpdateTime = 0f;
    private float targetZ = 0f;
    private float zVelocity = 0f;

    private Vector3 originalScale;
    private float originalMinZ;
    private float originalMaxZ;

    public float Speed { get => moveSpeed; set => moveSpeed = value; }
    public float MinZ { get => minZ; set => minZ = value; }
    public float MaxZ { get => maxZ; set => maxZ = value; }

    private void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();

        originalScale = transform.localScale;
        originalMinZ = minZ;
        originalMaxZ = maxZ;
    }

    private void FixedUpdate()
    {
        if (ballRb.velocity.x < 0f) return;

        float ballSpeed = ballRb.velocity.magnitude;

        // Nervousness increases as ball speed increases
        float nervousFactor = Mathf.InverseLerp(5f, 15f, ballSpeed); // adjust range to your game's speed
        float predictionError = Mathf.Lerp(basePredictionError, maxPredictionError, nervousFactor);
        float reactionDelay = Mathf.Lerp(baseReactionDelay, maxReactionDelay, nervousFactor);
        float smoothTime = Mathf.Lerp(smoothTimeMin, smoothTimeMax, nervousFactor);

        // Update targetZ occasionally (simulate reaction delay)
        if (Time.time >= nextUpdateTime)
        {
            float offset = Random.Range(-predictionError, predictionError);
            targetZ = ball.position.z + offset;
            nextUpdateTime = Time.time + reactionDelay;
        }

        // Move smoothly to targetZ
        float newZ = Mathf.SmoothDamp(transform.position.z, targetZ, ref zVelocity, smoothTime, moveSpeed);
        if (transform.position.z < minZ)
        {
            newZ = Mathf.MoveTowards(transform.position.z, minZ, moveSpeed * Time.deltaTime);
        }
        else if (transform.position.z > maxZ)
        {
            newZ = Mathf.MoveTowards(transform.position.z, maxZ, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Normal smooth movement
            newZ = Mathf.SmoothDamp(transform.position.z, targetZ, ref zVelocity, smoothTime, moveSpeed);
            newZ = Mathf.Clamp(newZ, minZ, maxZ);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    public void ResetPowerUpEffect()
    {
        // Reset clamp zone and scale
        transform.localScale = originalScale;
        minZ = originalMinZ;
        maxZ = originalMaxZ;

        // Reset tracking values
        nextUpdateTime = 0f;
        targetZ = transform.position.z;
        zVelocity = 0f;
    }
}
