using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Skin : Organs
{
    [SerializeField] private GameObject _skin2;
    [SerializeField] private GameObject _skin1;
    [SerializeField] private GameObject[] _visualAid;
    
    [SerializeField] private SwipeFollow _swipeFollow;
    [SerializeField] private Trace _trace;

    protected override void Start()
    {
        base.Start();
        _anims[_swipeFollow] = () => CutSkin(_swipeFollow);
        _anims[_trace] = () => OpenSkin(_trace);
    }
    
    private void CutSkin(State _state)
    {
        foreach (GameObject _point in _visualAid)
        {
            _point.SetActive(false);
        }


        _skin2.transform.DOMoveX(_skin2.transform.position.x - 0.13f, 0.2f);

        _skin1.transform.DOMoveX(_skin1.transform.position.x + 0.13f, 0.2f);

        _state.enabled = false;
    }

    private void OpenSkin(State _state)
    {
        foreach (GameObject _point in _visualAid)
        {
            _point.SetActive(false);
        }


        _skin2.transform.DOMoveX(_skin2.transform.position.x - 0.3f, 0.5f);

        _skin1.transform.DOMoveX(_skin1.transform.position.x + 0.3f, 0.5f);

        _state.enabled = false;
    }
    
}