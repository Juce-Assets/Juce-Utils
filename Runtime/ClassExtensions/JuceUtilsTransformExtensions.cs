using UnityEngine;

public static class JuceUtilsTransformExtensions
{
    public static void SetPositionX(this Transform transform, float pos)
    {
        Vector3 currPosition = transform.position;
        transform.position = new Vector3(pos, currPosition.y, currPosition.z);
    }

    public static void SetPositionY(this Transform transform, float pos)
    {
        Vector3 currPosition = transform.position;
        transform.position = new Vector3(currPosition.x, pos, currPosition.z);
    }

    public static void SetPositionZ(this Transform transform, float pos)
    {
        Vector3 currPosition = transform.position;
        transform.position = new Vector3(currPosition.x, currPosition.y, pos);
    }

    public static void SetLocalRotationX(this Transform transform, float angleDeg)
    {
        Vector3 currRotation = transform.localRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(angleDeg, currRotation.y, currRotation.z);
    }

    public static void SetLocalRotationY(this Transform transform, float angleDeg)
    {
        Vector3 currRotation = transform.localRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currRotation.x, angleDeg, currRotation.z);
    }

    public static void SetLocalRotationZ(this Transform transform, float angleDeg)
    {
        Vector3 currRotation = transform.localRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currRotation.x, currRotation.y, angleDeg);
    }
}