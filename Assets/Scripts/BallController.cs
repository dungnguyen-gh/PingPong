using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float speedIncreasePerHit = 0.2f;
    private float currentSpeed;
    private Rigidbody rb;
    private bool gameStarted = false;

    private bool hasScored = false;
    public bool HasScored => hasScored;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetBall();
        StopBall();
    }
    // Make sure the ball never moves flat
    private void FixedUpdate()
    {
        if (!gameStarted || currentSpeed <= 0.01f)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 velocity = rb.velocity;
        if (Mathf.Abs(velocity.z) < 0.1f)
        {
            velocity.z = Mathf.Sign(velocity.z) * 0.5f;
        }
        rb.velocity = velocity.normalized * currentSpeed;
    }
    public void OnScored()
    {
        hasScored = true;
        StopBall();
    }
    public void LaunchBall()
    {
        if (gameStarted) return;

        currentSpeed = baseSpeed;
        Vector3 dir = new Vector3(Random.value > 0.5f ? 1 : -1, 0, Random.Range(-0.5f, 0.5f)).normalized;
        rb.velocity = dir * currentSpeed;
        gameStarted = true;
    }
    public void ResetBall()
    {
        gameStarted = false;
        hasScored = false;

        ResetRb();

        transform.position = Vector3.zero;
    }
    public void StopBall()
    {
        gameStarted = false;

        ResetRb();
    }
    private void ResetRb()
    {
        currentSpeed = 0f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            if (hasScored) return;

            ContactPoint contact = collision.GetContact(0);
            Vector3 hitPoint = contact.point;
            Vector3 paddleCenter = collision.transform.position;

            float zOffset = hitPoint.z - paddleCenter.z;

            // Use offset to influence angle
            float verticalInfluence = Mathf.Clamp(zOffset * 2f, -1.0f, 1.0f);

            // Determine X direction based on which side the paddle is
            float xDir = transform.position.x < 0 ? 1f : -1f;

            // Set new direction
            Vector3 newDir = new Vector3(xDir, 0, verticalInfluence).normalized;

            currentSpeed += speedIncreasePerHit;
            rb.velocity = newDir * currentSpeed;

            AudioManager.instance.PlayBounce();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 vel = rb.velocity;

            // If Z is nearly 0, reintroduce vertical direction
            if (Mathf.Abs(vel.z) < 0.2f)
            {
                float sign = Mathf.Sign(vel.z) != 0 ? Mathf.Sign(vel.z) : (Random.value > 0.5f ? 1 : -1);
                vel.z = Random.Range(0.3f, 0.6f) * sign;
                rb.velocity = vel.normalized * currentSpeed;
            }
        }
    }
}
