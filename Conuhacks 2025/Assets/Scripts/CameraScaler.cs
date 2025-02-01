using Unity.VisualScripting;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float targetWidth = 1920;
    public float targetHeight = 1080;
    public float pixelsPerUnit = 16;

    void Start() {
        Camera.main.orthographicSize = (targetHeight / 2f) / pixelsPerUnit;
        AdjustForAspectRatio();
    }

    void AdjustForAspectRatio() {
        float targetAspect = targetWidth / targetHeight;
        float screenAspect = (float)Screen.width / Screen.height;
        if (screenAspect < targetAspect) {
            Camera.main.orthographicSize = (targetHeight / 2f / pixelsPerUnit) * (targetAspect / screenAspect);
        }
    }
}
