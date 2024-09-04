using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


public class BallAnimator : MonoBehaviour, IGameStateResonder
{
    private Ball ball;
    private AudioSource audioSource;

    private FaceAnimator faceAnimator;

    private Rigidbody2D rb;

    public GameObject CombineParticles;

    public Transform Body;

    private float oldVelocity;
    public float BumpThreshold = 2.25f;

    public Vector2 BoredomMinMaxSeconds = new Vector2(10, 20);

    private bool allowBump = true;

    private bool allowScaleTween = true;

    private List<Tweener> tweeners = new List<Tweener>();

    private bool gameover = false;

    private void OnEnable()
    {
        StartCoroutine(GettingBoredRoutine());
        GameStateManager.OnGameStateChange += RespondToGameState;
    }
    private void OnDisable()
    {
        GameStateManager.OnGameStateChange -= RespondToGameState;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        faceAnimator = GetComponentInChildren<FaceAnimator>();
        ball = GetComponent<Ball>();
        ball.Thrown += Throw;
        ball.CombineSpawned += CombineSpawn;

    }
    void Start()
    {
        DOTween.Init();
        oldVelocity = rb.velocity.sqrMagnitude;
        BumpThreshold *= BumpThreshold;   // So it works with sqrMagnitude
        
    }

    void FixedUpdate()
    {
        if (oldVelocity - rb.velocity.sqrMagnitude > BumpThreshold)
        {
            Bump();
        }
        oldVelocity = rb.velocity.sqrMagnitude;
    }

    private void Bump()
    {
        if (allowBump) 
        {
            Sound sound = AudioManager.instance.RandomSoundFromList(AudioManager.instance.SoundBank.Bump);
            AudioManager.instance.PlaySound(sound);
            faceAnimator.PlayBumpAnim();
            StartCoroutine(BumpCooldownRoutine());

            if (allowScaleTween)
            {
                allowScaleTween = false;
                Body.DOPunchScale(new Vector3(.1f, .1f, .1f), 0.2f).onComplete = ScaleTweenDone;
            }
                
        }
    }

    private void CombineSpawn() 
    {
        Sound sound = AudioManager.instance.RandomSoundFromList(AudioManager.instance.SoundBank.Angry);
        AudioManager.instance.PlaySound(sound);
        sound = AudioManager.instance.RandomSoundFromList(AudioManager.instance.SoundBank.Bump);
        AudioManager.instance.PlaySound(sound);
        faceAnimator.PlayAngryAnim();
        SpawnParticles(CombineParticles, ball.transform);

        if (allowScaleTween)
        {
            allowScaleTween = false;
            Body.DOPunchScale(new Vector3(.2f, .2f, .2f), 0.2f).onComplete = ScaleTweenDone;
        }
        
    }

    private void Bored()
    {
        if (gameover) { return; }

        int chance = UnityEngine.Random.Range(0, 2);

        if (chance < 1)
        {
            Sound sound = AudioManager.instance.RandomSoundFromList(AudioManager.instance.SoundBank.IdleExtra);
            AudioManager.instance.PlaySound(sound);
            faceAnimator.PlayBoredAnim();
        }
    }

    private void Throw()
    {
        Sound sound = AudioManager.instance.RandomSoundFromList(AudioManager.instance.SoundBank.Throw);
        AudioManager.instance.PlaySound(sound);
        faceAnimator.PlayBumpAnim();
    }

    private void SpawnParticles(GameObject prefab, Transform ballT)
    {
        GameObject p = Instantiate(prefab, ballT.position, Quaternion.identity, ballT.parent);
        p.transform.localScale = ballT.localScale;
        PaintParticles(p);
    }
    private void PaintParticles(GameObject particles)
    {
        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();
        
        foreach (ParticleSystem p in particleSystems) 
        {
            p.GetComponent<Renderer>().material.color = ball.Color;
        }
        //ball.Color;
    }

    private void ScaleTweenDone()
    {
        allowScaleTween = true;
    }

    public void RespondToGameState(GameStateManager.GameState state)
    {
        if (state == GameStateManager.GameState.Gameover)
        {
            gameover = true;
        }
    }
    IEnumerator BumpCooldownRoutine()
    {
        allowBump = false;
        yield return new WaitForSecondsRealtime(0.1f);
        allowBump = true;
    }
    IEnumerator GettingBoredRoutine()
    {
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(BoredomMinMaxSeconds.x, BoredomMinMaxSeconds.y));
        if (!gameover)
        {
            Bored();
        }
        StartCoroutine(GettingBoredRoutine());
    }
}
