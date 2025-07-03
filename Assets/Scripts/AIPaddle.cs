using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour, IPaddleController
{
    [SerializeField] private Transform ball;
    private Rigidbody ballRb;

    [SerializeField] private float baseMoveSpeed = 7f;
    [SerializeField] private float maxMoveSpeed = 14f; //max speed according to ball speed

    [SerializeField] private float basePredictionError = 0.05f; //inaccurate rate when predict the ball z
    [SerializeField] private float maxPredictionError = 0.3f; //max error when nervous

    [SerializeField] private float baseReactionDelay = 0.1f; 
    [SerializeField] private float maxReactionDelay = 0.3f;

    [SerializeField] private float smoothTimeMin = 0.04f;
    [SerializeField] private float smoothTimeMax = 0.2f;

    private float minZ;
    private float maxZ;

    private float nextUpdateTime = 0f;
    private float targetZ = 0f;
    private float zVelocity = 0f;

    private Vector3 originalScale;
    private float normalMinZ = -1.4f;
    private float normalMaxZ = 1.4f;
    private float bigMinZ = -0.8f;
    private float bigMaxZ = 0.8f;

    public float Speed { get => baseMoveSpeed; set => baseMoveSpeed = value; }
    public float MinZ { get => minZ; set => minZ = value; }
    public float MaxZ { get => maxZ; set => maxZ = value; }

    private void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        originalScale = transform.localScale;

        minZ = normalMinZ;
        maxZ = normalMaxZ;
    }

    private void FixedUpdate()
    {
        if (ballRb.velocity.x < 0f) return; //ignore the ball when it comes to the opponent

        float ballSpeed = ballRb.velocity.magnitude;

        // Nervousness increases with ball speed
        float nervousFactor = Mathf.InverseLerp(5f, 15f, ballSpeed); //nervous factor increase as the ball faster

        float predictionError = Mathf.Lerp(basePredictionError, maxPredictionError, nervousFactor);
        float reactionDelay = Mathf.Lerp(baseReactionDelay, maxReactionDelay, nervousFactor);
        float smoothTime = Mathf.Lerp(smoothTimeMin, smoothTimeMax, nervousFactor);
        float adaptiveMoveSpeed = Mathf.Lerp(baseMoveSpeed, maxMoveSpeed, nervousFactor);

        //reaction delay
        if (Time.time >= nextUpdateTime)
        {
            float offset = Random.Range(-predictionError, predictionError);
            targetZ = ball.position.z + offset;
            nextUpdateTime = Time.time + reactionDelay;
        }

        //smoothly move to the ball z
        float newZ = Mathf.SmoothDamp(transform.position.z, targetZ, ref zVelocity, smoothTime, adaptiveMoveSpeed);

        //clamp to current movement range
        if (transform.position.z < minZ - 0.05f)
        {
            newZ = Mathf.MoveTowards(transform.position.z, minZ, adaptiveMoveSpeed * Time.deltaTime);
        }
        else if (transform.position.z > maxZ + 0.05f)
        {
            newZ = Mathf.MoveTowards(transform.position.z, maxZ, adaptiveMoveSpeed * Time.deltaTime);
        }
        else
        {
            newZ = Mathf.Clamp(newZ, minZ, maxZ);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    public void ResetPowerUpEffect()
    {
        transform.localScale = originalScale;
        minZ = normalMinZ;
        maxZ = normalMaxZ;

        nextUpdateTime = 0f;
        targetZ = transform.position.z;
        zVelocity = 0f;
    }

    public void ApplyBigPaddleEffect()
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y, 2.4f);
        minZ = bigMinZ;
        maxZ = bigMaxZ;
    }
}
