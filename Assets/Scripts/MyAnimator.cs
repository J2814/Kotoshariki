using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyAnimator : MonoBehaviour
{
    protected Animator animator;
    protected string currentAnimation;
    public virtual void SetUpAnimator()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
