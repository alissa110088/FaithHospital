using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    

    public IEnumerator Open(int pAmount)
    {
        label.text = "End of surgery: Your patient lost: " + pAmount + "cl of blood";
        yield return new WaitForSeconds(3f);
    }
}
