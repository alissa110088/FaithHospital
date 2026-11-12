using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePatient : MonoBehaviour
{
    [SerializeField] private PatientContainer patientContainer;
    [SerializeField] private OperationDay operationPerDay;
    [SerializeField] private Transform transformParent;
    [SerializeField] private CanvasGroup canvasGroup;

    private List<PatientContainer> patientContainerInstance = new List<PatientContainer>();
    private List<OperationsSO> patientSo = new List<OperationsSO>();
    
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
    

    //quand je dois instancier un nouveau patient, appeler dans le game manager?
    public void UpdatePanel( List<OperationsSO> listPatient)
    {
        for (int i = 0; i < listPatient.Count; i++)
        {
            if (patientSo.Contains(listPatient[i]))
                return;
            
            PatientContainer current = Instantiate(patientContainer, transformParent);
            current.SetPicture(listPatient[i].operationsImage);
            current.SetText(listPatient[i].operationsText);
            current.LinkButtonOperation(PatientChosen);
            patientContainerInstance.Add(current);
            patientSo.Add(listPatient[i]);
        }
    }

    public void PatientChosen(PatientContainer patient)
    {
        int indexOp = patientContainerInstance.IndexOf(patient);
        GameManager.Instance.StartLevel(patientSo[indexOp]);
    }
}