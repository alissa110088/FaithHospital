using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MetaManager : MonoBehaviour
{
    [SerializeField] private Animation cameraAnim;
    
    public DaySystem daySystem;
    public ChoicePatient choicePatient;
    private GameManager gameManager;
    private int maxDay;
    
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

    private void Start()
    {
        maxDay = gameManager.operationDay.operationsSettings[currentDay - 1].poolOperation.Count;
    }
    
    public void OnClick(Button button)
    {
        cameraAnim.Play();
        button.interactable = false;
        Image image = button.GetComponent<Image>();
        image.raycastTarget = false;
        choicePatient.OnClick();
    }
    
    public void ChangeDay(List<OperationsSO> remainingSurgery)
    {
        currentDay++;
        
        //TODO fin du jeu 
        if (currentDay >= maxDay)
            return;
        
        StartCoroutine(daySystem.ShowCurrentDay(currentDay));
        remainingSurgery.Clear();
        
        OperationsSO patient = gameManager.operationDay.operationsSettings[currentDay - 1]
            .poolOperation[
                Random.Range(0, gameManager.operationDay.operationsSettings[currentDay - 1].poolOperation.Count - 1)];

        gameManager.operationDay.operationsSettings[currentDay - 1]
            .poolOperation.Remove(patient);
        remainingSurgery.Add(patient);

        GameManager.Instance.ChooseTime();
        choicePatient.ResetPanel();
        choicePatient.UpdatePanel(remainingSurgery);
    }
    
    
}
