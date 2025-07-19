using UnityEngine;

public struct MovementSnapshot
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public MovementSnapshot(Vector3 pos, Quaternion rot, Vector3 scl)
    {
        position = pos;
        rotation = rot;
        scale = scl;
    }
}
