using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PatientContainer : MonoBehaviour
{
    private TextMeshProUGUI textContainer;
    private RawImage image;
    private Button button;
    private void OnEnable()
    {
        textContainer = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponentInChildren<RawImage>();
        button = GetComponentInChildren<Button>();
    }

    public void LinkButtons(UnityAction<PatientContainer> pMyFunc)
    {
        button.onClick.AddListener(() => pMyFunc(this));
    }
    public void SetText(string text)
    {
        textContainer.text = text;
    }

    public void SetPicture(Texture sprite)
    {
        image.texture = sprite;
    }
}
