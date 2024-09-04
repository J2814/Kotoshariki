using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnimator : MyAnimator
{
    public List<string> AngryAnims;
    public List<string> IdleAnims;
    public List<string> BoredAnims;
    public List<string> BumpAnims;

    //private string currentAnimation;
    private void Awake()
    {
        SetUpAnimator();
    }
    public override void SetUpAnimator()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationExit";

            clip.AddEvent(animationEndEvent);
        }
    }

    int RandElement(List<string> list)
    {
        if (list.Count > 0)
        {
            return Random.Range(0, list.Count - 1);
        }
        else
        {
            return -1;
        }
    }

    public void PlayBumpAnim()
    {
        if (RandElement(BumpAnims) >= 0)
        {
            ChangeAnimationState(BumpAnims[RandElement(BumpAnims)]);
        }
    }

    public void PlayAngryAnim()
    {
        if (RandElement(AngryAnims) >= 0)
        {
            ChangeAnimationState(AngryAnims[RandElement(AngryAnims)]);
        }
    }

    public void PlayBoredAnim()
    {
        if (RandElement(BoredAnims) >= 0)
        {
            ChangeAnimationState(BoredAnims[RandElement(BoredAnims)]);
        }
    }

    public void PlayIdleAnim()
    {
        if (RandElement(IdleAnims) >= 0)
        {
            ChangeAnimationState(IdleAnims[RandElement(IdleAnims)]);
        }
    }

    public void AnimationExit()
    {
        PlayIdleAnim();
    }
}
