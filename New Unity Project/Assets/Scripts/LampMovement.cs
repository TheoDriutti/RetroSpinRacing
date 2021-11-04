using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Gino.instance.decorManager.lampSpeed * Time.deltaTime);
        if (transform.position.z < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
