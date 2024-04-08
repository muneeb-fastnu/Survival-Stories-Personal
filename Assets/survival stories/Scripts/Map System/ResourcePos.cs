using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourcePos : MonoBehaviour
{
    public Tilemap[] baseTilemaps; // Reference to multiple base tilemaps
    public TileBase[] spawnableTiles; // Tiles where resources can spawn
    public Tilemap[] decorationsTilemaps; // Reference to multiple decorations tilemaps

    public TileBase[] extraTiles;
    public GameObject testPrefab;

    private Vector3[] initialPositions; // Array to store initial positions of children
    private Transform[] children;
    private float minX, maxX, minY, maxY;

    int oldLevel = -1;
    int level;

    void Awake()
    {
        initialPositions = new Vector3[transform.childCount];
        // Get all child transforms
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
            initialPositions[i] = children[i].position;
        }

        // Calculate the min and max positions
        CalculateMinMaxPositions();

        // Randomize positions
        
    }

    void LateUpdate()
    {
        level = MapEdgeManager.GetLevelNum(); // Get the current base index from MapRandomizer
        if (oldLevel != level)
        {
            oldLevel = level;
            if (level == -1)
            {
                SetDefaultPosition();
            }
            else
            {
                
            }
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
    void SetDefaultPosition()
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].position = initialPositions[i]; // Reset position to initial position
        }
    }
    public void RandomizePositions()
    {
        Debug.Log("Base Index: " + MapRandomizer.instance.baseIndex);
        Debug.Log("decor Index: " + MapRandomizer.instance.decorIndex);
        foreach (Transform child in children)
        {
            //Debug.Log(child.name + " is at " + child.position);
            Vector3 newPosition = FindRandomPosition();
            if (child != null)
            {
                child.position = newPosition;
            }
            //Debug.Log(child.name + " is at " + child.position);
        }
    }

    Vector3 FindRandomPosition()
    {
        BoundsInt bounds = baseTilemaps[MapRandomizer.instance.baseIndex].cellBounds; // Use the current base tilemap for bounds

        for (int i = 0; i < 1000; i++) // Attempt 100 times to find a suitable position
        {
            int randomX = Random.Range(bounds.min.x, bounds.max.x);
            int randomY = Random.Range(bounds.min.y, bounds.max.y);

            Vector3Int randomPosition = new Vector3Int(randomX, randomY, 0);
            //Debug.Log("Random Position: " + randomPosition);
            if (IsTileInSpawnableTiles(randomPosition))
            {
                //Debug.Log("Selected");
                return baseTilemaps[MapRandomizer.instance.baseIndex].CellToWorld(randomPosition); // Use the current base tilemap for world position
            }
            else
            {
                //Debug.Log("Rejected");
            }
        }

        Debug.Log("Returning Center");
        // If no suitable position found after 100 attempts, return center of tilemap
        return baseTilemaps[MapRandomizer.instance.baseIndex].GetCellCenterWorld(new Vector3Int((int)bounds.center.x, (int)bounds.center.y, 0));

    }

    bool IsTileInSpawnableTiles(Vector3Int position)
    {
        TileBase tile = baseTilemaps[MapRandomizer.instance.baseIndex].GetTile(position); // Use the current base tilemap
        TileBase decorationTile = decorationsTilemaps[MapRandomizer.instance.decorIndex].GetTile(position); // Use the current decorations tilemap
        return ArrayContainsTile(spawnableTiles, tile) && decorationTile == null;
    }

    bool ArrayContainsTile(TileBase[] array, TileBase tile)
    {
        foreach (var item in array)
        {
            if (item == tile)
            {
                return true;
            }
        }
        return false;
    }
    public void testfoo()
    {
        Tilemap baseTilemap = baseTilemaps[MapRandomizer.instance.baseIndex]; // Get the current base tilemap

        foreach (Vector3Int position in baseTilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = baseTilemap.GetTile(position); // Get the tile at the current position

            // Check if the current tile exists in the extraTiles array
            if (ArrayContainsTile(extraTiles, tile))
            {
                Debug.Log("Match found");
                // Instantiate the testPrefab at the center of the current cell
                Vector3 worldPosition = baseTilemap.GetCellCenterWorld(position);
                Instantiate(testPrefab, worldPosition, Quaternion.identity);
            }
        }
    }
}
