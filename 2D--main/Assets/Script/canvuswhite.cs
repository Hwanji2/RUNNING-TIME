using UnityEngine;

public class EnablePixelPerfect : MonoBehaviour
{
    public Canvas canvas;

    void Start()
    {
        canvas.pixelPerfect = true;
    }
}
