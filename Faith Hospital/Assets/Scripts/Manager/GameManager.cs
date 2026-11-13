using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int energyToLoose;
    private MetaManager metaManager;
    [SerializeField] private int minSecBetweenPatients;
    [SerializeField] private int maxSecBetweenPatients;
    [SerializeField] private OperationDay operationDaySource;
    [SerializeField] private Timer timer;

    [HideInInspector] public OperationDay operationDay;

    public static List<OperationsSO> remainingSurgery = new List<OperationsSO>();

    public Action OnTimerEnded;

    private float timeBeforeNext;
    private OperationsSO currentOperation;


    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)

        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        operationDay = Instantiate(operationDaySource);
        OnTimerEnded += OnTimerDone;
    }
    
    private void Start()
    {
        metaManager = MetaManager.Instance;
        metaManager.ChangeDay(remainingSurgery, operationDay);
        timer.StartTimer();
    }

    public IEnumerator OperationEnded(int pToLoose)
    {
        energyToLoose = pToLoose;
        remainingSurgery.Remove(currentOperation);
        yield return StartCoroutine(LoadScene("MetaGame"));
        metaManager.choicePatient.UpdatePanel(remainingSurgery);
    }

    public void StartLevel(OperationsSO operationDay)
    {
        currentOperation = operationDay; 
        StartCoroutine(LoadScene(operationDay.operationsSceneName));
    }

    private IEnumerator LoadScene(string pSceneName)
    {
        AsyncOperation metaLoader = SceneManager.LoadSceneAsync(pSceneName);

        while (!metaLoader.isDone)
        {
            yield return null;
        }
    }

    private void Update()
    {
        if (timeBeforeNext >= 0)
        {
            timeBeforeNext -= Time.deltaTime;
        }

        if (timeBeforeNext <= 0)
        {
            //lol
             OperationsSO patient = operationDay.operationsSettings[MetaManager.currentDay - 1]
                .poolOperation[
                    Random.Range(0, operationDay.operationsSettings[MetaManager.currentDay - 1].poolOperation.Count - 1)];
            
             
            operationDay.operationsSettings[MetaManager.currentDay - 1]
                .poolOperation.Remove(patient);
            remainingSurgery.Add(patient);
            metaManager.choicePatient.UpdatePanel(remainingSurgery);
            ChooseTime();
            
        }
    }

    private void OnTimerDone()
    {
        if (SceneManager.GetActiveScene().name == "MetaGame")
        {
            metaManager.ChangeDay(remainingSurgery, operationDay);
        }
        else
            StartCoroutine(WaitForEndLoad());
    }

    private IEnumerator WaitForEndLoad()
    {
        yield return LoadScene("MetaGame");
        metaManager = MetaManager.Instance;
        metaManager.ChangeDay(remainingSurgery, operationDay);
        timer.StartTimer();
    }

    public void ChooseTime()
    {
        if (operationDay.operationsSettings[MetaManager.currentDay - 1].poolOperation.Count > 0)
            timeBeforeNext = Random.Range(minSecBetweenPatients, maxSecBetweenPatients);
        else
            timeBeforeNext = Mathf.Infinity;
    }
}