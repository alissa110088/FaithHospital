using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int energyToLoose;
    [HideInInspector] public MetaManager metaManager;
    [SerializeField] private int minSecBetweenPatients;
    [SerializeField] private int maxSecBetweenPatients;
    [SerializeField] private OperationDay operationDaySource;

    [HideInInspector] public OperationDay operationDay;

    public static List<OperationsSO> remainingSurgery;

    private float timeBeforeNext;
    private static int currentDay = 1;


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
        // remainingSurgery = operationDay.operationsSettings[0];
        // CheckChangeDay();
    }

    public void OperationEnded(int pToLoose)
    {
        energyToLoose = pToLoose;

        StartCoroutine(LoadScene("MetaGame"));
    }

    public void StartLevel(OperationsSO operationDay)
    {
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

            SpawnPatient();
            // metaManager.choicePatient.UpdatePanel(patient);
        }
    }

    private void ChangeDay()
    {
        currentDay++;
        // remainingSurgery = operationDay.operationsSettings[currentDay-1];
        StartCoroutine(metaManager.daySystem.ShowCurrentDay(currentDay));

        // metaManager.choicePatient.UpdatePanel(remainingSurgery);
        // choicePatient.gameObject.SetActive(false);
    }

    private void SpawnPatient()
    {
        timeBeforeNext = Random.Range(minSecBetweenPatients, maxSecBetweenPatients);
    }
}