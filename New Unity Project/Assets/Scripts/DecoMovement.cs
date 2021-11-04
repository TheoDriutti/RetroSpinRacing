using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Gino.instance.decorManager.decorSpeed * Time.deltaTime);
        if (transform.position.z < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
