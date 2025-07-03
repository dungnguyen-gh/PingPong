using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPaddleController
{
    [SerializeField] private string verticalInput = "Vertical";
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float minZ = -1.4f;
    [SerializeField] private float maxZ = 1.4f;

    private Vector3 originalScale;
    private float originalMinZ;
    private float originalMaxZ;

    public float Speed { get => moveSpeed; set => moveSpeed = value; }
    public float MinZ { get => minZ; set => minZ = value; }
    public float MaxZ { get => maxZ; set => maxZ = value; }

    private void Start()
    {
        originalScale = transform.localScale;
        originalMinZ = minZ;
        originalMaxZ = maxZ;
    }
    private void Update()
    {
        float moveY = Input.GetAxis(verticalInput) * moveSpeed * Time.deltaTime;
        float newZ = transform.position.z + moveY;

        // Clamp paddle position
        float clampedZ = Mathf.Clamp(newZ, minZ, maxZ);

        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
    }
    public void ResetPowerUpEffect()
    {
        transform.localScale = originalScale;
        minZ = originalMinZ;
        maxZ = originalMaxZ;
        moveSpeed = 7f;
    }
}
