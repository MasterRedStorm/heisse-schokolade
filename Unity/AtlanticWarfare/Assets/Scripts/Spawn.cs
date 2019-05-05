using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        foreach (var city in cities)
        {
            StartCoroutine(SpawnTimer(city));
        }
    }

    public void AddCity(GameObject city)
    {
        if (!city.CompareTag("PlayerStructure"))
        {
            return;
        }

        StartCoroutine(SpawnTimer(city));
        cities.Add(city);
    }

    private void SpawnOne(Transform cityTransform)
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemies)
        {
            return;
        }

        var cityPosition = cityTransform.position;
        var mapCenterPosition = mapCenter.position;

        var enemy = Instantiate(enemyPrefab);
        var spawnTransform = enemy.GetComponent<Transform>();

        var spawnPosition = FindSpawnPosition(spawnTransform, mapCenterPosition, cityPosition);

        if (spawnPosition == Vector3.zero)
        {
            // TODO get fallback position
            Destroy(enemy);
        }
        else
        {
            spawnTransform.position = spawnPosition;            
        }
    }

    private Vector3 FindSpawnPosition(Transform spawnTransform, Vector3 mapCenterPosition, Vector3 basePosition)
    {
        var relativePosition = mapCenterPosition - basePosition;
        var rotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        for (var i = 0; i < 3; i++)
        {
            var position = basePosition;
        
            spawnTransform.rotation = rotation;
            var random = new System.Random();
            var nextDouble = (float) (random.NextDouble() - 0.5);
            spawnTransform.RotateAround(basePosition, Vector3.up, nextDouble * 180);

            var spawnRadius = (float) (spawnRadiusMin + random.NextDouble() * (spawnRadiusMax - spawnRadiusMin)) + i * 10f;

            position -= spawnTransform.forward * spawnRadius;

            var n = position + Vector3.up * 100;
            var didHit = Physics.Raycast(n, -Vector3.up, out var hit, Mathf.Infinity);

            if (didHit && hit.point.y > waterLevel)
            {
                return hit.point;
            }
        }

        return Vector3.zero;
    }

    private IEnumerator SpawnTimer(GameObject city)
    {
        while (city.activeSelf)
        {
            yield return new WaitForSeconds(spawnCooldown);

            if (city == null || city.activeSelf == false)
            {
                yield break;
            }

            SpawnOne(city.GetComponent<Transform>());
        }
    }

    public int maxEnemies = 100;

    public float spawnCooldown = 2f;

    public float spawnRadiusMin = 50f;

    public float spawnRadiusMax = 70f;

    public GameObject enemyPrefab;

    public List<GameObject> cities;

    public Transform mapCenter;

    public float waterLevel = 12f;
}