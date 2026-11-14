using UnityEngine;

public class PatientBloodLevel
{
    public float currentBloodLevel = 1;
    private float currentBlood = 1;
    private float divider = -1;
    
    public PatientBloodLevel()
    {
        currentBlood = GameManager.Instance.howLongPerPatient;
        divider = 1 / GameManager.Instance.howLongPerPatient;          
        Debug.Log(GameManager.Instance.howLongPerPatient);
    }
    
    public void UpdateBloodLevel(float deltaTime)
    {
        currentBlood -= deltaTime;
        currentBloodLevel = currentBlood * divider;
    }
}
