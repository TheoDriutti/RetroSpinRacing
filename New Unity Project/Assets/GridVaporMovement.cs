using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVaporMovement : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Gino.instance.decorManager.decorSpeed/2 * Time.deltaTime);
        if (transform.position.z < -100f)
        {
            Gino.instance.decorManager.SpawnRoadBG();
            Destroy(this.gameObject);
        }
    }
}
