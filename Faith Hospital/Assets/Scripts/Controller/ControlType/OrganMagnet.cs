using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrganMagnet: MonoBehaviour
{
    [SerializeField] private string _organeName;
    private GameObject _gameObjectToMove;
    private Coroutine _lerpRoutine;
    private bool taken = false;
    private Collider _collider;
    private Rigidbody _rb;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Attrapable")|| other.gameObject.name != _organeName || taken) return;
        if (_lerpRoutine != null) return; 

        _gameObjectToMove = other.gameObject;

        if (_gameObjectToMove.TryGetComponent(out _rb))
        {
            _rb.linearVelocity = Vector3.zero; 
            _rb.useGravity = false;
        }

        taken = true;
        _lerpRoutine = StartCoroutine(LerpOrgan());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _gameObjectToMove)
        {
            Release();
        }
    }

    private IEnumerator LerpOrgan()
    {
        Vector3 target = new Vector3(transform.position.x
            , transform.position.y,transform.position.z);
        Transform t = _gameObjectToMove.transform;

        if (_gameObjectToMove == null)
        {
            taken = false;
            _gameObjectToMove = null;
        }
        
        while (Vector3.Distance(t.position, target) > 0.01f)
        {
            t.position = Vector3.MoveTowards(t.position, target, 10f * Time.deltaTime);
            yield return null;
        }
        if (_gameObjectToMove.TryGetComponent(out Organs organ))
        {
            if (organ.newOrgan)
            {
                Controller.OnInputValidateOrgan.Invoke(organ.nextLayer);
                organ.newOrgan = false;
            }
        }
        
        t.position = target;
        _lerpRoutine = null;
    }
    
    private void Release()
    {
        if (_lerpRoutine != null) StopCoroutine(_lerpRoutine);
        if (_rb != null) _rb.useGravity = true;
        _gameObjectToMove = null;
        taken = false;
        _rb = null;
        _lerpRoutine = null;
    }
}