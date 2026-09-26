using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private ControlState stateToSwitch;

    public static Tool activeTool;
    public Vector3 _originPos;
    private RawImage _icon;
    
    private void Start()
    {
        _originPos = transform.position;
        _icon = GetComponent<RawImage>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (activeTool != null)
        {
            activeTool._icon.raycastTarget = true;
            Controller.Instance.inputManager.onTouchingPos -= activeTool.PlaceAtTouch;
            activeTool.transform.position = activeTool._originPos;
        }

        activeTool = this;
        Controller.Instance.SwitchState(stateToSwitch);
        _icon.raycastTarget = false;
        
        if (stateToSwitch != ControlState.none)
        {
            transform.position = eventData.position;
            Controller.Instance.inputManager.onTouchingPos += PlaceAtTouch;
        }
            
    }

    private void PlaceAtTouch(Vector2 pPos)
    {
        if (float.IsInfinity(pPos.x) || float.IsInfinity(pPos.y))
            return;
        transform.position = pPos;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
    }

    private void OnDestroy()
    {
        if (activeTool == this)
        {
            Controller.Instance.inputManager.onTouchingPos -= PlaceAtTouch;
            activeTool = null;
        }
    }
}