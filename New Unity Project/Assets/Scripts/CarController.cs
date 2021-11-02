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

    public Lane[] lanes;
    private Lanes currentLane;

    // Start is called before the first frame update
    void Start()
    {
        currentLane = Lanes.CENTER;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCurrentLane())
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
                return true;
            }
        }
        if (currentLane < Lanes.RIGHT)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentLane += 1;
                return true;
            }
        }
        return false;
    }

    void ChangeLane()
    {
        transform.position = lanes[(int)currentLane].trans.position;
    }
}
