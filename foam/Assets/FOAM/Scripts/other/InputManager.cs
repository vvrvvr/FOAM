using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    

   

    public Vector2 GetMouseDelta()
    {
        if (Mouse.current != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            delta *= 0.5f;
            delta *= 0.1f;
            return delta;
        }
        else
        {
            Debug.LogError("[Mouse Init] Mouse is not connected!");
        }

        return default;
    }

    public Vector2 GetMouseScroll()
    {
        if (Mouse.current != null)
        {
            Vector2 scroll = Mouse.current.scroll.ReadValue() * 0.001f;
            return scroll;
        }
        else
        {
            Debug.LogError("[Mouse Init] Mouse is not connected!");
        }

        return default;
    }
}
