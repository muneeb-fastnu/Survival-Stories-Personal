using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class SkillsManager : MonoBehaviour
{
    //UI Skills
    //Foraging
    public GameObject HarvesterUI;
    public GameObject WoodyUI;
    public GameObject StonerUI;
    //General
    public GameObject SwiftnessUI;
    public GameObject HealthyUI;
    public GameObject FighterUI;
    //Crafting
    public GameObject DoubleCraftUI;
    public GameObject DurabilityUI;
    //Construction
    public GameObject EfficientUI;
    public GameObject BuilderUI;
    public GameObject LongrangeUI;

    //Actuall Skill
    //Foraging
    public Skill HarvesterSkill;
    public Skill WoodySkill;
    public Skill StonerSkill;
    //General
    public Skill SwiftnessSkill;
    public Skill HealthySkill;
    public Skill FighterSkill;
    //Crafting
    public Skill DoubleCraftSkill;
    public Skill DurabilitySkill;
    //Construction
    public Skill EfficientSkill;
    public Skill BuilderSkill;
    public Skill LongrangeSkill;

    //List
    public List <Skill> allSkills = new List <Skill> ();
    public List <GameObject> skillsUI = new List <GameObject> ();
    public SkillNames skillnameWrapper = new SkillNames();

    public CharacterStats stats;
    private void Start()
    {
        //if (GameManager.LoadorNot)
        {
            //LoadData();
            Invoke(nameof(LoadData), 0.1f);
        }
        
        //SkillsSystem.skillPoints += 5;

        InvokeRepeating(nameof(SaveData), 1, 5);
        SkillsSystem.instance.ShowSKillPoints();
    }
    public void SetSkillNames()
    {
        /*
        skillnameWrapper.HarvesterName = HarvesterSkill.name;
        skillnameWrapper.WoodyName = WoodySkill.name;
        skillnameWrapper.StonerName = StonerSkill.name;
        skillnameWrapper.SwiftnessName = SwiftnessSkill.name;
        skillnameWrapper.HealthyName = HealthySkill.name;
        skillnameWrapper.FighterName = FighterSkill.name;
        skillnameWrapper.DoubleCraftName = DoubleCraftSkill.name;
        skillnameWrapper.DurabilityName = DurabilitySkill.name;
        skillnameWrapper.EfficientName = EfficientSkill.name;
        skillnameWrapper.BuilderName = BuilderSkill.name;
        skillnameWrapper.LongrangeName = LongrangeSkill.name;
        */
        
        for (int i=0; i<allSkills.Count; i++)
        {
            Skill skil = allSkills[i];
            //Debug.Log("Name: " + skil.name);

            int existingIndex = skillnameWrapper.allNames.IndexOf(skil.name);
            if (existingIndex > -1 && existingIndex < 12)
            {
                // Skill name already exists, update the tier value
                skillnameWrapper.allSkillTiers[existingIndex] = skil.tierLevel;
            }
            else
            {
                // Skill name doesn't exist, add it to the lists
                skillnameWrapper.allNames.Add(skil.name);
                skillnameWrapper.allSkillTiers.Add(skil.tierLevel);

                // If you want to limit the number of elements, you can break here
                if (skillnameWrapper.allNames.Count >= 11)
                {
                    break;
                }
            }
        }
        skillnameWrapper.skillPoints = SkillsSystem.skillPoints;

        string str = JsonConvert.SerializeObject(skillnameWrapper);

        PlayerPrefs.SetString("SkillNames", str);

    }
    public void SaveData()
    {
        SetSkillNames();
        foreach (GameObject skilui in skillsUI)
        {
            SkillsUi skilsui = skilui.GetComponent<SkillsUi>();
            Skill skil = skilsui.skillData;

            //PlayerPrefs.SetInt("Skill_" + skil.name, skil.tierLevel);

            
        }
        SkillsSystem.instance.ShowSKillPoints();
    }
    int oldSkillPoints;
    public void LoadData()
    {
        
        if (PlayerPrefs.HasKey("SkillNames"))
        {
            Debug.Log(" : ");
            string sn = PlayerPrefs.GetString("SkillNames");
            if(sn == null || sn == "")
            {
                return;
            }
            skillnameWrapper = JsonConvert.DeserializeObject<SkillNames>(sn);
            Debug.Log("Wrapper Value: " + sn);
            int skillItemsAmount = skillnameWrapper.allNames.Count;
            oldSkillPoints = skillnameWrapper.skillPoints;
            SkillsSystem.skillPoints = skillnameWrapper.skillPoints;
            foreach (Skill ski in allSkills) 
            {
                for (int i = 0; i < skillItemsAmount; i++)
                {
                    if(ski.name == skillnameWrapper.allNames[i])
                    {
                        
                        Debug.Log(ski.name + " : " +  ski.tierLevel + " should be " + skillnameWrapper.allSkillTiers[i]);
                        foreach (var item in InventorySystem.instance.allItemsLibrary.allSKills)
                        {
                            item.SetDefaultValues();
                        }
                        for (int j = 0; j < skillnameWrapper.allSkillTiers[i]; j++)
                        {
                            SkillsSystem.skillPoints += 50;
                            SkillsUi SkillsUiScript = skillsUI[i].GetComponent<SkillsUi>();
                            SkillsUiScript.UnlockSkillBtnClicked();
                            //ski.unlockTier(ski);
                            //ski.ProcessSkill();
                        }
                    }
                }
            }
            Invoke(nameof(LoadOldSkillPoints), 2f);
        }
    }
    public void LoadOldSkillPoints()
    {
        SkillsSystem.skillPoints = oldSkillPoints;
    }

}
[System.Serializable]
public class SkillWrapper
{
    public string skillName;
    public int tierLevel;
}
[System.Serializable]
public class SkillNames
{
    /*
    //Foraging
    public string HarvesterName;
    public string WoodyName;
    public string StonerName;
    //General
    public string SwiftnessName;
    public string HealthyName;
    public string FighterName;
    //Crafting
    public string DoubleCraftName;
    public string DurabilityName;
    //Construction
    public string EfficientName;
    public string BuilderName;
    public string LongrangeName;
    */
    //allnames
    public List<string> allNames = new List<string>();
    public List<int> allSkillTiers = new List<int>();
    public int skillPoints;
}