using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int energyToLoose;
    public MetaManager metaManager;
    [SerializeField] private int minSecBetweenPatients;
    [SerializeField] private int maxSecBetweenPatients;
    [SerializeField] private OperationDay operationDaySource;

    [HideInInspector] public OperationDay operationDay;

    public static List<OperationsSO> remainingSurgery;

    private float timeBeforeNext;
    private static int currentDay = 0;
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
    }

    private void Start()
    {
        ChangeDay();
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

        if (timeBeforeNext == 0)
        {
            //lol
            OperationsSO patient = operationDay.operationsSettings[currentDay - 1]
                .poolOperation[
                    Random.Range(0, operationDay.operationsSettings[currentDay - 1].poolOperation.Count - 1)];

            operationDay.operationsSettings[currentDay - 1]
                .poolOperation.Remove(patient);
            remainingSurgery.Add(patient);

            ChooseTime();
            metaManager.choicePatient.UpdatePanel(remainingSurgery);
        }
    }

    private void ChangeDay()
    {
        currentDay++;
        Debug.Log(metaManager.daySystem);
        StartCoroutine(metaManager.daySystem.ShowCurrentDay(currentDay));
        remainingSurgery.Clear();
        
        OperationsSO patient = operationDay.operationsSettings[currentDay - 1]
            .poolOperation[
                Random.Range(0, operationDay.operationsSettings[currentDay - 1].poolOperation.Count - 1)];

        operationDay.operationsSettings[currentDay - 1]
            .poolOperation.Remove(patient);
        remainingSurgery.Add(patient);

        ChooseTime();
        metaManager.choicePatient.UpdatePanel(remainingSurgery);
    }

    private void ChooseTime()
    {
        timeBeforeNext = Random.Range(minSecBetweenPatients, maxSecBetweenPatients);
    }
}