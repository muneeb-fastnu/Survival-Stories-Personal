using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringIconClicked : MonoBehaviour
{
    public ObjectInfo currentObject;
    //  public ObjectInfo currenTool;
    public void OnCLick()
    {
        if (currentObject != null)
        {
            switch (currentObject.objectType)
            {
                case ObjectType.Building:
                    //Destroy(currentObject.gameObject);
                    if (currentObject.GetComponent<Building>())
                    {
                        ConstructionSystem.instance.DeleteBuilding(currentObject.GetComponent<Building>());
                        gameObject.SetActive(false);
                        KeyPress.instance.TriggerKeyPub();
                    }
                    break;

            }
        }

        

    }
}
