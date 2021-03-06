using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryMinigame : MonoBehaviour
{
    public float minigameDuration;

    [Header("References")]
    public Transform needle;
    public Transform jaugeMask;
    public Text timerText;

    [Header("Aiguille")]
    public float angularSpeed;
    public float lerpSpeed;
    public float decaySpeed;
    public float angleAmplitude;

    [Header("Jauge")]
    public float jaugeCoeff;
    public float jaugeSize;

    [Header("Score")]
    public float trancheScore;
    public float maxSpeedScore;

    private bool isContact = false;
    private float targetAngle;
    private float minigameTimer = 0f;
    private bool minigameActive = true;
    private int contactCounter;
    private int savedCounter;
    private float elapsedTime = 0f;

    public float timeDrop;
    public GameObject timeDropGO;
    public Text StartText;

    bool addTime;

    public GameObject MiniGame_Parent;

    // Start is called before the first frame update
    void Start()
    {
        targetAngle = angleAmplitude;
        timeDrop = 3;
    }

    // Update is called once per frame
    void Update()
    {
        timeDrop -= Time.deltaTime;
        StartText.text = timeDrop.ToString("F0");
        if (timeDrop < 1)
        {
            timeDropGO.SetActive(false);

            if (minigameActive)
            {
                if (minigameTimer < minigameDuration)
                {
                    minigameTimer += Time.deltaTime;
                    timerText.text = (minigameDuration - minigameTimer).ToString("F1") + "s";

                    RegisterInputs();
                    MoveNeedle();
                    MoveJauge();
                }
                else
                {
                    Finish();
                }
            }
            else
            {
                EndMiniGame();
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
        if ((-targetAngle + angleAmplitude) / (2 * angleAmplitude) > 0.99f) Finish();
        needle.rotation = Quaternion.Lerp(needle.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * lerpSpeed);
    }

    void MoveJauge()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > .5f)
        {
            elapsedTime = 0f;
            savedCounter = contactCounter;
            contactCounter = 0;
        }

        float newJaugeSize = savedCounter * jaugeSize / jaugeCoeff;
        jaugeMask.localScale = new Vector3(Mathf.Lerp(jaugeMask.localScale.x, newJaugeSize, Time.deltaTime * lerpSpeed / 2), jaugeMask.localScale.y, jaugeMask.localScale.z);
    }

    void Finish()
    {
        float anglePercentage = (-targetAngle + angleAmplitude) / (2 * angleAmplitude);
        float score = anglePercentage * maxSpeedScore;
        score -= score % trancheScore;
        //Debug.Log("Score : " + score);

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
            contactCounter++;
        }
    }

    private void EndMiniGame()
    {
        UIScoreManager.instance.AddTime(minigameDuration - minigameTimer);
        UIScoreManager.instance.Resume();
        MiniGame_Parent.SetActive(false);
    }

    private void OnEnable()
    {
        addTime = false;
        minigameActive = true;
        Gino.instance.player.PauseGame(true);
        ResetGame();
    }

    private void ResetGame()
    {
        timeDrop = 3;
        timeDropGO.SetActive(true);
        minigameTimer = 0;
        minigameActive = true;
    }
}
