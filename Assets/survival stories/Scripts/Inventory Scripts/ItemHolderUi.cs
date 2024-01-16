using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ItemHolderUi : MonoBehaviour
{
    public Image ItemBackground;
    public Image ItemIcon;
    public TextMeshProUGUI Stacks;
    public InventoryItem item;
    public bool inUse = false;


    public int currentDurabilityIndex = 0;

    private GameObject Tooltip;
    private TextMeshProUGUI description;
    private TextMeshProUGUI durabilityIndex;


    GameObject itemHolderUIHelper_gameObject;
    ItemHolderUI_helper itemHolderUIHelper;

    [SerializeField] DurabilityBar durabilityBar;

    private void OnEnable()
    {
        if (item != null)
        {
            item.ValueChanged += HandleStackUpdated;
        }

    }

    private void Start()
    {
        itemHolderUIHelper_gameObject = GameObject.FindWithTag("ItemHolderUIHelper");
        itemHolderUIHelper = itemHolderUIHelper_gameObject.GetComponent<ItemHolderUI_helper>();
        description = itemHolderUIHelper.description;
        durabilityIndex = itemHolderUIHelper.durability;
        Tooltip = itemHolderUIHelper.Tooltip;
    }

    public void UpdateItemDataToUi(InventoryItem itemToDisplay)
    {
        Debug.Log("container processed 4");
        item = itemToDisplay;
        itemToDisplay.ValueChanged += HandleStackUpdated;
        if (ItemIcon != null)
        {
            ItemIcon.sprite = item.data.icon;
        }
        else
        {
            Transform childTransform = transform.Find("itemIcon");
            ItemIcon = childTransform.GetComponent<Image>();
            ItemIcon.sprite = item.data.icon;
        }

        if (item.stacksize > 1)
        {
            Stacks.text = item.stacksize.ToString();
        }
        inUse = true;

        ToolContainerandDurabilityCheck();

    }


    public void ToolContainerandDurabilityCheck()
    {
        if (item.data.objectType == ObjectType.Tool)
        {


            if (((ToolData)item.data).backgroundColors.Length > 0)
            {
                ItemBackground.color = ((ToolData)item.data).backgroundColors[((ToolData)item.data).backgroundColors.Length - 1];
                DurabilityChangeCheck();

            }
        }

        if (item.data.objectType == ObjectType.Tool)
        {
            if (((ToolData)item.data).toolType.HasFlag(ToolType.Container) || ((ToolData)item.data).toolType.HasFlag(ToolType.Consumable))
            {
                AddButtonandToolUseScript();
                Debug.Log("container processed 5");
                ContainerStacksChangeCheck();

            }
        }

    }
    public void AddButtonandToolUseScript()
    {
        if (!GetComponent<AutoUseTool>())
        {
            AutoUseTool tool = this.gameObject.AddComponent<AutoUseTool>();

            //  ItemBackground.gameObject.AddComponent<Button>().onClick.AddListener(tool.AutoUseThis);
            TapCountChecker tap = ItemBackground.gameObject.AddComponent<TapCountChecker>();
            tap.TapCountEvent += tool.DoubleTapped;

        }


    }
    private void OnDisable()
    {
        // item.ValueChanged -= HandleStackUpdated;
    }
    public void DurabilityChangeCheck()
    {  // the player wont see unless we open the inventory so idk if we need to change this constantly
        Debug.Log("durability 3  loss duration has passsed");
        ToolData tool = ((ToolData)item.data);
        if (tool.backgroundColors.Length > 0)
        {
            if (item.CurrentDurability > 0)
            {
                int portionSize = ((ToolData)item.data).durability / tool.backgroundColors.Length - 1;
                int a = (int)item.CurrentDurability;
                int portionIndex = a / portionSize;
                Debug.LogWarning(" durability index is " + portionIndex);
                if (portionIndex >= 0 && portionIndex != tool.backgroundColors.Length)
                {

                    ItemBackground.color = tool.backgroundColors[portionIndex];
                }
            }
            else
            {

                Debug.Log("durability 2 loss duration has passsed");
                InventorySystem.instance.Remove(item);

            }
        }
    }
    public void ContainerStacksChangeCheck()
    {  // the player wont see unless we open the inventory so idk if we need to change this constantly
        Debug.Log("res in container stacks50000006");
        if (item.containerStacks <= ((ToolData)item.data).contains.maxQuantity)
        {
            // Debug.Log("container processed 6");
            // Debug.Log(item.data.displayName + " loook here mate");

            //  Debug.Log(newtool.contains.maxQuantity + " up and down -1" + newtool.contains.NewIcons.Count);
            float portionSize = ((ToolData)item.data).contains.maxQuantity / ((ToolData)item.data).contains.NewIcons.Count;

            // Debug.Log(portionSize + " this is the portion size");
            int portionIndex = item.containerStacks / (int)portionSize;
            Debug.Log("res in container stacks56 size" + portionIndex);
            //   Debug.LogWarning(" durability index is " + portionIndex);
            if (portionIndex >= 0 && portionIndex <= ((ToolData)item.data).contains.NewIcons.Count)
            {
                Debug.Log("res in container stacks56 1");
                ///    Debug.Log("icon changed here");

                Debug.Log("res in container stacks56 2");

                if (portionIndex == 0)
                {
                    ItemIcon.sprite = ((ToolData)item.data).contains.NewIcons[portionIndex];
                }
                else
                {
                    ItemIcon.sprite = ((ToolData)item.data).contains.NewIcons[portionIndex - 1];
                }


            }

        }


    }

    public void HandleStackUpdated()
    {
        //  Debug.Log("refresh inventory1111");
        if (item.stacksize > 1)
        {
            //  Debug.Log("refresh inventory133333333333333333333333333333");
            Stacks.text = item.stacksize.ToString();
        }
        else if (item.stacksize == 1)
        {
            Stacks.text = "";
        }
        ToolContainerandDurabilityCheck();
    }

    private string GetResourceInfo()
    {
        string info;

        if(item.data.objectType == ObjectType.Resource)
        {
            ResourceData data = (ResourceData)item.data;
            info = data.info;
        }
        else if(item.data.objectType == ObjectType.Tool)
        {
            ToolData data = (ToolData)item.data;
            info = data.info;
        }
        else if(item.data.objectType == ObjectType.Building)
        {
            BuildingData data = (BuildingData)item.data;
            info = data.info;
        }
        else
        {
            info = "Not Specified for " + item.objectID;
        }

        return info;
    }
    private float GetResourceDurability()
    {
        float info;

        if (item.data.objectType == ObjectType.Resource)
        {
            info = item.CurrentDurability;
        }
        else if (item.data.objectType == ObjectType.Tool)
        {
            info = item.CurrentDurability;
        }
        else if (item.data.objectType == ObjectType.Building)
        {
            info = item.CurrentDurability;
        }
        else
        {
            info = 0;
        }

        return info;
    }
    public void HoldItemDetails()
    {
        Tooltip.SetActive(true);
        Debug.Log("Hold New UI, in inventory");
        description.text = GetResourceInfo();
        //durabilityIndex.text = GetResourceDurability();
        CancelInvoke(nameof(TurnOffTooltip));
        Invoke(nameof(TurnOffTooltip), 5);
    }
    private void TurnOffTooltip()
    {
        Tooltip.SetActive(false);
    }

    private void Update()
    {
        DisplayDurability();
    }

    public void DisplayDurability()
    {
        durabilityBar.SetDurability(GetResourceDurability()/100);
    }
}
