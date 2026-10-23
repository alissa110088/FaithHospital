using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DaySystem : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI dayText;

    private void Start()
    {
        dayText = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public IEnumerator ShowCurrentDay(int pDay)
    {
        dayText.text = $"Day {pDay}";
        canvasGroup.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        canvasGroup.DOFade(0, 1);
    }
}
