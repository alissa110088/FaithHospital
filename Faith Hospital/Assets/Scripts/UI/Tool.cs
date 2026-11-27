using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tool : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private TrailRenderer trail;
    [SerializeField] private ParticleSystem hitImpact;
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
            if (value && activeTool != this)
            {
                StartBlinking();
            }
                
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
            Controller.Instance.inputManager.onToucheStart -= activeTool.PlayFxAtTouch;
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
        Controller.Instance.inputManager.onTouchingPos += PlaceAtTouch;
        Controller.Instance.inputManager.onToucheStart += PlayFxAtTouch;
        Controller.Instance.inputManager.onToucheEnd += OnFingerUp;
    }

    public void OnFingerUp()
    {
        if (trail != null && trail.emitting)
        {
            trail.emitting = false;
            trail.Clear();
        }
            
    }
    
    private void PlaceAtTouch(Vector2 pPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new  Vector3(pPos.x, pPos.y, 5f));

        if (float.IsInfinity(worldPos.x) || float.IsInfinity(worldPos.y) || trail == null)
            return;
        
        trail.transform.position =  worldPos;

        if (!trail.emitting)
        {
            trail.Clear();
            trail.startColor = Color.clear;
            trail.emitting = true;      
        }
    }

    private void PlayFxAtTouch()
    {
        
        Vector2 pos = Controller.Instance.inputManager.action.Touch.PrimaryPosition.ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new  Vector3(pos.x, pos.y, 5f));
        if (float.IsInfinity(worldPos.x) || float.IsInfinity(worldPos.y) || hitImpact == null)
            return;
        
        if(hitImpact.isPlaying)
            hitImpact.Stop();
        hitImpact.transform.position = worldPos;
        hitImpact.Play();
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