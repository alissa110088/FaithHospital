using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class OrganCollector : MonoBehaviour
{
    public bool isNew = false;

    public void Appear()
    {
        transform.DOMoveX(transform.position.x - 7.5f, 0.5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Attrapable") && isNew == false)
        {
            Debug.Log("detetcted " + other.gameObject.name);
            OrganPickUp _nextLayer = other.gameObject.GetComponent<OrganPickUp>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            StartCoroutine(Close(other.transform, _nextLayer));
        }
    }

    private IEnumerator Close(Transform pOrgan, OrganPickUp p)
    {
        transform.DOMoveX(transform.position.x + 7.5f, 0.5f);
        pOrgan.DOMoveX(transform.position.x + 7.5f, 0.5f);
        Destroy(pOrgan.gameObject);
        yield return new WaitForSeconds(0.5f);
        Controller.OnInputValidateOrgan.Invoke(p.nextLayer);
    }

    public void OpenWithOrgan(GameObject pOrgan)
    {
        Rigidbody rb = pOrgan.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        float targetX = transform.position.x - 7.5f;
        transform.DOMoveX(targetX, 0.5f);
        pOrgan.transform.DOMoveX(targetX, 0.5f).OnComplete(() => { rb.isKinematic = false; });
    }
}