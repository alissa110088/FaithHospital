using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class OrganCollector : MonoBehaviour
{
    public bool isNew = false;
    public bool isOpen;

    public void Appear()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        transform.DOMoveX(transform.position.x - 7.5f, 0.5f);
    }

    public void Close()
    {
        if (!isOpen)
            return;
        transform.DOMoveX(transform.position.x + 7.5f, 0.5f).OnComplete(() =>
        {
            isOpen = false;
        });
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Attrapable") && isNew == false)
        {
            if (!isOpen)
                return;
            Heart _nextLayer = other.gameObject.GetComponent<Heart>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            StartCoroutine(Close(other.transform, _nextLayer));
        }
    }

    private IEnumerator Close(Transform pOrgan, Heart p)
    {
        if (!isOpen)
            yield break;
        isOpen = false;
        transform.DOMoveX(transform.position.x + 7.5f, 0.5f);
        pOrgan.DOMoveX(transform.position.x + 7.5f, 0.5f);
        Destroy(pOrgan.gameObject);
        yield return new WaitForSeconds(0.5f);
        Controller.OnInputValidateOrgan.Invoke(p.nextLayer);
    }

    public void OpenWithOrgan(GameObject pOrgan)
    {
        if (isOpen)
            return;
        isOpen = true;
        Rigidbody rb = pOrgan.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        float targetX = transform.position.x - 7.5f;
        transform.DOMoveX(targetX, 0.5f);
        pOrgan.transform.DOMoveX(targetX, 0.5f).OnComplete(() => { rb.isKinematic = false; });
    }
}