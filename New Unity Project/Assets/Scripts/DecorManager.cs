using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorManager : MonoBehaviour
{
    public float decorSpeed;
    public float lampSpeed;
    public float spawnDelay;
    public float lampSpawnDelay;
    public GameObject[] decors;
    public GameObject lampadaire;
    public Transform[] spawnPoint;
    public Transform[] lampSpawns;
    public int range;

    private float currentTime = 0f;
    private float lampTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpawnObjectTimer();
        SpawnLamp();
    }

    void SpawnObject()
    {
        int randomSpawnPoint = Random.Range(0, 2);
        GameObject obj = Instantiate(decors[Random.Range(0, decors.Length)], spawnPoint[randomSpawnPoint], false);
        obj.AddComponent<DecoMovement>();
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

    void SpawnLamp()
    {
        if (lampTime < lampSpawnDelay)
        {
            lampTime += Time.deltaTime;
        }
        else
        {
            Debug.Log("instantiate");
            GameObject obj = Instantiate(lampadaire, lampSpawns[0], false);
            GameObject obj2 = Instantiate(lampadaire, lampSpawns[1], false);
            obj.AddComponent<LampMovement>();
            obj2.AddComponent<LampMovement>();
            obj.transform.Rotate(new Vector3(0, 0, 180));
            lampTime = 0f;
        }
    }

    void SpawnRoad()
    {
        //if ()
    }
}
