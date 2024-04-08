using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class MapRandomizer : MonoBehaviour
{
    public static MapRandomizer instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        baseIndex = -1;
        decorIndex = -1;
    }

    public GameObject homeBackgroundGrass;
    public GameObject newBackgroundGrass;

    public GameObject Home;
    public GameObject homeNavMesh;
    public GameObject[] baseMaps; // Reference to base map GameObjects
    public GameObject[] decorMaps; // Reference to decor map GameObjects
    public GameObject[] navMeshes;
    public int baseIndex;
    public int decorIndex;

    private int offset = 0;

    [SerializeField] ResourcePos resourcePos;
    [SerializeField] ConstructionSystem constructionSystem;
    [SerializeField] EnemyParentChanger enemyParentChanger;

    int[] testbase = new int[20];
    int[] testdeco = new int[20];
    private void Start()
    {
        /*
        int j = 0;
        for (int i = 0; i < 15; i++)
        {
            
            GetExpectedIndices(i, out testbase[j], out testdeco[j]);
            j++;
        }
        string baseStr = string.Join(",", testbase);
        string decoStr = string.Join(",", testdeco);

        // Print the result
        Debug.Log("Final InOut: " + baseStr + ",, " + decoStr);
        */
    }
    public void Randomize()
    {
        Home.SetActive(false);
        homeNavMesh.SetActive(false);

        homeBackgroundGrass.SetActive(false);

        newBackgroundGrass.SetActive(true);
        // Randomly select a base map and a decor map
        //baseIndex = Random.Range(0, baseMaps.Length);
        //decorIndex = Random.Range(0, decorMaps.Length);
        baseIndex++;
        decorIndex++;
        if (decorIndex >= decorMaps.Length)
        {
            decorIndex = 0;
        }
        if (offset >= decorMaps.Length)
        {
            offset = 0;
        }
        if (baseIndex >= baseMaps.Length)
        {
            baseIndex = 0;
            offset++;
            decorIndex = offset;
        }

        // Enable the selected base map and disable the rest
        for (int i = 0; i < baseMaps.Length; i++)
        {
            baseMaps[i].SetActive(i == baseIndex);
            navMeshes[i].SetActive(i == baseIndex);
        }

        // Enable the selected decor map and disable the rest
        for (int i = 0; i < decorMaps.Length; i++)
        {
            decorMaps[i].SetActive(i == decorIndex);
        }

        
        constructionSystem.ClearAllBuildings();
        constructionSystem.LoadData();

        enemyParentChanger.UpdateChildEnemies();
        resourcePos.RandomizePositions();
        

    }
    public void GoHome()
    {
        Home.SetActive(true);
        homeNavMesh.SetActive(true);
        homeBackgroundGrass.SetActive(true);

        newBackgroundGrass.SetActive(false);
        for (int i = 0; i < baseMaps.Length; i++)
        {
            baseMaps[i].SetActive(false);
        }
        for (int i = 0; i < decorMaps.Length; i++)
        {
            decorMaps[i].SetActive(false);
        }
        for (int i = 0; i < navMeshes.Length; i++)
        {
            navMeshes[i].SetActive(false);
        }
        baseIndex = -1;
        decorIndex = -1;

        constructionSystem.ClearAllBuildings();
        constructionSystem.LoadData();
        MapEdgeManager.instance.uiBlocker.SetActive(false);
        enemyParentChanger.UpdateChildEnemies();
    }

    public void GetExpectedIndices(int level, out int expectedBaseIndex, out int expectedDecorIndex)
    {
        if (level > 0) 
        { 
            int totalMaps = baseMaps.Length;

            // Calculate the expected baseIndex based on the level
            expectedBaseIndex = level % totalMaps;

            // Consider a maximum decor offset of 1
            //expectedDecorIndex = (expectedBaseIndex + 1) % totalMaps;

            // Calculate the tens and ones digits for the base index and decor index
            int tensDigit = level / totalMaps; // Calculate the tens digit
            int onesDigit = level % totalMaps; // Calculate the ones digit

            // Construct the two-digit base-5 number
            int base5Number = (tensDigit * 10) + onesDigit;

            // Split the two-digit base-5 number into tens and ones digits
            int offset = base5Number / 10; // Tens digit represents the offset
            int base5Value = base5Number % 10; // Ones digit represents the value

            expectedDecorIndex = (offset + base5Value) % 5;
            //Debug.Log(" Actual: " + (expectedBaseIndex + 1) + ", " + (expectedDecorIndex + 1));
        }
        else if (level == 0)
        {
            expectedBaseIndex = 0;
            expectedDecorIndex = 0;
        }
        else
        {
            expectedBaseIndex = -1;
            expectedDecorIndex = -1;
        }
        Debug.Log("INput: " + level + " OUTput: " + expectedBaseIndex + ", " + expectedDecorIndex);

    }

}
