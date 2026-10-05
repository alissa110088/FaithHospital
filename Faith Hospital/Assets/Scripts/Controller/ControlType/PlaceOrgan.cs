using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceOrgan : State
{
    private GameObject _gameObjectToMove;
    private float _timer;
    private List<GameObject> placedObjects = new List<GameObject>();
    private bool stopLerp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Attrapable"))
        {
            _gameObjectToMove = other.gameObject;
            _timer = 0;
            StartCoroutine(lerpOrgan());
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 3)
        {
            _gameObjectToMove = null;
        }
    }

    private IEnumerator lerpOrgan()
    {
        while (true)
        {
            _gameObjectToMove.transform.position = Vector3.Lerp(_gameObjectToMove.transform.position,
                        _drawShape.points[0], Time.deltaTime * 10);
                    yield return new WaitForEndOfFrame();
            if(Vector3.Distance(_gameObjectToMove.transform.position, _drawShape.points[0]) < 1f)
                yield break;
        }
        
        
    }
}