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

    private PatientContainer currentActive;
    private List<PatientContainer> patientContainerInstance = new List<PatientContainer>();

    public void OnClick()
    {
        gameObject.SetActive(true);
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
            PatientContainer current = Instantiate(patientContainer, transform);
            current.SetPicture(pairing.operationsSO[i].operationsImage);
            current.SetText(pairing.operationsSO[i].operationsText);
            current.LinkButtonOperation(PatientChosen);
            current.LinkButtonsChoice(ChangePatient);
            patientContainerInstance.Add(current);
            current.gameObject.SetActive(false);
        }
        
        currentActive = patientContainerInstance[0];
        
        currentActive.gameObject.SetActive(true);
    }

    public void PatientChosen()
    {
        int indexOp = patientContainerInstance.IndexOf(currentActive);
        string sceneName = MetaManager.remainingSurgery.operationsSO[indexOp].operationsSceneName;
        MetaManager.remainingSurgery.operationsSO.RemoveAt(indexOp);
        StartCoroutine(GameManager.Instance.LoadScene(sceneName));
    }
    
    public void ChangePatient(int pIndex)
    {
        if (pIndex > patientContainerInstance.Count - 1)
            return;
        currentActive.gameObject.SetActive(false);
        currentActive =  patientContainerInstance[pIndex];
        currentActive.gameObject.SetActive(true);
    }
}