using System;
using UnityEngine;
using UnityEngine.UI;

public class MetaManager : MonoBehaviour
{
    public Timer timer;
    public DaySystem daySystem;
    public ChoicePatient choicePatient;
    [SerializeField] private Animation cameraAnim;
    

    public void OnClick(Button button)
    {
        cameraAnim.Play();
        button.interactable = false;
        choicePatient.OnClick();
    }
}
