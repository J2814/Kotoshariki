using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownIndicator : MonoBehaviour
{
    private Image image;

    private float val = 0;

    bool armed;

    private List<Tweener> tweeners = new List<Tweener>(); 
    private void OnEnable()
    {
        Limit.StartCountDown += CountDown;
        Limit.StopCountDown += Reset;
    }

    private void OnDisable()
    {
        Limit.StartCountDown -= CountDown;
        Limit.StopCountDown -= Reset;
    }
    void Start()
    {
        DOTween.Init();
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.fillAmount = val;
    }
    private void CountDown(float time)
    {
        Tweener tweener = DOTween.To(() => val, x => val = x, 1, time).SetEase(Ease.Linear);
        tweeners.Add(tweener);
    }
    private void Reset()
    {
        foreach (Tweener t in tweeners)
        {
            t.Kill();
        }
        val=0;
    }
}
