using UnityEngine;

public static class JuceUtilsCameraExtensions
{
    public static float GetLeftSideWorldPosition(this Camera cam)
    {
        return cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).x;
    }

    public static float GetRightSideWorldPosition(this Camera cam)
    {
        return cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, cam.nearClipPlane)).x;
    }

    public static float GetBottomSideWorldPosition(this Camera cam)
    {
        return cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)).y;
    }

    public static float GetTopSideWorldPosition(this Camera cam)
    {
        return cam.ScreenToWorldPoint(new Vector3(0, Screen.height, cam.nearClipPlane)).y;
    }

    public static Rect GetWorldPositionRect(this Camera cam)
    {
        Vector2 min = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 max = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));

        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }
}