using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int energyToLoose;
    [SerializeField] private int minSecBetweenPatients;
    [SerializeField] private int maxSecBetweenPatients;
    public float howLongPerPatient;
    [SerializeField] private OperationDay operationDaySource;
    [SerializeField] private Timer timer;
    

    [HideInInspector] public OperationDay operationDay;
    [HideInInspector] public PatientBloodLevel currentPatientBlood;
    
    public static List<OperationsSO> remainingSurgery = new List<OperationsSO>();
    public List<PatientBloodLevel> patientsBloodLevelList = new List<PatientBloodLevel>();

    public Action OnTimerEnded;

    private float timeBeforeNext;
    private OperationsSO currentOperation;
    private MetaManager metaManager;

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
        metaManager.ChangeDay(remainingSurgery, patientsBloodLevelList);
        timer.StartTimer();
        PatientBloodLevel.onBloodDead += OnPatientDie;
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
        int index = remainingSurgery.IndexOf(operationDay);
        currentPatientBlood = patientsBloodLevelList[index];
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
            if (operationDay.operationsSettings[MetaManager.currentDay - 1].poolOperation.Count == 0 &&
                remainingSurgery.Count == 0)
            {
                timer.StartTimer();
                metaManager.ChangeDay(remainingSurgery, patientsBloodLevelList);
            }
                
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

    private void OnPatientDie(PatientBloodLevel patientBloodLevel)
    {
        int index = patientsBloodLevelList.IndexOf(patientBloodLevel);
        patientsBloodLevelList.Remove(patientBloodLevel);
        remainingSurgery.RemoveAt(index);
        if (SceneManager.GetActiveScene().name == "MetaGame")
        {
            metaManager.choicePatient.UpdatePanel(remainingSurgery);
            if (operationDay.operationsSettings[MetaManager.currentDay - 1].poolOperation.Count == 0 &&
                remainingSurgery.Count == 0)
            {
                timer.StartTimer();
                metaManager.ChangeDay(remainingSurgery, patientsBloodLevelList);
            }
                
        }
            
        else
            StartCoroutine(OnDieInOp());
    }

    private IEnumerator OnDieInOp()
    {
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(LoadScene("MetaGame"));
    }

    private void OnTimerDone()
    {
        if (SceneManager.GetActiveScene().name == "MetaGame")
        {
            timer.StartTimer();
            metaManager.ChangeDay(remainingSurgery, patientsBloodLevelList);
        }
        else
            StartCoroutine(WaitForEndLoad());
    }

    private IEnumerator WaitForEndLoad()
    {
        yield return LoadScene("MetaGame");
        metaManager = MetaManager.Instance;
        metaManager.ChangeDay(remainingSurgery, patientsBloodLevelList);
        timer.StartTimer();
    }

    public void ChooseTime()
    {
        if (operationDay.operationsSettings[MetaManager.currentDay - 1].poolOperation.Count > 0)
            timeBeforeNext = Random.Range(minSecBetweenPatients, maxSecBetweenPatients);
        else
            timeBeforeNext = Mathf.Infinity;
    }

    private void OnDestroy()
    {
        OnTimerEnded -= OnTimerDone;
        PatientBloodLevel.onBloodDead -= OnPatientDie;
    }
}