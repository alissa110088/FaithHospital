using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Organs : MonoBehaviour
{
    protected Dictionary<State, Action> _anims = new Dictionary<State, Action>();
    protected Dictionary<State, Action<Vector2>> _animsStep = new Dictionary<State, Action<Vector2>>();

    public static int _countGrouped = 0;
    public bool groupped;
    public int nextLayer;
    [HideInInspector] public bool newOrgan;

    protected virtual void Start()
    {
        Controller.OnInputValidate += OnInputFinished;
        Controller.OnInputStepFinished += OnInputStepFinished;
    }

    private void OnEnable()
    {
        if (groupped)
            _countGrouped++;
    }

    protected virtual void OnInputFinished(bool pBool = false, State _state = null)
    {
        if (!_anims.ContainsKey(_state))
            return;

        if (_anims.TryGetValue(_state, out var effect))
        {
            effect.Invoke();
        }
    }

    public bool UpdateGrouped()
    {
        _countGrouped--;
        groupped = false;
        if (_countGrouped > 0)
        {
            return false;
        }

        return true;
    }

    protected virtual void OnInputStepFinished(State _state, Vector2 _position)
    {
        if (!_animsStep.ContainsKey(_state))
            Debug.LogError("State envoyer apres la fin de l'input inexistant");

        if (_animsStep.TryGetValue(_state, out var effect))
            effect.Invoke(_position);
    }

    private void OnDisable()
    {
        if(groupped)
            _countGrouped--;
    }
}