using System.Collections;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    void Start()
    {
        PatientBloodLevel.onBloodDead += CorStart;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PatientBloodLevel.onBloodDead -= CorStart;
    }

    private void CorStart(PatientBloodLevel patientBloodLevel)
    {
        if (patientBloodLevel != GameManager.Instance.currentPatientBlood)
            return;
        gameObject.SetActive(true);
        StartCoroutine(OnDie(patientBloodLevel));
    } 

    private IEnumerator OnDie(PatientBloodLevel patientBloodLevel)
    {
        if (patientBloodLevel != GameManager.Instance.currentPatientBlood)
            yield break;
        
        group.alpha = 1;
        yield return new WaitForSeconds(3f);
    }
    
}
