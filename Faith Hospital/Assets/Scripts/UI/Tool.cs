using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private ControlState stateToSwitch;

    public static Tool activeTool;
    private Vector3 _originPos;

    private void Start()
    {
        _originPos = transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (activeTool != null)
        {
            Controller.Instance.inputManager.onTouchingPos -= activeTool.PlaceAtTouch;
            activeTool.transform.position = _originPos;
        }

        activeTool = this;
        Controller.Instance.SwitchState(stateToSwitch);

        transform.position = eventData.position;

        Controller.Instance.inputManager.onTouchingPos += PlaceAtTouch;
    }

    private void PlaceAtTouch(Vector2 pPos)
    {
        bool isDragging = Input.touchCount > 0 &&
                          (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary);
        if (!isDragging) return;

        
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