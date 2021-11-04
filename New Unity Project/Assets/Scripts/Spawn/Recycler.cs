using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<RoadObject>() != null)
            Gino.instance.spawnManager.AddToRecycleList(collider.gameObject.GetComponent<RoadObject>());
    }
}
