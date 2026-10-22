using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int energyToLoose;
    [HideInInspector] public EnergyBar energyManager;
    [HideInInspector] public DaySystem daySystem;
    private int currentDay = 1;
    
    
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
    }

    private void Start()
    {
        StartCoroutine(daySystem.ShowCurrentDay(currentDay));
    }

    public void OperationEnded(int pToLoose)
    {
        energyToLoose = pToLoose;

        StartCoroutine(LoadScene("MetaGame"));
    }

    private IEnumerator LoadScene(string pSceneName)
    {
        AsyncOperation metaLoader = SceneManager.LoadSceneAsync(pSceneName);

        while (!metaLoader.isDone)
        {
            yield return null;
        }
    }

    public void OnEndDayOrEnergyFinish()
    {
        currentDay++;
        StartCoroutine(daySystem.ShowCurrentDay(currentDay));
    }
    
}
