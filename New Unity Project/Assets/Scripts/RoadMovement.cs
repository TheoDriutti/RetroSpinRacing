using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class RoadMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Gino.instance.decorManager.decorSpeed * Time.deltaTime);
        if (transform.position.z < -20f)
        {
            Gino.instance.decorManager.SpawnRoad();
            Destroy(this.gameObject);
        }
    }
}
