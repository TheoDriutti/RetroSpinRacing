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
    public float slowLerp;
    public float slowLerpTime;
    public int coinScoreValue;
    private Lanes currentLane;
    private float currentTime = 0f;
    private int inputTapNumber = 0;
    private float speed;
    private float currentTapNumberTimer = 0f;
    private float oldLerpTime;
    private Coroutine slowed;
    private bool gameOver = false;
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
        oldLerpTime = lerpTime;
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
        if (!gameOver)
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
        if (!gameOver)
        {
            currentTapNumberTimer += Time.deltaTime;

            if (currentTapNumberTimer > resetTapNumberTimer)
            {
                speed = speedModifierInInterval.Evaluate(inputTapNumber / 100f) * 100;
                Gino.instance.spawnManager.objectSpeed = speed;
                Gino.instance.decorManager.decorSpeed = speed;
                Gino.instance.decorManager.lampSpeed = speed;
                inputTapNumber = 0;
                currentTapNumberTimer = 0f;
            }
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    private void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.GetComponent<RoadObject>() != null)
        {
            Debug.Log("Touched");
            switch (other.gameObject.GetComponent<RoadObject>().type)
            {
                case SpawnManager.RoadObjectIdentity.EMPTY:
                    break;
                case SpawnManager.RoadObjectIdentity.VEHICULE:
                    GameOver();
                    break;
                case SpawnManager.RoadObjectIdentity.SLOW:
                    if (slowed == null)
                    {
                        slowed = StartCoroutine(SlowLerpTimer(slowLerpTime));
                    }
                    break;
                case SpawnManager.RoadObjectIdentity.COIN:
                    UIScoreManager.instance.UpdateScore(coinScoreValue);
                    break;
                case SpawnManager.RoadObjectIdentity.BONUS:
                    break;
                default:
                    break;
            }
        }
    }

    void GameOver()
    {
        Gino.instance.spawnManager.objectSpeed = 0;
        gameOver = true;
    }

    IEnumerator SlowLerpTimer(float timer)
    {
        lerpTime = slowLerp;
        yield return new WaitForSeconds(timer);
        lerpTime = oldLerpTime;
        slowed = null;
    }
}
