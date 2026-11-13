using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MetaManager : MonoBehaviour
{
    public DaySystem daySystem;
    public ChoicePatient choicePatient;
    [SerializeField] private Animation cameraAnim;
    public static int currentDay = 0;
    public static MetaManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)

        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void OnClick(Button button)
    {
        cameraAnim.Play();
        button.interactable = false;
        Image image = button.GetComponent<Image>();
        image.raycastTarget = false;
        choicePatient.OnClick();
    }
    
    public void ChangeDay(List<OperationsSO> remainingSurgery, OperationDay operationDay)
    {
        currentDay++;

        StartCoroutine(daySystem.ShowCurrentDay(currentDay));
        remainingSurgery.Clear();

        OperationsSO patient = operationDay.operationsSettings[currentDay - 1]
            .poolOperation[
                Random.Range(0, operationDay.operationsSettings[currentDay - 1].poolOperation.Count - 1)];

        operationDay.operationsSettings[currentDay - 1]
            .poolOperation.Remove(patient);
        remainingSurgery.Add(patient);

        GameManager.Instance.ChooseTime();
        choicePatient.ResetPanel();
        choicePatient.UpdatePanel(remainingSurgery);
    }
    
    
}
