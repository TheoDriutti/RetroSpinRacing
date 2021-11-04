using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    public int index;

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
                    Gino.instance.isPause;
            }
            if (index == 1)
            {
                if (collision.gameObject.layer == 8)
                    //End
                if (collision.gameObject.layer == 9)
                    UIMiniGameManager.instance.currentScore++;
            }
            UIMiniGameManager.instance.HandleScore();
            Destroy(collision.gameObject);
        }
    }

}
