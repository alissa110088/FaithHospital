using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerDownHandler
{
    public ToolsEnum _toolsEnum;

    public static Tool activeTool;
    private RawImage _icon;
    private Vector3 _originScale;
    private Vector3 _upScale;
    private float blinkSpeed = 1f;
    private Coroutine blinkCor;

    private bool isTurn;

    public bool IsTurn
    {
        get { return isTurn;}
        set
        {
            isTurn = value;
            if(value)
                StartBlinking();
            else if (blinkCor != null)
            {
                StopCoroutine(blinkCor);
                blinkCor = null;
            }
        }
    }
    
    private void Start()
    {
        _upScale = transform.localScale * 1.45f;
        _originScale = transform.localScale;
        _icon = GetComponent<RawImage>();
        PatientManager.Instance.AddTool(this, _toolsEnum);
    }       

    public void OnPointerDown(PointerEventData eventData)
    {
        if (activeTool != null)
        {
            activeTool._icon.raycastTarget = true;
            Controller.Instance.inputManager.onTouchingPos -= activeTool.PlaceAtTouch;
            activeTool.transform.DOScale(activeTool._originScale, 0.5f);
            if(activeTool.isTurn)
                activeTool.StartBlinking();
        }

        activeTool = this;

        if (isTurn)
        {
            if (blinkCor != null)
            {
                StopCoroutine(blinkCor);
                blinkCor = null;
            }

            _icon.DOKill();
        }
        
        Controller.Instance.SwitchState(_toolsEnum);
        _icon.raycastTarget = false;
        
        _icon.color = Color.white;
        transform.DOScale(_upScale, 0.5f);
    }

    private void PlaceAtTouch(Vector2 pPos)
    {
        if (float.IsInfinity(pPos.x) || float.IsInfinity(pPos.y))
            return;
        transform.position = pPos;
    }

    public void StartBlinking()
    {
        if(blinkCor != null)
            StopCoroutine(blinkCor);

        blinkCor = StartCoroutine(OnBlink());
    }
    private IEnumerator OnBlink()
    {
        while (true)
        {
            yield return _icon.DOColor(Color.white, blinkSpeed).WaitForCompletion();
            yield return _icon.DOColor(Color.red, blinkSpeed).WaitForCompletion();
        }
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