using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kichta : MonoBehaviour
{
    public float speedRotation;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = transform.rotation.eulerAngles + new Vector3(0, speedRotation * Time.deltaTime, 0);
    }
}
