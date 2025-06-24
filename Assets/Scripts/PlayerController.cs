using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string verticalInput = "Vertical";
    [SerializeField] private float moveSpeed = 7f;
    private void Update()
    {
        float moveY = Input.GetAxis(verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(0,0,moveY);

        float clampedZ = Mathf.Clamp(transform.position.z, -1.4f, 1.4f);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
    }
}
