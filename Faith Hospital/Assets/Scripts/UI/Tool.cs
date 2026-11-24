using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private ToolsEnum _toolsEnum;

    public static Tool activeTool;
    private RawImage _icon;
    private Vector3 _originScale;
    private Vector3 _upScale;
    
    private void Start()
    {
        _upScale = transform.localScale * 1.45f;
        _originScale = transform.localScale;
        _icon = GetComponent<RawImage>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (activeTool != null)
        {
            activeTool._icon.raycastTarget = true;
            Controller.Instance.inputManager.onTouchingPos -= activeTool.PlaceAtTouch;
            activeTool.transform.DOScale(activeTool._originScale, 0.5f);
        }

        activeTool = this;
        Controller.Instance.SwitchState(_toolsEnum);
        _icon.raycastTarget = false;
        
        transform.DOScale(_upScale, 0.5f);
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