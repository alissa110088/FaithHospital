using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Organs : MonoBehaviour
{
    protected Dictionary<State, Action> _anims = new Dictionary<State, Action>();
    protected Dictionary<State, Action<Vector2>> _animsStep = new Dictionary<State, Action<Vector2>>();
    
    protected virtual void Start()
    {
        Controller.OnInputValidate += OnInputFinished;
        Controller.OnInputStepFinished += OnInputStepFinished;
    }
    
    protected virtual void OnInputFinished(State _state = null)
    {
        if(!_anims.ContainsKey(_state))
            Debug.LogError("State envoyer apres la fin de l'input inexistant");
        
        if (_anims.TryGetValue(_state, out var effect))
            effect.Invoke();
    }

    protected virtual void OnInputStepFinished(State _state, Vector2 _position)
    {
        if(!_animsStep.ContainsKey(_state))
            Debug.LogError("State envoyer apres la fin de l'input inexistant");
        
        if (_animsStep.TryGetValue(_state, out var effect))
            effect.Invoke(_position);
    }
}
