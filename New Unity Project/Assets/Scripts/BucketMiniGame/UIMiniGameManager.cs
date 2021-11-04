using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniGameManager : MonoBehaviour
{
    public static UIMiniGameManager instance = null;

    public int currentScore;
    public Text scoreText;
    public Image nextItem;
    public Sprite[] imgs;
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
    }

    public void HandleScore()
    {
        scoreText.text = "Score: " + currentScore;
    }
    public void SetNextItem(int index)
    {
        nextItem.sprite = imgs[index];
    }
}
