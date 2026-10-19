using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private ToolsEnum _toolsEnum;

    public static Tool activeTool;
    private Vector3 _originPos;
    private RawImage _icon;
    private Vector3 _originScale;
    
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
            activeTool.transform.DOScale(activeTool._originScale, 0.5f);
        }

        activeTool = this;
        Controller.Instance.SwitchState(_toolsEnum);
        _icon.raycastTarget = false;
        _originScale = transform.localScale;
        
        transform.DOScale(transform.localScale * 1.45f, 0.5f);

        // if (_toolsEnum != ToolsEnum.none)
        // {
        //     transform.position = eventData.position;
        //     Controller.Instance.inputManager.onTouchingPos += PlaceAtTouch;
        // }

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