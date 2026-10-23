using System;
using UnityEngine;

public class MetaManager : MonoBehaviour
{
    [SerializeField] private EnergyBar energy;
    [SerializeField] private DaySystem daySystem;
    [SerializeField] private ChoicePatient choicePatient;
    [SerializeField] private OperationDay operationDaySource;
    [HideInInspector] public OperationDay operationDay; 
    private static int currentDay = 1;
    public static DayOrganPairing remainingSurgery;
    private static bool isFirst = true;

    private void Awake()
    {
        operationDay = Instantiate(operationDaySource);
    }

    private void Start()
    {
        if (isFirst)
        {
            remainingSurgery = operationDay.operationsSettings[0];
            isFirst = false;
            
        }
        CheckChangeDay();
    }

    private void CheckChangeDay()
    {
        if (energy.OperationEnded(GameManager.Instance.energyToLoose))
        {
            currentDay++;
            remainingSurgery = operationDay.operationsSettings[currentDay-1];
            StartCoroutine(daySystem.ShowCurrentDay(currentDay));
        }
        choicePatient.UpdatePanel(remainingSurgery);
        choicePatient.gameObject.SetActive(false);
    }
}
