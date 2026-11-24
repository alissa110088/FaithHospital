using DG.Tweening;
using UnityEngine;

public class ToolsOpening : MonoBehaviour
{
    private bool isOpen;
    private Vector3 originalPos;
    private Vector3 targetPos;

    private void Start()
    {
        originalPos = transform.position;
        targetPos = originalPos + new Vector3(1127, 0, 0);
    }

    public void Open()
    {
        if (isOpen)
        {
            isOpen = false;
            transform.DOMoveX(originalPos.x, 0.4f);
        }
        else
        {
            isOpen = true;
            transform.DOMoveX(targetPos.x, 0.4f);
        }
    }
}
