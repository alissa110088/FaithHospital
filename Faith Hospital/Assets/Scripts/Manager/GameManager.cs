using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MetaManager MetaManager;
    [HideInInspector] public int energyToLoose;
    [HideInInspector] public MetaManager metaManager;


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
    }

    public void OperationEnded(int pToLoose)
    {
        energyToLoose = pToLoose;

        StartCoroutine(LoadScene("MetaGame"));
    }

    public IEnumerator LoadScene(string pSceneName)
    {
        AsyncOperation metaLoader = SceneManager.LoadSceneAsync(pSceneName);

        while (!metaLoader.isDone)
        {
            yield return null;
        }
    }
    
}