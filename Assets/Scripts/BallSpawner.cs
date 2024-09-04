using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallSpawner : MonoBehaviour, IGameStateResonder
{
    public Transform NextBallPlace;

    public List<GameObject> BallPrefabs;
    [SerializeField]
    private Ball CurrentBall = null;

    private GameObject NextBall;

    [SerializeField]
    private int maxIndexToSpawn;
    [SerializeField]
    private float randomOffset = 0.02f;

    private int nextSpawnIndex = 0;

    private int spawnIndex = 0;

    [SerializeField] 
    private float movingSpeed = 1;

    

    public Transform BallPit;

    public float LeftEdge;
    public float RightEdge;

    public static Action<BallSpawner> OnSpawnEvent;

    public static Action<int> OnCombineEvent;

    private bool gameover;

    private bool moveRight;
    private bool moveLeft;
    public enum Move
    {
        Right, Left, None
    }
    public Move CurrentMovement = Move.None;

    private void Start()
    {
        Init();
        //nextSpawnIndex = UnityEngine.Random.Range(0, maxIndexToSpawn);
        //LaunchBall();
    }
    private void OnEnable()
    {
        InputManger.LeftButtonDown += leftButton;
        InputManger.RightButtonDown += rightButton;
        InputManger.MiddleButtonClick += LaunchBall;
        GameStateManager.GameRestart += Init;
        GameStateManager.OnGameStateChange += RespondToGameState;
    }

    private void OnDisable()
    {
        InputManger.LeftButtonDown -= leftButton;
        InputManger.RightButtonDown -= rightButton;
        InputManger.MiddleButtonClick -= LaunchBall;
        GameStateManager.GameRestart -= Init;
        GameStateManager.OnGameStateChange -= RespondToGameState;
    }
    private void Update()
    {
        if (!gameover)
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                LaunchBall();
            }
            if (moveRight || Input.GetKey(KeyCode.RightArrow))
            {
                CurrentMovement = Move.Right;
            }
            if (moveLeft || Input.GetKey(KeyCode.LeftArrow))
            {
                CurrentMovement = Move.Left;
            }
            if (!moveRight && !moveLeft && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                CurrentMovement = Move.None;
            }

            Movement();
        }
    }

    private void rightButton(bool move)
    {
        moveRight = move;
    }
    private void leftButton(bool move)
    {
        moveLeft = move;
    }

    private void Init()
    {
        gameover = false;
        foreach (Transform t in BallPit)
        {
           Destroy(t.gameObject);
        }

        if (CurrentBall != null)
        {
            Destroy(CurrentBall.gameObject);
        }

        if (NextBall != null)
        {
            Destroy(NextBall.gameObject);
        }
        nextSpawnIndex = UnityEngine.Random.Range(0, maxIndexToSpawn);
        LaunchBall();
    }
    public void Movement()
    {
        void move(Vector2 dir)
        {
            transform.position = Vector2.MoveTowards(transform.position, dir, Time.deltaTime * movingSpeed);
        }
        switch (CurrentMovement)
        {
            case Move.Left:  
               
                Vector2 vecl = new Vector2(LeftEdge + CurrentBall.edgeOffset, transform.position.y);
                move(vecl);
                break; 

            case Move.Right:
                
                Vector2 vecr = new Vector2(RightEdge - CurrentBall.edgeOffset, transform.position.y);
                move(vecr);
                break; 

            case Move.None: 

                if(transform.position.x < LeftEdge + CurrentBall.edgeOffset)
                {
                    Vector2 vecl1 = new Vector2(LeftEdge + CurrentBall.edgeOffset, transform.position.y);
                    move(vecl1);
                }
                if (transform.position.x > RightEdge - CurrentBall.edgeOffset)
                {
                    Vector2 vecr1 = new Vector2(RightEdge - CurrentBall.edgeOffset, transform.position.y);
                    move(vecr1);
                }
                break;

        }
    }
    public void LaunchBall()
    {
        ReleaseBall();

        RandomizeIndecies();

        SpawnCurrentBall();

        AssignNextBall();
    }

    private void ReleaseBall()
    {
        if (CurrentBall != null)
        {
            Vector3 slightlyRandomPos = transform.position + new Vector3(UnityEngine.Random.Range(-randomOffset, randomOffset), 0, 0);
            CurrentBall.transform.position = slightlyRandomPos;
            CurrentBall.transform.parent = BallPit;
            CurrentBall.Activate();
            CurrentBall.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
            CurrentBall.Thrown?.Invoke();
        }
    }
    private void RandomizeIndecies()
    {
        spawnIndex = nextSpawnIndex;
        nextSpawnIndex = UnityEngine.Random.Range(0, maxIndexToSpawn);
    }
    private void SpawnCurrentBall()
    {
        CurrentBall = SpawnBall(BallPrefabs[spawnIndex], this.transform);
    }
    private void AssignNextBall()
    {
        if (NextBall != null)
        {
            Destroy(NextBall);
        }
        NextBall = Instantiate(BallPrefabs[nextSpawnIndex], NextBallPlace.position, Quaternion.identity, NextBallPlace);
    }
    

    public void CombineBalls(Ball ball1, Ball ball2)
    {
        int newIndex = ball1.Index + 1;

        OnCombineEvent?.Invoke(newIndex);

        if (newIndex < BallPrefabs.Count)
        {
            Vector3 midPos = (ball2.transform.position - ball1.transform.position) / 2 + ball1.transform.position;
            Destroy(ball1.gameObject);
            Destroy(ball2.gameObject);
            StartCoroutine(SpawnNewBallAfterDelay(newIndex, midPos, BallPit));
        }
        else
        {
            Destroy(ball1.gameObject);
            Destroy(ball2.gameObject);
        }
    }

    IEnumerator SpawnNewBallAfterDelay(int index, Vector3 pos, Transform parent)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Ball newBall = SpawnBall(BallPrefabs[index], pos, Quaternion.identity);
        newBall.transform.parent = BallPit;
        newBall.Activate();
        newBall.CombineSpawned();
        newBall.tag = "Activated";

        
    }

    private Ball SpawnBall(GameObject ballObject, Vector3 pos, Quaternion rot)
    {
        Ball ball = Instantiate(ballObject, pos, rot).GetComponent<Ball>();
        OnSpawnEvent?.Invoke(this);
        return ball;
    }

    private Ball SpawnBall(GameObject ballObject, Transform parent)
    {
        Ball ball = Instantiate(ballObject, parent).GetComponent<Ball>();
        ball.transform.position = parent.position;
        OnSpawnEvent?.Invoke(this);
        return ball;
    }

    public void RespondToGameState(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Gameover)
        {
            gameover = true;

            foreach (Transform t in BallPit)
            {
                if (t.gameObject.GetComponent<Ball>() != null)
                {
                    Ball ball = t.gameObject.GetComponent<Ball>();
                    ball.Deactivate();
                }
                
            }

        }
    }
    
}
