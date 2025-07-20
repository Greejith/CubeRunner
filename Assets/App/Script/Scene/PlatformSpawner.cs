using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject[] platformPrefabs; 
    public int poolSize = 10;
    public float spawnDistance = 30f;
    public float platformLength = 10f;

    public bool isGameActive = true;

    private Queue<GameObject> platformPool = new Queue<GameObject>();
    private float nextSpawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
            GameObject platform = Instantiate(prefab);
            platform.SetActive(false);
            platformPool.Enqueue(platform);
        }
        SpawnPlatform();
    }

    void Update()
    {
        if (!player.GetComponent<PlayerController>().isGameStarted) return;

        while (player.position.z + spawnDistance > nextSpawnZ)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject platform = platformPool.Dequeue();

        platform.transform.position = new Vector3(0, 0, nextSpawnZ);
        platform.transform.rotation = Quaternion.identity;
        platform.SetActive(true);

        nextSpawnZ += platformLength;

        platformPool.Enqueue(platform);
    }
}
