using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventOnCommand
{
    public bool isPressed;
    public KeyCode interactKey;
    public UnityEvent interactAction;
}
