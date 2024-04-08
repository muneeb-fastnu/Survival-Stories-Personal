using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Harvester")]
public class Harvester : Skill
{
    //harvestingSpeed

    public Traits effectedTrait;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            foreach (var item in InventorySystem.instance.allItemsLibrary.allResources)
            {
                if (item.displayName.Contains("wood") || item.displayName.Contains("Stone"))
                {
                    //item.harvestingSpeed = HelperFunctions.DecreaseByPercent(tiers[tierLevel]*5, item.originalHarvestingSpeed);
                    if (tierLevel == 0)
                    {
                        item.harvestingSpeed = 200;
                    }
                    else if (tierLevel == 1)
                    {
                        item.harvestingSpeed = 90;
                    }
                    else if (tierLevel == 2)
                    {
                        item.harvestingSpeed = 75;
                    }
                    else if (tierLevel == 3)
                    {
                        item.harvestingSpeed = 50;
                    }
                    else if (tierLevel == 4)
                    {
                        item.harvestingSpeed = 25;
                    }
                }
            }


        }
    }
    public override void SetDefaultValues()
    {
        foreach (var item in InventorySystem.instance.allItemsLibrary.allResources)
        {
            if (item.displayName.Contains("wood") || item.displayName.Contains("Stone"))
            //item.harvestingSpeed = HelperFunctions.DecreaseByPercent(tiers[tierLevel]*5, item.originalHarvestingSpeed);
            {
                item.harvestingSpeed = 200;
            }
        }
    }
}
