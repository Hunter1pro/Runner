using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private TMP_Text text;

    private Color initColor;
    private Color initTextColor;

    public void Init()
    {
        this.initColor = this.button.image.color;
        this.initTextColor = this.text.color;
    }

    public void Subscribe(Action action)
    {
        this.button.onClick.RemoveAllListeners();
        this.button.onClick.AddListener(() => action?.Invoke());
    }

    public void EnableButton(bool value)
    {
        this.button.enabled = value;
    }

    public void SetInteractable(bool value)
    {
        this.button.interactable = value;
    }

    public void SetHightLight(Color color)
    {
        this.button.image.color = color;
    }

    public void SetTextColor(Color color)
    {
        this.text.color = color;
    }

    public void ResetColors()
    {
        this.button.image.color = this.initColor;
        this.text.color = this.initTextColor;
    }

    public void SetText(string value)
    {
        this.text.text = value;
    }
}
