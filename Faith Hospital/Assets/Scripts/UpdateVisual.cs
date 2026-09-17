using DG.Tweening;
using UnityEngine;

public class UpdateVisual : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private GameObject _object2;
    
    private void Start()
    {
        Controller.OnInputValidate += InputValidated;
    }

    private void InputValidated()
    {
        _object.transform.DOMoveX(_object.transform.position.x - 0.3f, 0.5f);
        _object.transform.DOMoveY(_object.transform.position.y - 0.3f, 0.5f);
        
        _object2.transform.DOMoveX(_object2.transform.position.x + 0.3f, 0.5f);
        _object2.transform.DOMoveY(_object2.transform.position.y + 0.3f, 0.5f);
    }
}
