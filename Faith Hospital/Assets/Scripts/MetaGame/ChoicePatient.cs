using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class ChoicePatient : MonoBehaviour
{
    [SerializeField] private PatientContainer patientContainer;
    [SerializeField] private Transform transformParent;
    [SerializeField] private CanvasGroup canvasGroup;

    private List<PatientContainer> patientContainerInstance = new List<PatientContainer>();
    private List<OperationsSO> patientSo = new List<OperationsSO>();
    private Vector3 OriginalPos;
    private bool isOpen;
    
    private void OnEnable()
    {
        canvasGroup.blocksRaycasts = false;
        OriginalPos =  transform.position;
        PatientBloodLevel.onBloodDead += Close;
    }

    private void OnDisable()
    {
        PatientBloodLevel.onBloodDead -= Close;
    }

    public void OnClick()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnExitClick()
    {
        gameObject.SetActive(false);
    }
    
    public void UpdatePanel( List<OperationsSO> listPatient)
    {
     
        //check a rajouter
        for (int i = 0; i < listPatient.Count; i++)
        {
            
            if (patientSo.Contains(listPatient[i]))
                continue;
            
            PatientContainer current = Instantiate(patientContainer, transformParent);
            current.bloodLevel = GameManager.Instance.patientsBloodLevelList[i];    
            current.SetPicture(listPatient[i].operationsImage);
            current.SetText(listPatient[i].operationsText);
            current.SetTextCoin(listPatient[i].moneyWon);
            current.LinkButtonOperation(PatientChosen);
            patientContainerInstance.Add(current);
            patientSo.Add(listPatient[i]);
        }
        
        //check a enlever
        for (int i =  patientSo.Count-1; i >= 0; i--)
        {
            if (!listPatient.Contains(patientSo[i]))
            {
                Destroy(patientContainerInstance[i].gameObject);
                patientContainerInstance.RemoveAt(i);
                patientSo.RemoveAt(i);
            }           
        }
    }

    public void ResetPanel()
    {
        for (int i =  patientContainerInstance.Count -1; i >= 0; i--)
        {
            Destroy(patientContainerInstance[i].gameObject);
            patientContainerInstance.RemoveAt(i);
            patientSo.Remove(patientSo[i]);
        }
    }

    public void PatientChosen(PatientContainer patient)
    {
        int indexOp = patientContainerInstance.IndexOf(patient);
        GameManager.Instance.StartLevel(patientSo[indexOp]);
    }

    public void Open()
    {
        if (isOpen)
        {
            transform.DOMoveY( OriginalPos.y, 1f);
            canvasGroup.blocksRaycasts = false;
            isOpen = false;
        }
        else
        {
            isOpen = true;
            canvasGroup.blocksRaycasts = true;
            transform.DOMoveY( OriginalPos.y - 1700, 1f);
        }
        
    }

    private void Close(PatientBloodLevel patient)
    {
        if (patient != GameManager.Instance.currentPatientBlood)
            return;
        gameObject.SetActive(false);
    }
    
    public void Close(Button  button)
    {
        button.gameObject.SetActive(true);
        transform.DOMoveY( OriginalPos.y, 1f);
    }
}