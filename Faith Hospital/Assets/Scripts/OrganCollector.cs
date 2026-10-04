using System;
using DG.Tweening;
using UnityEngine;

public class OrganCollector : MonoBehaviour
{
    public void Appear()
    {
        transform.DOMoveX(transform.position.x - 7.5f, 0.5f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Attrapable"))
        {
            Debug.Log("detetcted");
            NextLayerOrgan _nextLayer = other.gameObject.GetComponent<NextLayerOrgan>();
            Controller.OnInputValidateOrgan.Invoke(_nextLayer.nextLayer);
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            Close(other.transform);
        }
    }

    private void Close(Transform pOrgan)
    {
        transform.DOMoveX(transform.position.x + 7.5f, 0.5f);
        pOrgan.DOMoveX(transform.position.x + 7.5f, 0.5f);
    }

    public void OpenWithOrgan(GameObject pOrgan)
    {
        Rigidbody rb = pOrgan.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        transform.DOMoveX(transform.position.x + 7.5f, 0.5f);
        pOrgan.transform.DOMoveX(transform.position.x + 7.5f, 0.5f).OnComplete(() => { rb.isKinematic = false; });
    }
}