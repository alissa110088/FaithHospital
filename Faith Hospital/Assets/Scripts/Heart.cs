using DG.Tweening;
using UnityEngine;

public class Heart : Organs
{
    public int nextLayer;
    [HideInInspector]  public bool newOrgan;
    
    [SerializeField] private MeshRenderer _Artere1;
    [SerializeField] private MeshRenderer _Artere2;
    [SerializeField] private MeshRenderer _Artere3;
    
    [SerializeField] private DrawShape _Trace;
    [SerializeField] private DrawShape _Trace2;
    
    protected override void Start()
    {
        base.Start();
        _anims[_Trace._state] = () => CutFirst(_Trace._state);
        _anims[_Trace2._state] = () => CutSecond(_Trace2._state);
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


}
