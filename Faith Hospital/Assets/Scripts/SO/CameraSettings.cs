using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable Objects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public Vector3 Position;
    public float fieldOfView = 60f;
    public int layer;
}
