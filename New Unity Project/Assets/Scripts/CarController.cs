using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Lanes
    {
        LEFT,
        CENTER,
        RIGHT
    }

    [System.Serializable]
    public struct Lane
    {
        public Lanes lane;
        public Transform trans;
    }

    public float time;
    public Lane[] lanes;
    private Lanes currentLane;
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentLane = Lanes.CENTER;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCurrentLane() || currentTime < time)
        {
            ChangeLane();
        }
    }

    bool CheckCurrentLane()
    {
        if (currentLane > Lanes.LEFT)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentLane -= 1;
                currentTime = 0f;
                return true;
            }
        }
        if (currentLane < Lanes.RIGHT)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentLane += 1;
                currentTime = 0f;
                return true;
            }
        }
        return false;
    }

    Vector3 LerpOriginToTarget(Vector3 origin, Vector3 target, float time)
    {
        currentTime += Time.deltaTime;
        return Vector3.Lerp(origin, target, currentTime / time);
    }

    void ChangeLane()
    {
        //transform.position = lanes[(int)currentLane].trans.position;
        transform.position = LerpOriginToTarget(transform.position, lanes[(int)currentLane].trans.position, time);
    }

}
