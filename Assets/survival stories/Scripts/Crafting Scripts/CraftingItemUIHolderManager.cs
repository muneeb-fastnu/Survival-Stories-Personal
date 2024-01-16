using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItemUIHolderManager : MonoBehaviour
{
    CraftingItemUIHolder[] craftingItemHolders;


    public static CraftingItemUIHolderManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        craftingItemHolders = GetComponentsInChildren<CraftingItemUIHolder>();
    }
    public void CheckGreyButtonsInAllChildren()
    {
        // Call GreyButtonsCheck on each child
        foreach (CraftingItemUIHolder craftingItemHolder in craftingItemHolders)
        {
            craftingItemHolder.GreyButtonsCheck();
        }
    }
}
