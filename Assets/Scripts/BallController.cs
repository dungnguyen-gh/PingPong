using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float baseSpeed = 4f;
    private float speedIncreasePerHit = 0.1f;
    private float currentSpeed;
    private Rigidbody rb;
    private bool gameStarted = false;
    private Vector3 originalScale;
    public Vector3 OriginalScale => originalScale;
    private bool hasScored = false;
    public bool HasScored => hasScored;

    private GameObject lastHitPaddle;
    public GameObject LastHitPaddle => lastHitPaddle;

    [SerializeField] PlayerIdentifier[] playerIdentifiers = null;
    public float GetSpeed() => currentSpeed;
    public void SetSpeed(float speed) => currentSpeed = Mathf.Max(speed, 0.1f);

    [SerializeField] PowerUpSpawner spawner = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
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

        // rotation for the ball
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, rb.velocity.normalized);
        rb.angularVelocity = rotationAxis * currentSpeed;
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

        ResetEffects();
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
            lastHitPaddle = collision.gameObject;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if (lastHitPaddle == null) return; // Prevent applying power up if no paddle touched yet

            PowerUp powerUp = other.GetComponent<PowerUp>();
            if (powerUp != null)
            {
                GameObject collector = lastHitPaddle;
                GameObject opponent = FindOpponent(collector);

                if (spawner != null)
                {
                    powerUp.Collect(collector, opponent, spawner);
                    AudioManager.instance.PlayCollectPowerUp();
                }
            }
        }
    }
    private GameObject FindOpponent(GameObject collector)
    {
        var identifier = collector.GetComponent<PlayerIdentifier>();
        if (identifier == null) return null;

        PlayerIdentifier.PlayerType opponentType = identifier.playerType == PlayerIdentifier.PlayerType.Player1
            ? PlayerIdentifier.PlayerType.Player2
            : PlayerIdentifier.PlayerType.Player1;

        foreach (var id in playerIdentifiers)
        {
            if (id.playerType == opponentType) return id.gameObject;
        }

        return null;
    }
    public void ResetEffects()
    {
        SetSpeed(baseSpeed);
        transform.localScale = originalScale;
    }
}
