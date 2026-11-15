using System;
using UnityEngine;

public class PatientBloodLevel
{
    public float currentBloodLevel = 1;
    private float currentBlood = 1;
    private float divider = -1;
    private float failLoss = 0.05f;
    public static Action<PatientBloodLevel> onBloodDead;
    
    public PatientBloodLevel()
    {
        currentBlood = GameManager.Instance.howLongPerPatient;
        divider = 1 / GameManager.Instance.howLongPerPatient;          
    }

    public void OnFail()
    {
        currentBloodLevel -= failLoss;
    }
    
    public void UpdateBloodLevel(float deltaTime)
    {
        currentBlood -= deltaTime;
        currentBloodLevel = currentBlood * divider;
        if (currentBloodLevel <= 0)
        {
            onBloodDead.Invoke(this);
        }
    }
}
