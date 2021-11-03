using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) {
        Gino.instance.spawnManager.AddToRecycleList(collider.gameObject.GetComponent<RoadObject>());
    }
}
