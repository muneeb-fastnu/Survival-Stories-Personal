using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewResourcePos : MonoBehaviour
{
    public Texture2D[] baseTextures; // Reference to base textures for different levels
    public Color[] spawnableColors; // Colors corresponding to areas where objects can be spawned

    private Transform[] children;
    private float minX, maxX, minY, maxY;
    private int oldLevel = 0;

    void Start()
    {
        // Get all child transforms
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // Calculate the min and max positions
        CalculateMinMaxPositions();
    }

    void Update()
    {
        int level = MapEdgeManager.GetLevelNum();
        if (level != oldLevel)
        {
            oldLevel = level;
            RandomizePositions();
        }
    }

    void CalculateMinMaxPositions()
    {
        minX = Mathf.Infinity;
        maxX = -Mathf.Infinity;
        minY = Mathf.Infinity;
        maxY = -Mathf.Infinity;

        foreach (Transform child in children)
        {
            minX = Mathf.Min(minX, child.position.x);
            maxX = Mathf.Max(maxX, child.position.x);
            minY = Mathf.Min(minY, child.position.y);
            maxY = Mathf.Max(maxY, child.position.y);
        }
    }

    void RandomizePositions()
    {
        Texture2D baseTexture = baseTextures[MapRandomizer.instance.baseIndex];
        Debug.Log("base texture name " + baseTexture.GameObject().name);

        foreach (Transform child in children)
        {
            Vector3 newPosition = FindRandomPosition(baseTexture);
            child.position = newPosition;
        }
    }

    Vector3 FindRandomPosition(Texture2D baseTexture)
    {
        Debug.Log("base texture name " + baseTexture.GameObject().name);
        for (int i = 0; i < 100; i++) // Attempt 100 times to find a suitable position
        {
            int randomX = Random.Range(0, baseTexture.width);
            int randomY = Random.Range(0, baseTexture.height);

            Color pixelColor = baseTexture.GetPixel(randomX, randomY);

            if (IsSpawnableColor(pixelColor))
            {
                // Calculate spawn position based on texture scale
                float xPosition = (randomX / baseTexture.width) * (maxX - minX) + minX;
                float yPosition = (randomY / baseTexture.height) * (maxY - minY) + minY;

                Vector3 spawnPosition = new Vector3(xPosition, 0, yPosition); // Assuming y is the vertical axis
                return spawnPosition;
            }
        }

        // If no suitable position found after 100 attempts, return a random position within min and max bounds
        float randomXPos = Random.Range(minX, maxX);
        float randomYPos = Random.Range(minY, maxY);
        return new Vector3(randomXPos, 0, randomYPos);
    }

    bool IsSpawnableColor(Color color)
    {
        foreach (Color spawnableColor in spawnableColors)
        {
            // Check if the color is approximately equal to any of the spawnable colors
            if (ColorApproximatelyEqual(color, spawnableColor))
            {
                return true;
            }
        }
        return false;
    }

    bool ColorApproximatelyEqual(Color color1, Color color2)
    {
        // Define a threshold for color comparison
        float threshold = 0.05f; // Adjust as needed

        // Compare the difference between the RGB components of the colors
        float deltaR = Mathf.Abs(color1.r - color2.r);
        float deltaG = Mathf.Abs(color1.g - color2.g);
        float deltaB = Mathf.Abs(color1.b - color2.b);

        // If all differences are within the threshold, colors are approximately equal
        return deltaR <= threshold && deltaG <= threshold && deltaB <= threshold;
    }
}
