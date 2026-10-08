using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrgansSO", menuName = "Scriptable Objects/OrgansSO")]
public class OrgansSO : ScriptableObject
{
    public OrganLayerPairing[] organLayerPairings;
}

[Serializable]
public class OrganLayerPairing
{
    public int layer;
    public int Nextlayer;
    public Heart heart;
}