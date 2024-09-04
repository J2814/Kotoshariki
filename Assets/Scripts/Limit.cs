using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour
{
    [SerializeField]
    private float slackTime;
    private float tempTime = 100;
    private Rigidbody2D rb;

    private bool armed;
    [SerializeField]
    private int ActivatedBallsOutOfLimitCount;

    public static Action<float> StartCountDown;
    public static Action StopCountDown;

    private bool isBusted = false;
    
    private void OnEnable()
    {
        GameStateManager.GameRestart += Init;
        StartCountDown += CountDownStart;
        StopCountDown += CountDownStop;
    }
    private void OnDisable()
    {
        GameStateManager.GameRestart -= Init;
        StartCountDown -= CountDownStart;
        StopCountDown -= CountDownStop;

    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (armed)
        {
            tempTime -= Time.deltaTime;
            if (tempTime <= 0) 
            {
                Busted();
            }
        }
    }

    private void CountDownStart(float time)
    {
        if (!armed)
        {
            armed = true;
            tempTime = time;
        }
    }

    private void CountDownStop()
    {
        armed = false;
    }

    private void Init()
    {
        armed = false;
        isBusted = false;
        tempTime = 100;
    }

    private void Busted()
    {
        isBusted = true;
        armed = false; tempTime = 100; 
        GameStateManager.ChangeGameState(GameStateManager.GameState.Gameover);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>())
        {
            if (collision.gameObject.tag != "Activated")
            {
                collision.gameObject.tag = "Activated";
            }

            ActivatedBallsOutOfLimitCount++;
            if (!armed)
            {
                StartCountDown?.Invoke(slackTime);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Ball>())
        {
            
            if (collision.gameObject.tag == "Activated")
            {
                ActivatedBallsOutOfLimitCount--;
                if (ActivatedBallsOutOfLimitCount <= 0) 
                {
                    ActivatedBallsOutOfLimitCount = 0;
                    StopCountDown?.Invoke();
                }
                
                
            }
        }
    }
}
