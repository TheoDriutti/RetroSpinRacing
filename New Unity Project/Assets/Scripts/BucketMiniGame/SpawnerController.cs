using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject[] Item;
    Vector3 pos;
    Vector3 ItemPos;
    public float interval;
    private float timerDrop;
    public float timeToSpawn;
    private int itemToDrop;
    private float timerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timerDrop -= Time.deltaTime;
        timerSpeed += Time.deltaTime;
        if (timerSpeed > 5)
        {
            timerSpeed = 0;
            if(timeToSpawn > 0.5f)timeToSpawn -= 0.2f;
        }
        if (timerDrop <= 0)
        {
            ItemPos.x = Random.Range(pos.x - interval, pos.x + interval);
            itemToDrop = Random.Range(0, 2);
            UIMiniGameManager.instance.SetNextItem(itemToDrop);
            Instantiate(Item[itemToDrop], new Vector3(ItemPos.x, pos.y, pos.z), Quaternion.identity, transform);
            timerDrop = timeToSpawn;
        }

    }
}
