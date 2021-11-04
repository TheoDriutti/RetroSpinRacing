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
    public float timerStart;

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
        UpdateTimer();
    }
    public void ResetCheckPoint()
    {
        cp = cpMax;
        CheckPointTxt.text = "CP : " + cp + " / " + cpMax;
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
        CheckPointTxt.text = "CP : " + cp + " / " + cpMax;
    }
    public void UpdateTimer()
    {
        timer -= Time.deltaTime;
        TimerTxt.text = "Time : " + timer.ToString("F0");
        //if (timer <= 0) GameOver;
    }
    public void UpdateScore(int amount)
    {
        score += amount;
        ScoreTxt.text = "Score : " + score.ToString();
    }
}
