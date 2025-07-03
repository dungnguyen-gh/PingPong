using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs;
    [SerializeField] private float initialDelay = 10f;
    [SerializeField] private float spawnCooldown = 10f;
    [SerializeField] private Vector2 spawnAreaX = new Vector2(-1.5f, 1.5f);
    [SerializeField] private Vector2 spawnAreaZ = new Vector2(-1.2f, 1.2f);

    private GameObject current = null;
    private Coroutine spawnCoroutine = null;


    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine(initialDelay));
        }
    }

    private IEnumerator SpawnRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (current == null)
            {
                Vector3 pos = new Vector3(
                    Random.Range(spawnAreaX.x, spawnAreaX.y),
                    0.5f,
                    Random.Range(spawnAreaZ.x, spawnAreaZ.y)
                );

                var prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
                Quaternion prefabRot = prefab.gameObject.transform.rotation;
                current = Instantiate(prefab, pos, prefabRot);
            }
            yield return new WaitForSeconds(spawnCooldown);

        }
    }
    public void PowerUpCollected(GameObject go)
    {
        if (go == current) 
            current = null;
    }

    public void ResetPowerUps()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // destroy any onÅ]table pickups
        foreach (var pu in GameObject.FindGameObjectsWithTag("PowerUp"))
        {
            Destroy(pu);
        }
            
        current = null;

        // reset any active effects
        foreach (var r in FindObjectsOfType<MonoBehaviour>().OfType<IPowerUpResettable>())
        {
            r.ResetPowerUpEffect();
        }

        spawnCoroutine = StartCoroutine(SpawnRoutine(5f));
    }
}
