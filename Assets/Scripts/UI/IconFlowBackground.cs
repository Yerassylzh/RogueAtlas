using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IconFlowBackground : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movePixelsPerSecond = 100f;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private float iconSize = 50f;
    [SerializeField] private int overflowPixels = 200;

    private List<RectTransform> iconTransforms = new List<RectTransform>();
    private List<Vector2> iconBasePositions = new List<Vector2>(); // Store original positions
    private Vector2 currentOffset;
    private int gridWidth;
    private int gridHeight;
    private float screenWidth;
    private float screenHeight;
    private float previousScreenWidth;
    private float previousScreenHeight;

    void Awake()
    {
        CalculateGridDimensions();
        // Store initial screen dimensions
        previousScreenWidth = screenWidth;
        previousScreenHeight = screenHeight;
        SpawnIcons();
        currentOffset = Vector2.zero;
    }

    void Update()
    {
        CheckForScreenSizeChange();
        MoveBg();
    }

    void CalculateGridDimensions()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        screenWidth = canvasRect.rect.width;
        screenHeight = canvasRect.rect.height;

        gridWidth = Mathf.CeilToInt((screenWidth + overflowPixels * 2) / iconSize) + 2;
        gridHeight = Mathf.CeilToInt((screenHeight + overflowPixels * 2) / iconSize) + 2;
    }

    void SpawnIcons()
    {
        float startX = -screenWidth / 2 - overflowPixels;
        float startY = screenHeight / 2 + overflowPixels;

        for (int row = 0; row < gridHeight; row++)
        {
            for (int col = 0; col < gridWidth; col++)
            {
                GameObject icon = Instantiate(iconPrefab, transform);
                RectTransform iconRect = icon.GetComponent<RectTransform>();

                iconRect.anchorMin = new Vector2(0.5f, 0.5f);
                iconRect.anchorMax = new Vector2(0.5f, 0.5f);
                iconRect.pivot = new Vector2(0.5f, 0.5f);

                iconRect.sizeDelta = new Vector2(iconSize, iconSize);

                float posX = startX + col * iconSize + iconSize / 2;
                float posY = startY - row * iconSize - iconSize / 2;

                Vector2 basePosition = new Vector2(posX, posY);
                iconRect.anchoredPosition = basePosition;

                iconTransforms.Add(iconRect);
                iconBasePositions.Add(basePosition);
            }
        }
    }

    void CheckForScreenSizeChange()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float currentScreenWidth = canvasRect.rect.width;
        float currentScreenHeight = canvasRect.rect.height;

        if (!Mathf.Approximately(currentScreenWidth, previousScreenWidth) ||
            !Mathf.Approximately(currentScreenHeight, previousScreenHeight))
        {
            RemoveAllIcons();
            CalculateGridDimensions();
            SpawnIcons();

            currentOffset = Vector2.zero;

            previousScreenWidth = screenWidth;
            previousScreenHeight = screenHeight;
        }
    }

    void RemoveAllIcons()
    {
        for (int i = 0; i < iconTransforms.Count; i++)
        {
            if (iconTransforms[i] != null)
            {
                DestroyImmediate(iconTransforms[i].gameObject);
            }
        }

        iconTransforms.Clear();
        iconBasePositions.Clear();
    }

    void MoveBg()
    {
        float moveStep = movePixelsPerSecond * Time.deltaTime;
        Vector2 movement = new Vector2(moveStep, -moveStep); // -45 degrees

        currentOffset += movement;

        if (currentOffset.x >= iconSize && Mathf.Abs(currentOffset.y) >= iconSize)
        {
            currentOffset.x -= iconSize;
            currentOffset.y += iconSize; // Add because we're moving in negative Y
        }

        // Apply movement to all icons using their base positions
        for (int i = 0; i < iconTransforms.Count; i++)
        {
            // basePosition: The icon's original "home" position in the grid
            Vector2 basePosition = iconBasePositions[i];

            // anchoredPosition: The final screen position = base position + movement offset
            iconTransforms[i].anchoredPosition = basePosition + currentOffset;
        }
    }
}