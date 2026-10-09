using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class PatientManager : MonoBehaviour
{
    public List<List<State>> _layers = new List<List<State>>();
    public int _currentLayer = 1;

    [SerializeField] private OrgansSO _organsToPut;
    [SerializeField] private OrganCollector _organCollector;
    [SerializeField] private int _layerCount = 7;

    private OrganLayerPairing currentLayerPairing;
    private int _numToInstantiate;
    public static PatientManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < _layerCount; i++)
            _layers.Add(new List<State>());
    }

    void Start()
    {
        UpdateLayers();

        Controller.OnInputValidate += UnlockNextLayer;
        Controller.OnInputValidateOrgan += UnlockNextLayer;
    }

    public void AddLayer(int pIndex, State pState)
    {
        if (pIndex < 0 || pIndex >= _layers.Count) return;
        if (_layers[pIndex].Contains(pState)) return;

        _layers[pIndex].Add(pState);
        pState.enabled = pIndex == _currentLayer - 1;
    }

    private void UpdateLayers()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            foreach (State lObject in _layers[i])
            {
                if (lObject == null)
                    continue;
                lObject.enabled = i == _currentLayer - 1;
            }
        }

        foreach (OrganLayerPairing organs in _organsToPut.organLayerPairings)
        {
            if (organs.layer == _currentLayer)
            {
                currentLayerPairing = organs;
                _numToInstantiate = organs.num;
                _organCollector.InstantiateOrgan(organs.organ, organs.nameOrgan, organs.Nextlayer);
                return;
            }
        }

        _organCollector.isNew = false;
    }

    private void UnlockNextLayer(bool pIsGrouped, State _state = null)
    {
        if (pIsGrouped)
            return;

        currentLayerPairing = null;
        _currentLayer = _state._drawShape._nextLayer;
        UpdateLayers();
    }

    private void UnlockNextLayer(int pNextLayer)
    {
        if (currentLayerPairing != null)
            Debug.Log(currentLayerPairing.num);
        if (currentLayerPairing != null && _numToInstantiate > 0)
        {
            _numToInstantiate--;
            _organCollector.InstantiateOrgan(currentLayerPairing.organ, currentLayerPairing.nameOrgan,
                currentLayerPairing.Nextlayer);
            return;
        }
        
        currentLayerPairing = null;
        _currentLayer = pNextLayer;
        UpdateLayers();
    }
}