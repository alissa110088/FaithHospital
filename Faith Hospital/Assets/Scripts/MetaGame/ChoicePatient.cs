using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePatient : MonoBehaviour
{
    private DayOrganPairing currentDayPairing;

    [SerializeField] private PatientContainer patientContainer;
    [SerializeField] private OperationDay operationPerDay;
    [SerializeField] private Transform transformParent;
    [SerializeField] private CanvasGroup canvasGroup;

    private List<PatientContainer> patientContainerInstance = new List<PatientContainer>();

    private void OnEnable()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnClick()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnExitClick()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePanel(DayOrganPairing pairing)
    {
        currentDayPairing = pairing;
        if (currentDayPairing == null)
            return;

        for (int i = 0; i < pairing.operationsSO.Count; i++)
        {
            PatientContainer current = Instantiate(patientContainer, transformParent);
            current.SetPicture(pairing.operationsSO[i].operationsImage);
            current.SetText(pairing.operationsSO[i].operationsText);
            current.LinkButtonOperation(PatientChosen);
            patientContainerInstance.Add(current);
        }
        
        
    }

    public void PatientChosen(PatientContainer patient)
    {
        int indexOp = patientContainerInstance.IndexOf(patient);
        string sceneName = MetaManager.remainingSurgery.operationsSO[indexOp].operationsSceneName;
        MetaManager.remainingSurgery.operationsSO.RemoveAt(indexOp);
        StartCoroutine(GameManager.Instance.LoadScene(sceneName));
    }
    
}