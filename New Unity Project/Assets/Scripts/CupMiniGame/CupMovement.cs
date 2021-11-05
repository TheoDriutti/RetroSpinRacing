using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupMovement : MonoBehaviour
{
    /*public Transform cup1Empty;
    public Transform cup2Empty;
    public Transform cup3Empty;*/

    public List<Transform> cupList;

    public float timeBetweenMovement;
    private int nbMovement;
    public int numberOfMovements; 

    /*public Transform cup1;
    public Transform cup2;
    public Transform cup3;*/

    // Time to move from sunrise to sunset position, in seconds.
    public float journeyTime;

    // The time at which the animation started.
    private float elapsedTime;

    private Vector3 centerUp = Vector3.zero;
    private Vector3 centerDown = Vector3.zero;
    private Vector3 riseRelCenter = Vector3.zero;
    private Vector3 setRelCenter = Vector3.zero;

    public Transform coinTransform;
    public SpriteRenderer coinSpriteRenderer;
    public Sprite cupOpened;
    public Sprite cupClosed;
    public Transform arrowTransform;

    public List<Transform> arrowList;
    public List<Transform> posList;
    private int iteratorArrow = 0;

    private Transform cupWithCoin;

    public Text EndText;

    public GameObject MiniGame_Parent;

    void Start()
    {
        // Note the time at the start of the animation.
        elapsedTime = 0.0f;
        //StartCoroutine(Sequencer());
        CoinRandomizer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ArrowRight();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ArrowLeft();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckWin();
        }
    }

    private void SetMovement(ref Transform cup1, ref Transform cup2)
    {
        StartCoroutine(SetMovementCoroutine(cup1, cup2));
    }

    IEnumerator SetMovementCoroutine(Transform cup1, Transform cup2)
    {
        Vector3 pos1 = cup1.position;
        Vector3 pos2 = cup2.position;

        Debug.Log(pos1);
        elapsedTime = 0.0f;

        while (elapsedTime < journeyTime)
        {
            elapsedTime += Time.deltaTime;
            float fracComplete = elapsedTime / journeyTime;

            // The center of the arc
            centerUp = (pos1 + pos2) * 0.5F;
            centerDown = (pos1 + pos2) * 0.5F;

            // move the center a bit downwards to make the arc vertical
            centerUp -= new Vector3(0, 1, 0);
            centerDown -= new Vector3(0, -1, 0);


            riseRelCenter = pos1 - centerUp;
            setRelCenter = pos2 - centerUp;
            cup1.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            cup1.position += centerUp;


            riseRelCenter = pos1 - centerDown;
            setRelCenter = pos2 - centerDown;
            cup2.position = Vector3.Slerp(setRelCenter, riseRelCenter, fracComplete);
            cup2.position += centerDown;
            yield return new WaitForEndOfFrame();
          
        }
        cup1.position = pos2;
        cup2.position = pos1;

        

        yield return null;
    }

    public void MovementRandomizer()
    {
        Transform randomCup1 = cupList[Random.Range(0, 3)];
        Transform randomCup2 = cupList[Random.Range(0, 3)];

        while (randomCup1.position == randomCup2.position)
        {
            randomCup2 = cupList[Random.Range(0, 3)];
        }

        SetMovement(ref randomCup1, ref randomCup2);
        nbMovement++;
    }

    IEnumerator Sequencer()
    {
        if(nbMovement < numberOfMovements)
        {
            MovementRandomizer();
            yield return new WaitForSeconds(timeBetweenMovement);
            StartCoroutine(Sequencer());
        }
        else
        {
            SpawnArrow();
            yield return null;
        }
    }

    public void CoinRandomizer()
    {
        Transform randomCupCoin = cupList[Random.Range(0, 3)];
        cupWithCoin = randomCupCoin.transform;
        Debug.Log(cupWithCoin);
        coinTransform.position = randomCupCoin.position + new Vector3(0, 2, 0);
        coinTransform.gameObject.SetActive(true);
        StartCoroutine(LerpCoin());
    }

    IEnumerator LerpCoin()
    {
        yield return new WaitForSeconds(2.0f);
        coinTransform.GetComponent<FloatingObject>().enabled = false;
        float elapsedTimeCoin = 0.0f;
        float journeyTimeCoin = 0.5f;

        Vector3 firstPosCoin = coinTransform.position;

        float coinAlpha = coinSpriteRenderer.color.a;
        float newAlpha;

        while (elapsedTimeCoin < journeyTimeCoin)
        {
            
            elapsedTimeCoin += Time.deltaTime;
            float fracCompleteCoin = elapsedTimeCoin / journeyTimeCoin;

            coinTransform.position = Vector3.Lerp(firstPosCoin, firstPosCoin - new Vector3(0, 3, 0), fracCompleteCoin);
            newAlpha = Mathf.Lerp(coinAlpha, 0,fracCompleteCoin);
            coinSpriteRenderer.color = new Color(coinSpriteRenderer.color.r, coinSpriteRenderer.color.g, coinSpriteRenderer.color.b, newAlpha);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Sequencer());

    }

    public void SpawnArrow()
    {
        arrowTransform.gameObject.SetActive(true);
        arrowTransform.position = arrowList[0].position;
    }

    private void ArrowLeft()
    {
        iteratorArrow--;
        if (iteratorArrow < 0) iteratorArrow = 0;
        arrowTransform.position = arrowList[iteratorArrow].position;
    }

    private void ArrowRight()
    {
        
        iteratorArrow++;
        if (iteratorArrow > 2) iteratorArrow = 2;
        arrowTransform.position = arrowList[iteratorArrow].position;
        
    }

    private void CheckWin()
    {
        if(posList[iteratorArrow].position == cupWithCoin.position)
        {
            Gino.instance.player.life++;
            EndText.text = "True ! +1 up";
            EndText.gameObject.SetActive(true);

        }
        else
        {
            EndText.text = "Wrong !";
            EndText.gameObject.SetActive(true);
        }
        //StartCoroutine(WaitToEnd());

    }
    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(2);
        EndMiniGame();
    }
    private void EndMiniGame()
    {
        UIScoreManager.instance.Resume();
        MiniGame_Parent.SetActive(false);
    }

    private void OnEnable()
    {
        Gino.instance.player.PauseGame(true);
        ResetGame();
    }

    public void ResetGame()
    {
        EndText.gameObject.SetActive(false);
        elapsedTime = 0.0f;
        //for (int i = 0; i < 3; i++)
        //{
        //    cupList[i].position = posList[i].position;
        //    cupList[i].position = cupList[i].position + new Vector3(0, -1, 0);
        //    arrowList[i].position = posList[i].position;
        //    arrowList[i].position = arrowList[i].position + new Vector3(0, -2.5f, 0);
        //}
        CoinRandomizer();
    }
}
