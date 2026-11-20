using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MetaManager : MonoBehaviour
{
    [SerializeField] private Animation cameraAnim;
    [SerializeField] private GameObject LooseScreen;
    public QuotaManager quotaManager;
    
    public DaySystem daySystem;
    public ChoicePatient choicePatient;
    private GameManager gameManager;
    private int maxDay;

    private Image imageChoice;
    private Button buttonChoice;
    private Vector3 camBase;
    private Quaternion camRotation;
    
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

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
    }
    
    public void OnClick(Button button)
    {
        cameraAnim.Play();
        camBase = Camera.main.transform.position;
        camRotation = Camera.main.transform.rotation;
        button.interactable = false;
        buttonChoice = button;
        imageChoice = button.GetComponent<Image>();
        imageChoice.raycastTarget = false;
        choicePatient.OnClick();
    }

    private void ResetCam()
    {
        if (buttonChoice == null)
            return;
        buttonChoice.interactable = true;
        imageChoice.raycastTarget = true;
        Camera.main.transform.position = camBase;
        Camera.main.transform.rotation = camRotation;
    }
    
    public IEnumerator ChangeDay(List<OperationsSO> remainingSurgery, List<PatientBloodLevel> patientBloodLevels)
    {
        if(maxDay == 0)
            maxDay = gameManager.operationDay.operationsSettings.Length;
        
        currentDay++;
        
        //TODO fin du jeu 
        if (currentDay -1 >= maxDay)
        {
            yield break;
        }
        
        remainingSurgery.Clear();
        patientBloodLevels.Clear();

        if (!quotaManager.QuotaMet())
        {
            yield return OnGameOver();
            GameManager.Instance.StartGame();
            yield break;
        }
        
        yield return StartCoroutine(daySystem.ShowCurrentDay(currentDay));
        
        OperationsSO patient = gameManager.operationDay.operationsSettings[currentDay - 1]
            .poolOperation[
                Random.Range(0, gameManager.operationDay.operationsSettings[currentDay - 1].poolOperation.Count - 1)];

        gameManager.operationDay.operationsSettings[currentDay - 1]
            .poolOperation.Remove(patient);
        remainingSurgery.Add(patient);
        
        PatientBloodLevel patientBloodLevel = new PatientBloodLevel();
        patientBloodLevels.Add(patientBloodLevel);

        GameManager.Instance.ChooseTime();
        choicePatient.ResetPanel();
        choicePatient.UpdatePanel(remainingSurgery);
        quotaManager.SetQuota( gameManager.operationDay.operationsSettings[currentDay - 1].quota);
        ResetCam();
        daySystem.FinishShowDay();
    }

    private IEnumerator OnGameOver()
    {
        LooseScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        LooseScreen.gameObject.SetActive(false);
    }
}
