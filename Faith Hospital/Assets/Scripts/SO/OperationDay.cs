using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "OperationDay", menuName = "Scriptable Objects/OperationDay")]
public class OperationDay : ScriptableObject
{
    public DayOrganPairing[] operationsSettings;
}

[Serializable]

public class DayOrganPairing
{
    public int day;
    public List<OperationsSO> operationsSO;

}

[Serializable]

public class OperationsSO  
{
    public string operationsSceneName;
    public Texture operationsImage;
    public string operationsText;
}
