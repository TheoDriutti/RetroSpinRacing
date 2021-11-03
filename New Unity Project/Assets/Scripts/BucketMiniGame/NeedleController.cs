using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleController : MonoBehaviour
{
    public float gap;
    public float rotationSpeed;
    int delta;
    Rigidbody2D rb;

    enum State {Left, Right};
    State currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        delta = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentState = State.Right;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentState = State.Left;
        }

        switch (currentState)
        {
            case State.Left:
                MoveLeft();
                break;
            case State.Right:
                MoveRight();
                break;
            default:
                break;
        }

        rb.rotation = Mathf.Lerp(rb.rotation, gap * delta, rotationSpeed * Time.deltaTime);
    }

    private void MoveRight()
    {
        delta = -1;
    }
    private void MoveLeft()
    {
        delta = 1;
    }
}
