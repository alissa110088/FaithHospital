using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDeactivates : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private ControlState stateToSwitch;

    private Coroutine holdCor;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (holdCor != null)
        {
            StopCoroutine(holdCor);
            holdCor = null;
        }
        holdCor = StartCoroutine(Hold(eventData));
    }

    private IEnumerator Hold(PointerEventData eventData)
    {
        while (true)
        {
            Controller.Instance.SwitchState(stateToSwitch, this.gameObject);
            transform.position = eventData.pressPosition;   
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (holdCor != null)
        {
            StopCoroutine(holdCor);
            holdCor = null;
        }
    }
}