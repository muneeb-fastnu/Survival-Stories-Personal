using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Android;


public class CraftingItemUIHolder : MonoBehaviour
{
    public Image ItemIcon;
    public CraftableItemData item;

    public Button craft;
    public TextMeshProUGUI info;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI ownedAmount;
    public GameObject PricePrefab;
    public GameObject Content;
    public List<GameObject> priceGameObj = new List<GameObject>();

    public GameObject greyButton;


    public static CraftingItemUIHolder instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    private void OnEnable()
    {
        //   GetWoodQuantity();
        UpdatePriceAmmounts();
        InventorySystem.itemsEvent += UpdatePriceAmmounts;

        
    }
    private void OnDisable()
    {
        InventorySystem.itemsEvent -= UpdatePriceAmmounts;
    }
    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Craft (1)")
            {
                greyButton = child.gameObject;
                break;
            }
        }
        InvokeRepeating(nameof(GreyButtonsCheck),1f, 5f);
    }
    public void GreyButtonsCheck()
    {
        if (item.objectType == ObjectType.Tool)
        {
            if (item.StringID == "T_saxe")
            {
                CraftableItemData craftableItem = (CraftableItemData)item;
                foreach (SubPrice pr in craftableItem.price)
                {
                    Traits health = (Traits)pr.objectData;
                    if ((health.currentQuantity-200) > pr.quantity)
                    {
                        greyButton.SetActive(false);
                    }
                    else
                    {
                        greyButton.SetActive(true);
                    }
                }
            }
            else
            {
                if (CraftingSystem.CheckRequirementsForCrafting(item))
                {
                    greyButton.SetActive(false);
                    //Debug.Log("green");
                }
                else
                {
                    greyButton.SetActive(true);
                    //Debug.Log("grey");
                }
            }
        }
        else if (item.objectType == ObjectType.Building)
        {
            Debug.Log("grey button object type is building and item is: " + item.displayName);
            if (GreyButtonCheckForConstruction(item))
            {
                greyButton.SetActive(false);
                //Debug.Log("green");
            }
            else
            {
                greyButton.SetActive(true);
                //Debug.Log("grey");
            }
        }
    }
    public bool GreyButtonCheckForConstruction(CraftableItemData myItem)
    {
        bool hasRequiredAmount = true;

        foreach (var _building in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (myItem == _building.building && ((BuildingData)item).maxAllowed > _building.currentlyPlaced)
            {
                Debug.Log("Building Under observation: " + myItem.displayName);
                foreach (SubPrice pr in myItem.price)
                {
                    Debug.Log("for Building: " + _building.building.StringID + " you need: " + pr.quantity + " " + pr.objectData.displayName);
                    if(InventorySystem.HasItemWithAmmount(pr.objectData, pr.quantity))
                    {
                        Debug.Log("player has amount");
                    }
                    else
                    {
                        Debug.Log("player doesnt have amount");
                        return false;
                    }
                    
                }
                
            }


        }

        return hasRequiredAmount;
    }
    //private void Start()
    //{///i didnt do it here to save memory when game starts  connect button in editor
    //   // craft.onClick.AddListener(CraftItem);
    //    // ShowData();
    //}

    /// <summary>
    /// complete it tomorow
    /// </summary>
    [ContextMenu("show new data ")]
    public void ShowData()
    {
        ItemIcon.sprite = item.icon;
        info.text = item.info;
        itemName.text = item.name;



        foreach (SubPrice p in item.price)
        {
            GameObject gam = Instantiate(PricePrefab, Content.transform);

            if (gam.TryGetComponent<Price>(out Price pri))
            {
                pri.SubPrice = new SubPrice();
                pri.SubPrice = p;
            }
            else
            {
                Price pr = gam.AddComponent<Price>();
                pr.SubPrice = new SubPrice();
                pr.SubPrice = p;
            }

            priceGameObj.Add(gam);
            gam.GetComponentInChildren<Image>().sprite = p.objectData.icon;
            gam.GetComponentInChildren<TextMeshProUGUI>().text = (/*InventorySystem.GetItemQuantity((ObjectData)p.resourceData).ToString()*/ /*+*/ "0/" + p.quantity);
        }
        foreach (BuildingData p in item.buildingsAOE)
        {
            GameObject gam = Instantiate(PricePrefab, Content.transform);

            if (gam.TryGetComponent<BuildingAOEPrice>(out BuildingAOEPrice pri))
            {
                pri.buildingReq = new BuildingData();
                pri.buildingReq = p;
            }
            else
            {
                BuildingAOEPrice pr = gam.AddComponent<BuildingAOEPrice>();
                pr.buildingReq = new BuildingData();
                pr.buildingReq = p;
            }

            priceGameObj.Add(gam);
            gam.GetComponentInChildren<Image>().sprite = p.icon;
            gam.GetComponentInChildren<TextMeshProUGUI>().text = p.displayName;
        }

    }

    [ContextMenu("destroy old data ")]
    public void DestroyOldData()
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (GameObject gam in priceGameObj)
            {

                DestroyImmediate(gam.gameObject);
            }
        }
        priceGameObj.Clear();
        //   int c = Content.transform.childCount;

    }

    public void CraftItem()
    {
        Debug.Log("called times 1");
        if (item.objectType == ObjectType.Tool)
        {
            CraftingSystem.CraftButtonClicked(item);
        }
        else if (item.objectType == ObjectType.Building)
        {
            ConstructionSystem.ConstructionButtonClicked(item);
        }
        UpdatePriceAmmounts();

        Invoke(nameof(GreyButtonsCheck), 0.1f);
    }
    public void GetItemQuanitityFromInventory()
    {


    }
    public void UpdatePriceAmmounts()
    {
        //update using switch

        foreach (GameObject t in priceGameObj)
        {
            if (t.TryGetComponent<Price>(out Price p))
            {

                t.transform.GetComponentInChildren<TextMeshProUGUI>().text = "<color=#00FF51>" + (PriceTypeCheck(p.SubPrice.objectData).ToString() + "</color>" + "/" + p.SubPrice.quantity);

                //   Debug.Log(PriceTypeCheck(p.SubPrice.objectData).ToString() + " :" + p.SubPrice.objectData.displayName);
            }
            else if (t.TryGetComponent<BuildingAOEPrice>(out BuildingAOEPrice B))
            {
                if (ConstructionSystem.IsPlayerInRangeBool(B.buildingReq))
                {
                    Debug.Log("In Updated Price Amount uiHolder:: Building in Range");
                    t.GetComponentInChildren<Image>().color = new Color(0, .5f, 0, 1);
                    GreyButtonsCheck();
                }
                else
                {
                    t.GetComponentInChildren<Image>().color = new Color(.5f, 0, 0, 1);
                }

            }


        }
        if (item.objectType == ObjectType.Tool)
        {
            ownedAmount.text = InventorySystem.GetItemQuantity(item).ToString();
        }
        else if (item.objectType == ObjectType.Building)
        {
            ownedAmount.text = ConstructionSystem.GetBuildingQuantity((BuildingData)item) + "/" + ((BuildingData)item).maxAllowed;
        }


    }
    [ContextMenu("get wood quantity")]
    public void GetWoodQuantity()
    {

        //Debug.Log("script raeeeeeeeeeeeeeeeeeeeeen");
        //  Debug.Log(PriceTypeCheck(priceGameObj[0].GetComponent<Price>().SubPrice.objectData) + "wood availableeeeeeeeeeeeeeeeeeeee");

    }


    public static int PriceTypeCheck(ObjectData obj)
    {
        obj.GetType();



        switch (obj.objectType)
        {
            case ObjectType.Resource:
                return InventorySystem.GetItemQuantity((ObjectData)obj) + InventorySystem.GetItemQuantityInContainers((ObjectData)obj);


            case ObjectType.Tool:
                return InventorySystem.GetItemQuantity((ObjectData)obj);

            case ObjectType.Building:
                return ConstructionSystem.IsPlayerInRangeInt((BuildingData)obj);


            case ObjectType.PlayerTrait:

                return (int)((Traits)obj).currentQuantity;


            default:
                return 0;





        }
        // Debug.Log("returns 0 here");



    }
    
}
