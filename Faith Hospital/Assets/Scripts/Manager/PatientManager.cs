using System.Collections.Generic;
using UnityEngine;

public class PatientManager : MonoBehaviour
{
    private List<List<State>> _layers =  new List<List<State>>();
    [SerializeField] private int _currentLayer = 1;
    void Start()
    {
        State[] lObjects = FindObjectsByType<State>(FindObjectsSortMode.None);
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
            }
        }
        
        Controller.OnInputValidate += UnlockNextLayer;
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
    }

    private void UnlockNextLayer(State _state)
    {
        _currentLayer = _state._nextLayer; //TODO mettre dans drawshape??????!!?!?!
        UpdateLayers();
    }
}
