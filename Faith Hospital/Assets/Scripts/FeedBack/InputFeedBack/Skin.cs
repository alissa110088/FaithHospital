using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Skin : Organs
{
    [SerializeField] private GameObject _skin2;
    [SerializeField] private GameObject _skin1;
    
    [SerializeField] private DrawShape _swipeFollow;
    [SerializeField] private DrawShape _Close;
    [SerializeField] private DrawShape _trace;
    [SerializeField] private DrawShape _Stitch;

    protected override void Start()
    {
        base.Start();
        _anims[_swipeFollow._state] = () => CutSkin(_swipeFollow._state);
        _anims[_trace._state] = () => OpenSkin(_trace._state);
        _anims[_Close._state] = () => CloseSkin(_Close._state);
        _anims[_Stitch._state] = () => StitchSkin(_Stitch._state);
    }
    
    private void CutSkin(State _state)
    {
        _skin2.transform.DOMoveX(_skin2.transform.position.x - 0.13f, 0.2f);

        _skin1.transform.DOMoveX(_skin1.transform.position.x + 0.13f, 0.2f);

        _state.enabled = false;
    }
    
    private void StitchSkin(State _state)
    {
        _skin2.transform.DOMoveX(_skin2.transform.position.x + 0.13f, 0.2f);

        _skin1.transform.DOMoveX(_skin1.transform.position.x - 0.13f, 0.2f);

        _state.enabled = false;
    }

    private void OpenSkin(State _state)
    {
        _skin2.transform.DOMoveX(_skin2.transform.position.x - 3f, 0.5f);

        _skin1.transform.DOMoveX(_skin1.transform.position.x + 3f, 0.5f);

        _state.enabled = false;
    }
    
    private void CloseSkin(State _state)
    {
        _skin2.transform.DOMoveX(_skin2.transform.position.x + 3f, 0.5f);

        _skin1.transform.DOMoveX(_skin1.transform.position.x - 3f, 0.5f);

        _state.enabled = false;
    }
}