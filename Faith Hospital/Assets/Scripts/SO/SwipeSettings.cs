using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Scriptable Objects/SwipeSettings")]
public class SwipeSettings : InputSettings
{
    public float howFarFromSegmentMax;
    public float DirectionRange;
}
