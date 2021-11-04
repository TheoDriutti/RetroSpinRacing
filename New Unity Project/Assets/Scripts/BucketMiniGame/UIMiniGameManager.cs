using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniGameManager : MonoBehaviour
{
    public static UIMiniGameManager instance = null;

    public int currentScore = 0;
    public Text scoreText;
    public Text StartText;
    public Image nextItem;
    public Sprite[] imgs;
    public float timeDrop;
    public GameObject timeDropGO;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;
        timeDrop = 3;
    }
    private void Update()
    {
        timeDrop -= Time.deltaTime;
        StartText.text = timeDrop.ToString("F0");
        if(timeDrop < 1) timeDropGO.SetActive(false);
    }
    public void HandleScore()
    {
        scoreText.text = "Score: " + currentScore;
    }
    public void SetNextItem(int index)
    {
        nextItem.sprite = imgs[index];
    }
    private void OnEnable()
    {
        timeDrop = 3;
        timeDropGO.SetActive(true);
    }
}
