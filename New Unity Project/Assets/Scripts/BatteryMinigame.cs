using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryMinigame : MonoBehaviour
{
    public Transform needle;
    public Text timerText;

    public float angularSpeed;
    public float lerpSpeed;
    public float decaySpeed;
    public float minigameDuration;

    public float angleAmplitude;

    public float trancheScore;
    public float maxSpeedScore;

    private bool isContact = false;
    private float targetAngle;
    private float minigameTimer = 0f;
    private bool minigameActive = true;

    // Start is called before the first frame update
    void Start()
    {
        targetAngle = angleAmplitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (minigameActive)
        {
            if (minigameTimer < minigameDuration)
            {
                minigameTimer += Time.deltaTime;
                timerText.text = (minigameDuration - minigameTimer).ToString("F1") + "s";

                RegisterInputs();
                MoveNeedle();
            }

            else
            {
                Finish();
            }
        }
    }

    void MoveNeedle()
    {
        if (targetAngle < angleAmplitude)
        {
            targetAngle += Time.deltaTime * decaySpeed;
        }

        targetAngle = Mathf.Clamp(targetAngle, -angleAmplitude, angleAmplitude);
        needle.rotation = Quaternion.Lerp(needle.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * lerpSpeed);
    }

    void Finish()
    {
        float anglePercentage = (-targetAngle + angleAmplitude) / (2 * angleAmplitude);
        float score = anglePercentage * maxSpeedScore;
        score -= score % trancheScore;

        minigameActive = false;
    }

    void RegisterInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isContact) isContact = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isContact) isContact = false;

            targetAngle -= angularSpeed;
        }
    }
}
