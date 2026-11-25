using UnityEngine;

public class SwipeAffliction : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _affliction;

    private void Start()
    {
        Controller.OnInputStepFinished += Erase;
        Controller.OnInputValidate += Disable;
    }

    private void Erase(State pState, Vector2 pVector, float pPourcentage)
    {
        if (pPourcentage > _canvasGroup.alpha)
            return;
        
        _canvasGroup.alpha = pPourcentage;
        if (_canvasGroup.alpha == 0f)
        {
            Controller.OnInputValidate(false, pState);
        }
    }

    private void Disable(bool pBool, State pState)
    {
        _affliction.SetActive(false);
        _canvasGroup.alpha = 1f;
    }
}
