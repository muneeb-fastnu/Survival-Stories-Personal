using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeName : MonoBehaviour
{
    public TMP_Text user_name;
    void Start()
    {
        if(PlayerPrefs.HasKey("Username"))
        {
            user_name.text = PlayerPrefs.GetString("Username");
        }
        else
        {
            user_name.text = "BaconMan";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
