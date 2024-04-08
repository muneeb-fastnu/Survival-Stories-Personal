using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParentChanger : MonoBehaviour
{
    private Transform[] childEnemies; // List to store child enemies
    private Transform[] children;
    int oldLevel;
    public int level;
    private void Awake()
    {
        oldLevel = -1;
        level = -1;
        FindChildEnemies();
    }
    void Start()
    {
        // Find all child enemies with the tag "NewEnemy" and add them to the list
        Invoke(nameof(UpdateChildEnemies), 0.1f);
        
    }

    

    void FindChildEnemies()
    {
        // Find all child enemies with the tag "NewEnemy" and add them to the list

        children = new Transform[transform.childCount];
        childEnemies = new Transform[transform.childCount];

        int j = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
            if (children[i].CompareTag("NewEnemy"))
            {
                childEnemies[j] = children[i];
                j++;
            }
        }
    }

    public void UpdateChildEnemies()
    {
        int level = MapEdgeManager.GetLevelNum();
        foreach (Transform enemy in childEnemies)
        {
            //Debug.Log("Name: " + enemy.name);
            // Get the EnemyChanger component from each child and call ChangeMeTo function
            if (enemy == null)
            {
                continue;
            }
            EnemyChanger enemyChanger = enemy.GetComponent<EnemyChanger>();

            if (enemyChanger != null)
            {
                if (level == -1)
                {
                    if (enemy.gameObject.name.Contains("Bunny"))
                    {
                        enemyChanger.ChangeMeTo(enemyChanger.bunnyAnimator, enemyChanger.bunnySpr, enemyChanger.bunnyStat, enemyChanger.bunnyStr);

                    }
                    else if (enemy.gameObject.name.Contains("Forest"))
                    {
                        enemyChanger.ChangeMeTo(enemyChanger.slimeAnimator, enemyChanger.slimeSpr, enemyChanger.slimeStat, enemyChanger.slimeStr);

                    }
                    else if (enemy.gameObject.name.Contains("Mole"))
                    {
                        enemyChanger.ChangeMeTo(enemyChanger.moleAnimator, enemyChanger.moleSpr, enemyChanger.moleStat, enemyChanger.moleStr);
                    }
                }
                else
                {
                    if (level % 3 == 1)
                    {
                        //Debug.Log("to Slime");
                        enemyChanger.ChangeMeTo(enemyChanger.slimeAnimator, enemyChanger.slimeSpr, enemyChanger.slimeStat, enemyChanger.slimeStr);
                    }
                    else if (level % 3 == 2)
                    {
                        //Debug.Log("to mole");
                        enemyChanger.ChangeMeTo(enemyChanger.moleAnimator, enemyChanger.moleSpr, enemyChanger.moleStat, enemyChanger.moleStr);
                    }
                    else if (level % 3 == 0)
                    {
                        //Debug.Log("to Bunny");
                        enemyChanger.ChangeMeTo(enemyChanger.bunnyAnimator, enemyChanger.bunnySpr, enemyChanger.bunnyStat, enemyChanger.bunnyStr);
                    }
                }
            }
            else
            {
                Debug.Log("Could Not find enemy changer");
            }
        }
    }
}
