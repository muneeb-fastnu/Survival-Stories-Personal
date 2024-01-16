using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MarketQuestUIHolder : MonoBehaviour
{
    public Image ItemIcon;
    public MarketQuest item;
    public TextMeshProUGUI reward;
    public Button craft;
    public TextMeshProUGUI saying;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI time;
    public GameObject PricePrefab;
    public GameObject Content;
    public List<GameObject> priceGameObj = new List<GameObject>();


    private void OnEnable()
    {
        //   GetWoodQuantity();
        UpdatePriceAmmounts();
        InventorySystem.itemsEvent += UpdatePriceAmmounts;
    }
    private void Start()
    {


    }

    private void OnDisable()
    {
        InventorySystem.itemsEvent -= UpdatePriceAmmounts;
    }
    //private void Start()
    //{///i didnt do it here to save memory when game starts  connect button in editor
    //   // craft.onClick.AddListener(CraftItem);
    //    // ShowData();
    //}

    public void ShowNewQuest()
    {

        item.Randomizer();
        string rewardString = "<color=white>Reward :</color> " + "<color=white>" + item.rewardAmmount + "</color> " + "<color=#FAF459>" + item.rewardCurrency.ToString() + "</color>";
        //reward.text = "Reward :" + item.rewardAmmount + " " + item.rewardCurrency.ToString();
        reward.text = rewardString;
        DestroyOldData();
        ShowData();
    }
    /// <summary>
    /// complete it tomorow
    /// </summary>
    [ContextMenu("show new data ")]
    public void ShowData()
    {
        ItemIcon.sprite = item.icon;
        saying.text = item.info;
        npcName.text = item.displayName;



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

    public void CollectReward()
    {

        //  MarketQuestSystem

        if (MarketQuestSystem.CheckRequirementsForCrafting(item))
        {

            MarketQuestSystem.CollectRes(item);

            item.DeActivateQuest();
            this.gameObject.SetActive(false);
        }


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
                    t.GetComponentInChildren<Image>().color = new Color(0, .5f, 0, 1);
                }
                else
                {
                    t.GetComponentInChildren<Image>().color = new Color(.5f, 0, 0, 1);
                }

            }


        }
        // string

        time.text = "Time Remaining : " + item.time.ToString();



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
                return InventorySystem.GetItemQuantity((ObjectData)obj);

            case ObjectType.Tool:
                return InventorySystem.GetItemQuantity((ObjectData)obj);

            case ObjectType.Building:

                return ConstructionSystem.IsPlayerInRangeInt((BuildingData)obj);

            case ObjectType.PlayerTrait:
                return 0;
            //   CheckAvailableSubPriceForCraftingInInventory((ToolData)obj);

            default:
                return 0;





        }
        // Debug.Log("returns 0 here");



    }
    public bool ShowTime()
    {
        item.TimeCheck();

        time.text = item.time.ToString();
        time.text = string.Format("{0:D2}:{1:D2}", item.time.Hours, item.time.Minutes);
        return item.TimeCheck();


    }

}
