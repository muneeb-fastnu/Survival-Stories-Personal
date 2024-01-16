using UnityEngine;
using UnityEngine.UI;

public class CircleZoom : MonoBehaviour
{
    public GameObject mainCharacter;
    private Transform mainCharacterTransform;
    public Image circleImage;
    public float maxSize = 100f; // Set your initial max size
    public float minSize = 10f; // Set your minimum size
    public float sizeReductionSpeed = 10f; // Adjust the speed of size reduction

    public bool triggered = false;

    public static CircleZoom instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Update()
    {
        if (triggered)
        {
            if (mainCharacter != null)
            {
                mainCharacterTransform = mainCharacter.transform;
            }
            if (mainCharacterTransform != null)
            {
                SetCirclePosition(mainCharacterTransform);
                ReduceSizeOverTime();
            }
            
        }
    }

    void ReduceSizeOverTime()
    {
        // Calculate the distance between the UI Image and the main character
        float distance = Vector3.Distance(transform.position, mainCharacterTransform.position);

        // Map the distance to a range between minSize and maxSize
        float mappedSize = Mathf.Lerp(minSize, maxSize, 1f - (distance / maxSize));
        // Gradually reduce the size of the UI Image
        circleImage.rectTransform.sizeDelta = Vector2.Lerp(circleImage.rectTransform.sizeDelta, new Vector2(mappedSize, mappedSize), Time.deltaTime * sizeReductionSpeed);
        if(circleImage.rectTransform.sizeDelta.x < 200f)
        {
            triggered = false;
        }
    }

    public void CircleZoomTrigger()
    {
        circleImage.gameObject.SetActive(true);
        triggered = true;
    }

    public void ResetCircleSize()
    {
        circleImage.rectTransform.sizeDelta = new Vector2(6000f, 6000f);
    }

    public void SetCirclePosition(Transform parent = null)
    {
        Vector3 positionwOFfset = new Vector3(parent.position.x, parent.position.y, 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(positionwOFfset);
        transform.position = screenPos;
    }
}
