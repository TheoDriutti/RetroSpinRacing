using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    int increment;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<RoadObject>() != null)
        {
            Gino.instance.spawnManager.AddToRecycleList(collider.gameObject.GetComponent<RoadObject>());
            increment++;
            if(increment >= 3)
            {
                increment = 0;
                UIScoreManager.instance.UpdateCheckPoint();
            }
        }
    }
}
