using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : ButtonView
{
    [SerializeField]
    private Button _closeBtn;

    public void SubscribeClose(Action close)
    {
        _closeBtn.onClick.AddListener(() => close?.Invoke());
    }

}
