using UnityEngine;
using UnityEngine.Events;
using UnityEditor.EditorTools;

public class OnEventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent OnEventtoTrigger; // Event to trigger when the script is enabled or disabled
    [SerializeField] bool onEnabled; // Flag to determine if the event should be triggered when the script is enabled
    [SerializeField] float onEnabledEventDelay; // Delay in seconds before triggering the event when the script is enabled

    void OnEnable()
    {
        // Check if the event should be triggered when the script is enabled
        if (onEnabled)
        {
            // Invoke the event after a specified delay
            Invoke(nameof(InvokeEvent), onEnabledEventDelay);
        }
    }

    public void InvokeEvent()
    {
        // Invoke the UnityEvent
        OnEventtoTrigger.Invoke();
    }
}