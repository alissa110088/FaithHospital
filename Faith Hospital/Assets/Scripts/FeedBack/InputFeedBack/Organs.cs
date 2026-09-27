using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Organs : MonoBehaviour
{
    protected Dictionary<State, Action> _anims = new Dictionary<State, Action>();
    
    protected virtual void Start()
    {
        Controller.OnInputValidate += OnInputFinished;
    }
    
    protected virtual void OnInputFinished(State _state)
    {
        if(!_anims.ContainsKey(_state))
            Debug.LogError("State envoyer apres la fin de l'input inexistant");
        
        if (_anims.TryGetValue(_state, out var effect))
            effect.Invoke();
    }
}
