using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // Note the time at the start of the animation.
        elapsedTime = 0.0f;
        //StartCoroutine(Sequencer());
        CoinRandomizer();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            SetMovement(ref cup1, ref cup3);
            //SetMovement(ref cup3, ref cup1);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            SetMovement(ref cup3, ref cup2);
           // SetMovement(ref cup2, ref cup3);
        }*/
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
        Transform randomCup1 = cupList[Random.Range(0, 2)];
        Transform randomCup2 = cupList[Random.Range(0, 2)];

        while (randomCup1.position == randomCup2.position)
        {
            randomCup2 = cupList[Random.Range(0, 2)];
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
            yield return null;
        }
    }

    public void SetCoin()
    {

    }

    public void CoinRandomizer()
    {
        Transform randomCupCoin = cupList[Random.Range(0, 2)];
        coinTransform.position = randomCupCoin.position + new Vector3(0, 10, 0);
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

            coinTransform.position = Vector3.Lerp(firstPosCoin, firstPosCoin - new Vector3(0, 5, 0), fracCompleteCoin);
            newAlpha = Mathf.Lerp(coinAlpha, 0,fracCompleteCoin);
            Debug.Log(newAlpha);
            coinSpriteRenderer.color = new Color(coinSpriteRenderer.color.r, coinSpriteRenderer.color.g, coinSpriteRenderer.color.b, newAlpha);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Sequencer());

    }
}
