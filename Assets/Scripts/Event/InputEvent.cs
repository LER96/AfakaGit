using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputEvent : MonoBehaviour
{
    public List<EventOnCommand> commands;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < commands.Count; i++)
        {
            var command = commands[i];

            if (Input.GetKeyDown(command.interactKey))//&& command.isPressed == false)
            {
                command.isPressed = true;
                command.interactAction.Invoke();
            }
            else if (Input.GetKeyUp(command.interactKey))
            {
                command.isPressed = false;
            }
        }
    }
}
