using UnityEngine;

public class DoubleTapDetector : MonoBehaviour
{
    private float lastTapTime;
    public float doubleTapThreshold = 0.2f; // Adjust this based on your desired double tap speed

    public GameObject Controller;
    public GameObject Player;
    MainPlayerController mainPlayerController;

    public int SitStandState = 1; //1=stand, 0=sit
    private void Start()
    {
        mainPlayerController = Player.GetComponent<MainPlayerController>();
    }
    public void HandleDoubleTap()
    {

        Debug.Log("double tapped");
        if (InventorySystem.instance.isIdle)
        {
            if (SitStandState == 1)
            {
                PromptManager.Instance.SitDown();
                //Controller.SetActive(false);
                //mainPlayerController.isPlayerAllowedMove = false;
                SitStandState = 1;
            }
            else
            {
                //mainPlayerController.isPlayerAllowedMove = true;
                //Controller.SetActive(true); 
                SitStandState = 1;
            }
        }
    }
}
