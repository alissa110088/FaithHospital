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
    public float howLongPerPatient;
    [SerializeField] private OperationDay operationDaySource;
    [SerializeField] private Timer timer;
    

    [HideInInspector] public OperationDay operationDay;

    public static List<OperationsSO> remainingSurgery = new List<OperationsSO>();
    public List<PatientBloodLevel> patientsBloodLevelList = new List<PatientBloodLevel>();

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
        metaManager.ChangeDay(remainingSurgery);
        timer.StartTimer();
    }

    public void OpEnded()
    {
        StartCoroutine(OperationEnded());
    }

    private IEnumerator OperationEnded()
    {
        remainingSurgery.Remove(currentOperation);
        yield return StartCoroutine(LoadScene("MetaGame"));
        yield return new WaitForSeconds(0.3f);
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

        if (pSceneName == "MetaGame")
        {
            metaManager = MetaManager.Instance;
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
            Debug.Log("new patient added");
            
            PatientBloodLevel patientBloodLevel = new PatientBloodLevel();
            patientsBloodLevelList.Add(patientBloodLevel);
            
            if(SceneManager.GetActiveScene().name == "MetaGame")
                metaManager.choicePatient.UpdatePanel(remainingSurgery);
            ChooseTime();
        }

        for (int i = 0; i < patientsBloodLevelList.Count; i++)
        {
            patientsBloodLevelList[i].UpdateBloodLevel(Time.deltaTime);
        }
    }

    private void OnTimerDone()
    {
        if (SceneManager.GetActiveScene().name == "MetaGame")
        {
            timer.StartTimer();
            metaManager.ChangeDay(remainingSurgery);
        }
        else
            StartCoroutine(WaitForEndLoad());
    }

    private IEnumerator WaitForEndLoad()
    {
        yield return LoadScene("MetaGame");
        metaManager = MetaManager.Instance;
        metaManager.ChangeDay(remainingSurgery);
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