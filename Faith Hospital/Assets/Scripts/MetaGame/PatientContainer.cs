using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PatientContainer : MonoBehaviour
{
    private TextMeshProUGUI textContainer;
    [SerializeField] private RawImage imagePatients;
    [SerializeField] private Button buttonChoiceOperation;
    private void OnEnable()
    {
        textContainer = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void LinkButtonOperation(UnityAction<PatientContainer> pMyFunc)
    {
        buttonChoiceOperation.onClick.AddListener(() => pMyFunc(this));
    }
    public void SetText(string text)
    {
        textContainer.text = text;
    }

    public void SetPicture(Texture sprite)
    {
        imagePatients.texture = sprite;
    }
}
