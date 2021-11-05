using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreManager : MonoBehaviour
{
    public static UIScoreManager instance = null;
    public Text CheckPointTxt;
    public Text TimerTxt;
    public Text ScoreTxt;

    private int cp, score;
    public int cpMax, scoreStart;
    private float timer;
    private float addScore;
    public float timerStart;
    public bool pause;

    public float timeDrop;
    public GameObject timeDropGO;
    public Text StartText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
    private void Start()
    {
        ResetCheckPoint();
        ResetScore();
        ResetTimer();
    }
    // Update is called once per frame
    void Update()
    {
        timeDrop -= Time.deltaTime;
        StartText.text = timeDrop.ToString("F0");
        if (timeDrop < 1)
        {
            timeDropGO.SetActive(false);
            UpdateTimer();
        }
    }
    public void ResetCheckPoint()
    {
        cp = cpMax;
        addScore = Mathf.Ceil(timer);
        UpdateScore((int)addScore);
        CheckPointTxt.text = "CP : " + cp + " / " + cpMax;
        ResetTimer();
    }
    public void ResetTimer()
    {
        timer = timerStart;
        TimerTxt.text = "Time : " + timerStart.ToString("F0");
    }
    public void ResetScore()
    {
        score = scoreStart;
        ScoreTxt.text = "Score : " + score.ToString();
    }

    public void UpdateCheckPoint()
    {
        cp--;
        UpdateScore(1);
        if (cp <= 0) ResetCheckPoint();
        CheckPointTxt.text = "CP : " + cp + " / " + cpMax;
    }
    public void UpdateTimer()
    {
        if (!pause)
        {
            timer -= Time.deltaTime;
            TimerTxt.text = "Time : " + timer.ToString("F0");
            if (timer <= 0) Gino.instance.player.GameOver();
        }
    }
    public void AddTime(float amount)
    {
        timer += amount;
        TimerTxt.text = "Time : " + timer.ToString("F0");
    }
    public void UpdateScore(int amount)
    {
        score += amount;
        ScoreTxt.text = "Score : " + score.ToString();
    }
    
    public void Resume()
    {
        StartCoroutine(ResumeTime());
    }

    IEnumerator ResumeTime()
    {
        timeDrop = 3;
        timeDropGO.SetActive(true);
        yield return new WaitForSeconds(3);
        Gino.instance.player.PauseGame(false);
        pause = false;
    }

    public int GetScore()
    {
        return score;
    }
}
