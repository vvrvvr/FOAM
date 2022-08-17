using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : Singleton<ScriptManager>
{
    [Header("Cameras")]
    public Camera MainCamera;

    [HideInInspector] public bool IsGrabRaycast;
    [HideInInspector] public bool ScriptEnabledGlobal;

    // Start is called before the first frame update
    void Start()
    {
        ScriptEnabledGlobal = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
