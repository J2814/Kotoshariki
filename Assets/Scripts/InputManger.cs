using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManger : MonoBehaviour
{
    [SerializeField] private MyButton leftButton;
    [SerializeField] private MyButton rightButton;
    [SerializeField] private MyButton middleButton;

    public static Action MiddleButtonClick;

    public static Action<bool> LeftButtonDown;
    public static Action<bool> RightButtonDown;
    private void OnEnable()
    {
        middleButton.Click += middleClick;
    }

    private void OnDisable()
    {
        middleButton.Click -= middleClick;
    }

    private void middleClick()
    {
        MiddleButtonClick?.Invoke();
    }
    void Update()
    {
        LeftButtonDown?.Invoke(leftButton.IsDown);
        RightButtonDown?.Invoke(rightButton.IsDown);
    }
}
