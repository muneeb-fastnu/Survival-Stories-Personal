using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCTutorial : MonoBehaviour
{
    public GameObject mainCharacter;
    public GameObject npcCharacter;

    public GameObject npcSpeechBox;
    public TMP_Text npcText;
    public GameObject mainCharSpeechBox;
    public TMP_Text mainCharText;


    void Start()
    {
        
    }

    public void SetBubblePosition2(Transform parent = null, bool npc = true)
    {
        //speechBubble.gameObject.SetActive(true);
        if (npc)
        {
            Vector3 positionwOFfset = new Vector3(parent.position.x - 1f, parent.position.y + 1f, 0f);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(positionwOFfset);
            npcSpeechBox.transform.position = screenPos;
        }
        else
        {
            Vector3 positionwOFfset = new Vector3(parent.position.x + 1f, parent.position.y + 0.5f, 0f);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(positionwOFfset);
            mainCharSpeechBox.transform.position = screenPos;
        }
    }

    public void MainCharDot()
    {
        mainCharText.text = "...";
    }
    public void MainCharOKAY()
    {
        mainCharText.text = "Okay...";
    }
    public void NPCText1()
    {
        npcText.text = "Ooh, you're new here";
    }
    public void NPCText2()
    {
        npcText.text = "I've been here for many years";
    }
    public void NPCText3()
    {
        npcText.text = "Let me teach you a thing or two";
    }
    public void NPCText4()
    {
        npcText.text = "This is where you keep stuff";
    }
    public void NPCText5()
    {
        npcText.text = "And here is where you can craft or build stuff";
    }
    public void NPCText6()
    {
        npcText.text = "You get smarter at adventuring the more you do stuff";

    }
    public void NPCText7()
    {
        npcText.text = "And you unlock special skills when you do stuff enough times";
    }
    public void NPCText8()
    {
        npcText.text = "There are people here who would like stuff from you!";
    }
    public void NPCText9()
    {
        npcText.text = "And that's all. Here's an axe for you to start your journey";
    }
    public void NPCText10()
    {
        npcText.text = "Oh and be careful of the monsters!";
    }
    public void NPCText11()
    {
        npcText.text = "Ah, you're going deeper into the forest";
    }
    public void NPCText12()
    {
        npcText.text = "It's dangerous here, and it gets more dangerous the deeper you go";
    }
    public void NPCText13()
    {
        npcText.text = "If you ever feel overwhelmed, use this pocket portal to come home";
    }
    public void NPCText14()
    {
        npcText.text = "Oh, but note that the charge for the portal takes a while.. use it sparingly!";
    }
    

    public void MainCharSpeechBox_ON()
    {
        mainCharSpeechBox.SetActive(true);
    }
    public void NPCSpeechBox_ON()
    {
        npcSpeechBox.SetActive(true);
    }
    public void MainCharSpeechBox_OFF()
    {
        mainCharSpeechBox.SetActive(false);
    }
    public void NPCSpeechBox_OFF()
    {
        npcSpeechBox.SetActive(false);
    }
    public void TurnOffNPC()
    {
        npcCharacter.SetActive(false);
    }

    public void TurnOnNPC()
    {
        npcCharacter.SetActive(true);
    }

    public void AllignNPC()
    {
        npcCharacter.transform.position = new Vector3(mainCharacter.transform.position.x, mainCharacter.transform.position.y + 1.5f, mainCharacter.transform.position.z);
        SetBubblePosition2(npcCharacter.transform, true);
        SetBubblePosition2(mainCharacter.transform, false);
    }
}
