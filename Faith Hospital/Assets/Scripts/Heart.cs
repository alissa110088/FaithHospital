using DG.Tweening;
using UnityEngine;

public class Heart : Organs
{
    [SerializeField] private MeshRenderer _Artere1;
    [SerializeField] private MeshRenderer _Artere2;
    [SerializeField] private MeshRenderer _Artere3;
    
    [SerializeField] private DrawShape _Trace;
    [SerializeField] private DrawShape _Trace2;
    [SerializeField] private DrawShape _Trace3;
    [SerializeField] private DrawShape _Trace4;

    protected override void Start()
    {
        base.Start();
        if (_Trace && _Trace2 != null)
        {
            _anims[_Trace._state] = () => CutFirst(_Trace._state);
            _anims[_Trace2._state] = () => CutSecond(_Trace2._state);
        }
        else
        {
            _anims[_Trace3._state] = () => CloseFirst(_Trace3._state);
            _anims[_Trace4._state] = () => CloseSecond(_Trace4._state);
        }
    }
    
    private void CutFirst(State _state)
    {
        _Artere1.material.color = Color.red;
    }
    private void CutSecond(State _state)
    {
        _Artere2.material.color = Color.red;
        _Artere3.material.color = Color.red;
    }
    
    private void CloseFirst(State _state)
    {
        _Artere1.material.color = Color.gray;
    }
    private void CloseSecond(State _state)
    {
        _Artere2.material.color = Color.gray;
        _Artere3.material.color = Color.gray;
    }


}
