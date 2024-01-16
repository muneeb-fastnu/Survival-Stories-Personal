using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemHolderUI_helper : MonoBehaviour
{
    public GameObject Tooltip;
    public TextMeshProUGUI description;
    public TextMeshProUGUI durability;

    public TextMeshProUGUI GetDescription() { return description; }
    public TextMeshProUGUI GetDurability() { return durability; }
}
