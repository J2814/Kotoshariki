using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    private BallSpawner Spawner;
    private CircleCollider2D collider;
    private Rigidbody2D rb;

    public Color Color;

    [SerializeField]
    private SpriteRenderer bodySprite;

    internal float edgeOffset = 0;

    [SerializeField] private float mass = 1;

    public int Index;

    private bool released = false;

    public Action CombineSpawned;

    public Action Thrown;

    

    private void OnEnable()
    {
        BallSpawner.OnSpawnEvent += AssignSpanwer;
        
    }

    private void OnDisable()
    {
        BallSpawner.OnSpawnEvent -= AssignSpanwer;
    }
    private void Awake()
    {
        bodySprite.color = Color;
        AssignPhysicsStuff();
        edgeOffset = transform.localScale.x/2;

        collider.enabled = false;
        rb.isKinematic = true;
    }
    private void AssignSpanwer(BallSpawner ballSpawner)
    {
        if (Spawner == null) {
            Spawner = ballSpawner;
        }
    }
    public void Activate()
    {
        AssignPhysicsStuff();
        collider.enabled = true;
        rb.isKinematic = false;
        rb.mass = mass;
        released = true;
    }

    public void Deactivate()
    {
        rb.bodyType = RigidbodyType2D.Static;
        collider.enabled = false;
        //rb.velocity = Vector3.zero;
        //rb.rotation = 0;
    }

    private void AssignPhysicsStuff()
    {
        if (collider == null)
        {
            collider = GetComponent<CircleCollider2D>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            GameObject otherBall = collision.gameObject;
            if (otherBall.GetComponent<Ball>().Index == Index) 
            {
                int thisID = gameObject.GetInstanceID();
                int otherID = otherBall.gameObject.GetInstanceID();

                if (thisID > otherID)
                {
                    Spawner.CombineBalls(this, otherBall.GetComponent<Ball>());
                }
            }
        }
    }

    

}
