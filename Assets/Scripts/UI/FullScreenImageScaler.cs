using System;
using UnityEngine;

public class FullScreenImageScaler : MonoBehaviour
{
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float worldHeight = Camera.main.orthographicSize * 2.0f;
        float worldWidth = worldHeight * Camera.main.aspect;

        float width = FromPixelsToWorldUnits(rectTransform.rect.width);
        float height = FromPixelsToWorldUnits(rectTransform.rect.height);

        Debug.Log(worldWidth + " " + worldHeight + "\n" + width + " " + height);

        float scaleX = worldWidth / width;
        float scaleY = worldHeight / height;

        rectTransform.localScale = new Vector2(Math.Max(scaleX, scaleY), Math.Max(scaleX, scaleY));
    }

    float FromPixelsToWorldUnits(float pixel)
    {
        float worldHeight = Camera.main.orthographicSize * 2.0f;
        float pixelHeight = Screen.height;
        float pixels2World = worldHeight / pixelHeight;
        return pixel * pixels2World;
    }

}

