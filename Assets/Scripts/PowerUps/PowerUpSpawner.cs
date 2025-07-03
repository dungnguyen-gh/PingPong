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

    private List<GameObject> pool = new List<GameObject>();
    private GameObject lastSpawned = null;

    private void Start()
    {
        foreach (var prefab in powerUpPrefabs)
        {
            GameObject item = Instantiate(prefab, Vector3.one * 1000f, prefab.transform.rotation);
            item.SetActive(false);
            pool.Add(item);
        }
    }
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
                GameObject chosenItem = GetAvailablePowerUp();

                if (chosenItem != null)
                {
                    Vector3 pos = new Vector3(
                        Random.Range(spawnAreaX.x, spawnAreaX.y),
                        0.5f,
                        Random.Range(spawnAreaZ.x, spawnAreaZ.y)
                    );

                    chosenItem.transform.position = pos;
                    chosenItem.SetActive(true);
                    current = chosenItem;
                    lastSpawned = chosenItem;
                }
                
            }
            yield return new WaitForSeconds(spawnCooldown);

        }
    }

    private GameObject GetAvailablePowerUp()
    {
        var available = pool.Where(item => !item.activeInHierarchy).ToList();

        if (available.Count == 0) return null;

        if (available.Count > 1 && lastSpawned != null)
        {
            available.Remove(lastSpawned);
        }

        return available[Random.Range(0, available.Count)];
    }

    public void PowerUpCollected(GameObject go)
    {
        if (go == current)
        {
            current = null;
            
            go.transform.position = Vector3.one * 1000f;
        }
    }

    public void ResetPowerUps()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        foreach (var item in pool) 
        { 
            item.SetActive(false);
            item.transform.position = Vector3.one * 1000f;
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
