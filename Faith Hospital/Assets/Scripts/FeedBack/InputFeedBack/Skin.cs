using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Skin : Organs
{
    [SerializeField] private GameObject _closed;
    [SerializeField] private GameObject _opened;
    
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
        _closed.SetActive(false);

        _opened.SetActive(true);

        _state.enabled = false;
    }
    
    private void StitchSkin(State _state)
    {
        _closed.transform.DOMoveX(_closed.transform.position.x + 0.13f, 0.2f);

        _opened.transform.DOMoveX(_opened.transform.position.x - 0.13f, 0.2f);

        _state.enabled = false;
    }

    private void OpenSkin(State _state)
    {
        _closed.transform.DOMoveX(_closed.transform.position.x - 3f, 0.5f);

        _opened.transform.DOMoveX(_opened.transform.position.x + 3f, 0.5f);

        _state.enabled = false;
    }
    
    private void CloseSkin(State _state)
    {
        _closed.transform.DOMoveX(_closed.transform.position.x + 3f, 0.5f);

        _opened.transform.DOMoveX(_opened.transform.position.x - 3f, 0.5f);

        _state.enabled = false;
    }
}