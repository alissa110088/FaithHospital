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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Attrapable")|| other.gameObject.name != _organeName || taken) return;
        if (_lerpRoutine != null) return; 

        _gameObjectToMove = other.gameObject;

        if (_gameObjectToMove.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero; 
            rb.useGravity = false;
        }

        taken = true;
        _lerpRoutine = StartCoroutine(LerpOrgan());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _gameObjectToMove)
        {
            taken = false;
        }
    }

    private IEnumerator LerpOrgan()
    {
        Vector3 target = new Vector3(transform.position.x
            , transform.position.y,transform.position.z);
        Transform t = _gameObjectToMove.transform;

        while (Vector3.Distance(t.position, target) > 0.01f)
        {
            t.position = Vector3.MoveTowards(t.position, target, 10f * Time.deltaTime);
            yield return null;
        }
        Debug.Log("huh");
        if (_gameObjectToMove.TryGetComponent(out Organs organ))
        {
            Debug.Log(organ.newOrgan);
            if (organ.newOrgan)
            {
                Debug.Log("huh");
                Controller.OnInputValidateOrgan.Invoke(organ.nextLayer);
                organ.newOrgan = false;
            }
        }
        
        t.position = target;
        _lerpRoutine = null;
    }
}