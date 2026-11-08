using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PatientContainer : MonoBehaviour
{
    private TextMeshProUGUI textContainer;
    [SerializeField] private RawImage imagePatients;
    [SerializeField] private Button[] buttonsPatientsChoice;
    [SerializeField] private Button buttonChoiceOperation;
    private CanvasGroup canvasGroup;
    private void OnEnable()
    {
        textContainer = GetComponentInChildren<TextMeshProUGUI>();
        imagePatients.SetNativeSize();
        GetComponent<Image>().raycastTarget = false;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void LinkButtonOperation(UnityAction pMyFunc)
    {
        buttonChoiceOperation.onClick.AddListener(() => pMyFunc());
    }

    public void LinkButtonsChoice(UnityAction<int> pMyFunc)
    {
        for (int i = 0; i < buttonsPatientsChoice.Length; i++)
        {
            int index = i;
            buttonsPatientsChoice[i].onClick.AddListener(() => pMyFunc(index));
        }
    }

    public void ActivateButton()
    {
        canvasGroup.blocksRaycasts = true;
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
