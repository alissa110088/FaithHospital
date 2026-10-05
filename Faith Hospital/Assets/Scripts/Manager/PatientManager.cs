using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    private List<List<State>> _layers =  new List<List<State>>();
    public int _currentLayer = 1;

    [SerializeField] private OrgansSO _organsToPut;
    [SerializeField] private OrganCollector _organCollector;
    public static PatientManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            return;
        }

        Instance = this;
    }
    void Start()
    {
        State[] lObjects = FindObjectsByType<State>(FindObjectsSortMode.None);
        _layers.Add(new List<State>());
        _layers.Add(new List<State>());
        _layers.Add(new List<State>());
        _layers.Add(new List<State>());
        _layers.Add(new List<State>());
        _layers.Add(new List<State>());
        _layers.Add(new List<State>());
        _layers.Add(new List<State>());
        foreach (State lObject in lObjects)
        {
            switch (lObject.tag)
            {
               case "Layer1":
                   _layers[0].Add(lObject);
                   break;
               case "Layer2":
                   _layers[1].Add(lObject);
                   break;
               case "Layer3":
                   _layers[2].Add(lObject);
                   break;
               case "Layer4":
                   _layers[3].Add(lObject);
                   break;
               case "Layer5":
                   _layers[4].Add(lObject);
                   break;
               case "Layer6":
                   _layers[5].Add(lObject);
                   break;
               case "layer7":
                   _layers[6].Add(lObject);
                   break;
            }
        }
        UpdateLayers();
        
        Controller.OnInputValidate += UnlockNextLayer;
        Controller.OnInputValidateOrgan += UnlockNextLayer;
    }

    private void UpdateLayers()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            foreach (State lObject in _layers[i])
            {
                lObject.enabled = i == _currentLayer -1;
            }
        }

        foreach (OrganLayerPairing organs in _organsToPut.organLayerPairings)
        {
            if (organs.layer == _currentLayer)
            {
                Vector3 startPos = new Vector3(_organCollector.transform.position.x, _organCollector.transform.position.y, _organCollector.transform.position.z - 2f);
                GameObject organ = Instantiate(organs.organ, startPos, Quaternion.identity);
                organ.tag = "Layer" + _currentLayer;
                _organCollector.isNew = true;
                _organCollector.OpenWithOrgan(organ);
                return;
            }
        }

        _organCollector.isNew = false;
    }

    private void UnlockNextLayer(State _state)
    {
        _currentLayer = _state._drawShape._nextLayer; 
        UpdateLayers();
    }
    
    private void UnlockNextLayer(int pNextLayer)
    {
        _currentLayer = pNextLayer; 
        UpdateLayers();
    }
}
