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
        Debug.Log("Here");
        _canvasGroup.alpha = pPourcentage;
    }

    private void Disable(bool pBool, State pState)
    {
        _affliction.SetActive(false);
    }
}
