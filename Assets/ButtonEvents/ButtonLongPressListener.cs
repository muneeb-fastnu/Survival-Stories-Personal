/*--------------------------------------
   Email  : hamza95herbou@gmail.com
   Github : https://github.com/herbou
----------------------------------------*/

using System;
using System.Collections;
using UnityEngine ;
using UnityEngine.Events ;
using UnityEngine.EventSystems ;
using UnityEngine.UI ;


[RequireComponent(typeof(Button))]
public class ButtonLongPressListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [Tooltip("Hold duration in seconds")]
    [Range(0.3f, 5f)] public float holdDuration = 0.5f;
    public UnityEvent onLongPress;
    public UnityEvent onShortPress;

    private bool isPointerDown = false;
    private bool isLongPressed = false;
    private DateTime pressTime;

    private Button button;

    private WaitForSeconds delay;


    private void Awake() {
        button = GetComponent<Button>();
        delay = new WaitForSeconds(0.1f);
    }

    public void OnPointerDown(PointerEventData eventData) {
        isPointerDown = true;
        pressTime = DateTime.Now;
        StartCoroutine(Timer());
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (!isLongPressed && button.interactable) // Check if it's not a long press and the button is interactable
        {
            if (onShortPress != null && onShortPress.GetPersistentEventCount() > 0) // Check if onShortPress event is set
            {
                onShortPress.Invoke(); // Invoke the short press event
            }
            else
            {
                button.onClick?.Invoke(); // Invoke the normal button click event if short press event is not set
            }
        }
        isLongPressed = false; // Reset long press flag
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // This method is required by the IPointerClickHandler interface
        // We don't need to implement anything here
    }

    private IEnumerator Timer() {
        while (isPointerDown && !isLongPressed) {
            double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

            if (elapsedSeconds >= holdDuration) {
                isLongPressed = true;
                if (button.interactable)
                    onLongPress?.Invoke();

                yield break;
            }

            yield return delay;
        }
    }
}
