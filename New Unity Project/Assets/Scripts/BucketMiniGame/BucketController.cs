using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    public int index;
    public GameObject MiniGame_Parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            GetComponent<Animator>().SetTrigger("Collid");
            if (index == 0)
            {
                if (collision.gameObject.layer == 8)
                    UIMiniGameManager.instance.currentScore++;
                if (collision.gameObject.layer == 9)
                    LoseMiniGame();
            }
            if (index == 1)
            {
                if (collision.gameObject.layer == 8)
                    LoseMiniGame();
                if (collision.gameObject.layer == 9)
                    UIMiniGameManager.instance.currentScore++;
            }
            UIMiniGameManager.instance.HandleScore();
            Destroy(collision.gameObject);
        }
    }

    private void LoseMiniGame()
    {
        UIScoreManager.instance.UpdateScore(UIMiniGameManager.instance.currentScore);
        Gino.instance.player.PauseGame(false);
        MiniGame_Parent.SetActive(false);
    }

    private void OnEnable()
    {
         Gino.instance.player.PauseGame(true);
    }

}
