using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorManager : MonoBehaviour
{
    public float decorSpeed;
    public float spawnDelay;
    public GameObject[] decors;
    public Transform[] spawnPoint;
    public int range;

    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnObjectTimer();   
    }

    void SpawnObject()
    {
        int randomSpawnPoint = Random.Range(0, 2);
        GameObject obj = Instantiate(decors[Random.Range(0, decors.Length)], spawnPoint[randomSpawnPoint], false);
        obj.transform.position = new Vector3(Random.Range(spawnPoint[randomSpawnPoint].position.x - range, spawnPoint[randomSpawnPoint].position.x + range), spawnPoint[randomSpawnPoint].position.y, spawnPoint[randomSpawnPoint].position.z);
        currentTime = 0f;
    }

    void SpawnObjectTimer()
    {
        currentTime += Time.deltaTime;

        if (currentTime > spawnDelay)
        {
            SpawnObject();
        }
    }
}
