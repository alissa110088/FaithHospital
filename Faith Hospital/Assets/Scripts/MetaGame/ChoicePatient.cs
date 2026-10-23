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
    [SerializeField] private Transform layerGroup;
    
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

        Debug.Log(pairing.operationsSO.Count);
        for (int i = 0; i < pairing.operationsSO.Count; i++)
        {
            PatientContainer current = Instantiate(patientContainer, layerGroup);
            current.SetPicture(pairing.operationsSO[i].operationsImage);
            current.SetText(pairing.operationsSO[i].operationsText);
            current.LinkButtons(PatientChosen);
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