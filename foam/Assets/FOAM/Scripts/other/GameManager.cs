using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
//using UnityEngine.Rendering.PostProcessing;
//using ThunderWire.CrossPlatform.Input;
//using ThunderWire.Game.Options;
//using HFPS.Prefs;
//using ThunderWire.Cutscenes;

public enum HideHelpType
{
    Interact, Help
}

public class GameManager : Singleton<GameManager>
{

    [HideInInspector] public bool isPaused;
    [HideInInspector] public bool isHeld;
    [HideInInspector] public bool canGrab;
    [HideInInspector] public bool isGrabbed;
    [HideInInspector] public bool isExamining;
    [HideInInspector] public bool isLocked;
    [HideInInspector] public bool isWeaponZooming;
    [HideInInspector] public bool ConfigError;

    private bool playerLocked;

    private ScriptManager scriptManager;

    private void Awake()
    {
        scriptManager = ScriptManager.Instance;
    }
   

    public void LockPlayerControls(bool Controller, bool Interact, bool CursorVisible, int BlurLevel = 0, bool BlurEnable = false, bool ResetBlur = false, int ForceLockLevel = 0)
    {
        if (ForceLockLevel == 2)
        {
            playerLocked = false;
        }

        //if (!playerLocked)
        //{
        //    //Controller Lock
        //    Player.GetComponent<PlayerController>().isControllable = Controller;
        //    scriptManager.GetScript<PlayerFunctions>().enabled = Controller;
        //    scriptManager.ScriptGlobalState = Controller;
        //    LockScript<MouseLook>(Controller);

        //    //Interact Lock
        //    scriptManager.GetScript<InteractManager>().inUse = !Interact;
        //}

        ////Show Cursor
        //ShowCursor(CursorVisible && !isGamepad);

        ////Blur Levels
        //if (BlurLevel > 0)
        //{
        //    if (BlurEnable)
        //    {
        //        SetBlur(true, BlurLevel, ResetBlur);
        //    }
        //    else
        //    {
        //        if (playerLocked)
        //        {
        //            SetBlur(true, oldBlurLevel, true);
        //        }
        //        else
        //        {
        //            SetBlur(false, BlurLevel);
        //        }
        //    }
        //}

        //if (ForceLockLevel == 1)
        //{
        //    playerLocked = true;
        //    oldBlurLevel = BlurLevel;
        //}
    }
}
