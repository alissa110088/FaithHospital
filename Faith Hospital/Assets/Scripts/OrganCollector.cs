using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class OrganCollector : MonoBehaviour
{
    [SerializeField] private Transform[] anchors;
    
    [HideInInspector] public bool isNew = false;
    [HideInInspector] public bool isOpen;

    public void Appear()
    {
        if (isOpen)
        {
            return;
        }

        isOpen = true;
        transform.DOMoveX(anchors[0].position.x, 0.5f);
    }

    public void Close()
    {
        if (!isOpen)
            return;
        transform.DOMoveX(anchors[1].position.x, 0.5f).OnComplete(() =>
        {
            isOpen = false;
        });
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Attrapable") && !isNew)
        {
            if (!isOpen)
                return;
            Organs _nextLayer = other.gameObject.GetComponent<Organs>();
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            StartCoroutine(Close(other.transform, _nextLayer));
        }
    }

    private IEnumerator Close(Transform pOrgan, Organs p)
    {
        if (!isOpen)
            yield break;
        isOpen = false;
        transform.DOMoveX(anchors[1].position.x, 0.5f);
        pOrgan.DOMoveX(anchors[1].position.x, 0.5f);
        
        yield return new WaitForSeconds(0.5f);
        
        if (p.groupped)
        {
            if (p.UpdateGrouped())
            {
                Controller.OnInputValidateOrgan.Invoke(p.nextLayer);
            };
        }
        else
        {
            Controller.OnInputValidateOrgan.Invoke(p.nextLayer);
        }
            
        Destroy(pOrgan.gameObject);
    }

    public void InstantiateOrgan(Organs pOrgan, string pName, int pNextLayer)
    {
        isNew = true;
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2f);
        Organs organ = Instantiate(pOrgan, startPos, pOrgan.transform.rotation);
              
        organ.newOrgan = true;
        organ.gameObject.name = pName;  
        organ.newOrgan = true;
        organ.tag = "Layer" + PatientManager.Instance._currentLayer;
        organ.nextLayer = pNextLayer;
        OpenWithOrgan(organ.gameObject);
    }
    
    public void OpenWithOrgan(GameObject pOrgan)
    {
        if (isOpen)
            return;
        isOpen = true;
        
        Rigidbody rb = pOrgan.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        transform.DOMoveX(anchors[0].position.x, 0.5f);
        pOrgan.transform.DOMoveX(anchors[0].position.x, 0.5f).OnComplete(() => { rb.isKinematic = false; });
    }
}