using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Lanes
    {
        CRASH_LEFT,
        LEFT,
        CENTER,
        RIGHT,
        CRASH_RIGHT
    }

    [System.Serializable]
    public struct Lane
    {
        public Lanes lane;
        public Transform trans;
    }

    public float lerpTime;
    public float crashTime;
    public Lane[] lanes;
    public float resetTapNumberTimer;
    public AnimationCurve speedModifierInInterval;
    private Lanes currentLane;
    private float currentTime = 0f;
    private int inputTapNumber = 0;
    private float speed;
    private float currentTapNumberTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentLane = Lanes.CENTER;
        if (speedModifierInInterval == null)
        {
            speedModifierInInterval = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        }
        speedModifierInInterval.preWrapMode = WrapMode.Clamp;
        speedModifierInInterval.postWrapMode = WrapMode.Clamp;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCurrentLane();
        if (currentTime < lerpTime)
        {
            ChangeLane();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            AddTap();
        }

        SetSpeed();
    }

    void CheckCurrentLane()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentLane > Lanes.CRASH_LEFT)
            {
                currentLane -= 1;
                currentTime = 0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < Lanes.CRASH_RIGHT)
            {
                currentLane += 1;
                currentTime = 0f;
            }
        }

        switch (currentLane)
        {
            case Lanes.CRASH_LEFT:
                break;
            case Lanes.LEFT:
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -10));
                break;
            case Lanes.CENTER:
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0));
                break;
            case Lanes.RIGHT:
                transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 10));
                break;
            case Lanes.CRASH_RIGHT:
                break;
            default:
                break;
        }
    }

    Vector3 LerpOriginToTarget(Vector3 origin, Vector3 target, float time)
    {
        currentTime += Time.deltaTime;
        return Vector3.Lerp(origin, target, currentTime / time);
    }

    void ChangeLane()
    {
        //transform.position = lanes[(int)currentLane].trans.position;
        if (currentLane != Lanes.CRASH_LEFT && currentLane != Lanes.CRASH_RIGHT)
        {
            transform.position = LerpOriginToTarget(transform.position, lanes[(int)currentLane].trans.position, lerpTime);
        }
        else
        {
            Crash();
        }
    }

    void Crash()
    {
        if (currentTime < crashTime)
        {
            transform.position = LerpOriginToTarget(transform.position, lanes[(int)currentLane].trans.position, crashTime);
        }
        else if (currentLane == Lanes.CRASH_LEFT)
        {
            currentLane += 1;
        }
        else
        {
            currentLane -= 1;
        }
    }

    void AddTap()
    {
        inputTapNumber++;
    }

    void SetSpeed()
    {
        currentTapNumberTimer += Time.deltaTime;

        if (currentTapNumberTimer > resetTapNumberTimer)
        {
            speed = speedModifierInInterval.Evaluate(inputTapNumber / 100f) * 100;
            Gino.instance.spawnManager.objectSpeed = speed;
            Gino.instance.decorManager.decorSpeed = speed;
            //Gino.instance.decorManager.lampSpeed = speed;
            inputTapNumber = 0;
            currentTapNumberTimer = 0f;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }
}
