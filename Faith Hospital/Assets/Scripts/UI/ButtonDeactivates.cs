using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDeactivates : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private ControlState stateToSwitch;

    public void OnPointerDown(PointerEventData eventData)
    {
        Controller.Instance.SwitchState(stateToSwitch, this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Controller.Instance.SwitchState((ControlState.none));
    }
}