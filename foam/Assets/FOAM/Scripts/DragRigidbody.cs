﻿using UnityEngine;
using System.Collections;
//using ThunderWire.CrossPlatform.Input;

public class DragRigidbody : MonoBehaviour
{
    //private CrossPlatformInput crossPlatformInput;
    //private PlayerFunctions pfunc;
    //private DelayEffect delay;
    //private ItemSwitcher itemSwitcher;
    private bool inputsLoaded = true; //вот это надо подтащить из скрипта инпута 
    private Camera playerCam;
    private GameManager gameManager;
    private InteractManager interact;
    private ScriptManager scriptManager;
    private InputManager inputManager;

    [Header("Main")]
    public LayerMask CullLayers;
    [Layer] public int InteractLayer;

    [Header("Drag")]
    public float ThrowStrength = 2f;
    public float DragSmoothing = 10f;
    public float minDistanceZoom = 1.5f;
    public float maxDistanceZoom = 3f;
    public float maxDistanceGrab = 4f;
    public float spamWaitTime = 0.5f;

    [Header("Other")]
    public float rotateSpeed = 10f;
    public float rotateSmoothing = 1f;
    public float rotationDeadzone = 0.1f;
    public float objectZoomSpeed = 3f;

    [Space(7)]
    public bool FreezeRotation = true;
    public bool enableObjectPull = true;
    public bool enableObjectRotation = true;
    public bool enableObjectZooming = true;
    public bool dragHideWeapon;
    public bool fixedHold = false;

    #region Private Variables
    private Transform oldParent;

    private GameObject objectHeld;
    private GameObject objectRaycast;
    private Rigidbody heldRigidbody;
    private DraggableObject heldDraggable;

    private GameObject fixedVelocityObj;
    private Rigidbody fixedVelocityRigid;

    private float PickupRange = 3f;
    private float distance;

    private bool RotateButton;
    private bool GrabObject;
    private bool isObjectHeld;
    private bool isRotatePressed;
    private bool antiSpam;

    private float timeDropCheck;
    private float zoomInputY;
    private Vector2 rotateValue;
    private Vector2 rotationVelocity;
    private Vector2 smoothRotation;
    #endregion

    void Awake()
    {
        //delay = transform.root.GetComponentInChildren<DelayEffect>(true);
        //crossPlatformInput = CrossPlatformInput.Instance;
        //pfunc = GetComponent<PlayerFunctions>();
        scriptManager = ScriptManager.Instance;
        inputManager = InputManager.Instance;
        interact = GetComponent<InteractManager>();
        gameManager = GameManager.Instance;
        playerCam = scriptManager.MainCamera;
        PickupRange = interact.RaycastRange;
    }

    void Start()
    {
        //itemSwitcher = GetComponent<ScriptManager>().GetScript<ItemSwitcher>();
        isObjectHeld = false;
        objectHeld = null;
    }

    void Update()
    {
        gameManager.isHeld = objectHeld != false;
        interact.isHeld = objectHeld != false;

        if (gameManager.isPaused) return;

        if (inputsLoaded)
        {
            RotateButton = true;//crossPlatformInput.GetInput<bool>("Fire");

            if (objectRaycast && !antiSpam && !gameManager.isWeaponZooming)
            {
                if (Input.GetMouseButtonDown(0)) // нажать кнопочку и по ходу взять или положить
                {
                    GrabObject = !GrabObject;
                }
            }

            if (Input.GetMouseButtonDown(1) && objectHeld) //нажать кнопочку и выбросить
            {
                ThrowObject();
            }


            Vector2 delta = inputManager.GetMouseDelta(); //получить дельту
            zoomInputY = inputManager.GetMouseScroll().y * objectZoomSpeed; //получить скролл У

            if (Mathf.Abs(delta.x) > rotationDeadzone)
            {
                rotateValue.x = -(delta.x * rotateSpeed);
            }
            else
            {
                rotateValue.x = 0;
            }

            if (Mathf.Abs(delta.y) > rotationDeadzone)
            {
                rotateValue.y = delta.y * rotateSpeed;
            }
            else
            {
                rotateValue.y = 0;
            }

            //else if (crossPlatformInput.deviceType == Device.Gamepad)
            //{
            //    Vector2 delta = crossPlatformInput.GetInput<Vector2>("Look");

            //    if (RotateButton)
            //    {
            //        zoomInputY = crossPlatformInput.GetInput<Vector2>("Movement").y;
            //    }

            //    if (Mathf.Abs(delta.x) > rotationDeadzone)
            //    {
            //        rotateValue.x = -(delta.x * rotateSpeed);
            //    }
            //    else
            //    {
            //        rotateValue.x = 0;
            //    }

            //    if (Mathf.Abs(delta.y) > rotationDeadzone)
            //    {
            //        rotateValue.y = delta.y * rotateSpeed;
            //    }
            //    else
            //    {
            //        rotateValue.y = 0;
            //    }
            //}

            smoothRotation = Vector2.SmoothDamp(smoothRotation, rotateValue, ref rotationVelocity, Time.deltaTime * rotateSmoothing);
        }

        if (GrabObject)
        {
            if (!isObjectHeld)
            {
                TryDragObject();
            }
            else if (objectHeld)
            {
                HoldObject();
            }
            else
            {
                ResetDrag(false);
            }
        }
        else if (isObjectHeld)
        {
            DropObject();
        }

        Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(playerAim, out RaycastHit hit, PickupRange, CullLayers))
        {
            if (hit.collider.gameObject.layer == InteractLayer)
            {
                if (hit.collider.GetComponent<DraggableObject>())
                {
                    objectRaycast = hit.collider.gameObject;
                    gameManager.canGrab = true;
                }
            }
            else
            {
                if (!isObjectHeld)
                {
                    objectRaycast = null;
                    gameManager.canGrab = false;
                }
            }

            scriptManager.IsGrabRaycast = objectRaycast != null;

            if (objectHeld)
            {
                if (RotateButton && enableObjectRotation)
                {
                    heldRigidbody.freezeRotation = true;
                    gameManager.LockPlayerControls(false, false, false);
                    objectHeld.transform.Rotate(playerCam.transform.up, smoothRotation.x, Space.World);
                    objectHeld.transform.Rotate(playerCam.transform.right, smoothRotation.y, Space.World);
                    isRotatePressed = true;
                }
                else if (isRotatePressed)
                {
                    gameManager.LockPlayerControls(true, false, false);
                    isRotatePressed = false;
                }
            }
        }
        else
        {
            if (!isObjectHeld)
            {
                objectRaycast = null;
                gameManager.canGrab = false;
                scriptManager.IsGrabRaycast = false;
            }
        }
    }

    void TryDragObject()
    {
        StartCoroutine(AntiSpam());

        if (objectRaycast && !objectRaycast.GetComponent<Rigidbody>())
        {
            Debug.LogError("[DragRigidbody] " + objectRaycast.name + " does not contains a Rigidbody component!");
            GrabObject = false;
            return;
        }
        else
        {
            heldRigidbody = objectRaycast.GetComponent<Rigidbody>();
            heldDraggable = objectRaycast.GetComponent<DraggableObject>();
        }

        objectHeld = objectRaycast;

        if (enableObjectPull && heldDraggable)
        {
            if (heldDraggable.automaticDistance)
            {
                float dist = Vector3.Distance(transform.position, objectHeld.transform.position);

                if (dist > maxDistanceZoom)
                {
                    distance = maxDistanceZoom - 1f;
                }
                else if (dist < minDistanceZoom)
                {
                    distance = minDistanceZoom + 1f;
                }
                else
                {
                    distance = dist;
                }
            }
            else
            {
                distance = heldDraggable.dragDistance;
            }
        }

        heldRigidbody.useGravity = false;
        heldRigidbody.isKinematic = false;
        heldRigidbody.freezeRotation = FreezeRotation;

        if (fixedHold)
        {
            oldParent = objectHeld.transform.parent;
            objectHeld.transform.SetParent(playerCam.transform);
            heldRigidbody.velocity = Vector3.zero;

            fixedVelocityObj = new GameObject(objectHeld.name + " velocity");
            fixedVelocityObj.transform.position = objectHeld.transform.position;

            Collider collider = fixedVelocityObj.AddComponent<SphereCollider>();
            collider.isTrigger = true;

            fixedVelocityRigid = fixedVelocityObj.AddComponent<Rigidbody>();
            fixedVelocityRigid.angularDrag = heldRigidbody.angularDrag;
            fixedVelocityRigid.drag = heldRigidbody.drag;
            fixedVelocityRigid.mass = heldRigidbody.mass;

            fixedVelocityRigid.useGravity = false;
            fixedVelocityRigid.isKinematic = false;
        }

        //itemSwitcher.FreeHands(dragHideWeapon);
        //gameManager.UIPreventOverlap(true);
        //gameManager.ShowGrabSprites();
        gameManager.isGrabbed = true;
        //delay.isEnabled = false;
        //pfunc.zoomEnabled = false;
        timeDropCheck = 0f;

        GetComponent<ScriptManager>().ScriptEnabledGlobal = false;
       // Physics.IgnoreCollision(objectHeld.GetComponent<Collider>(), transform.root.GetComponent<Collider>(), true);
        objectHeld.SendMessage("OnRigidbodyDrag", SendMessageOptions.DontRequireReceiver);

        isObjectHeld = true;
    }

    void HoldObject()
    {
        //interact.CrosshairVisible(false);
        //gameManager.HideSprites(HideHelpType.Interact);
        distance = Mathf.Clamp(distance, minDistanceZoom, maxDistanceZoom - 0.5f);

        Ray playerAim = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 currPos = objectHeld.transform.position;
        Vector3 grabPos = playerAim.origin + playerAim.direction * distance;

        heldRigidbody.velocity = (grabPos - currPos) * DragSmoothing;

        if (fixedHold && fixedVelocityObj && fixedVelocityRigid)
        {
            Vector3 fixedVelocity = Vector3.zero;
            Vector3 currFixedPos = fixedVelocityObj.transform.position;
            Vector3.SmoothDamp(currFixedPos, grabPos, ref fixedVelocity, Time.deltaTime * DragSmoothing);
            fixedVelocityRigid.velocity = fixedVelocity;
        }

        if (enableObjectZooming)
        {
            float mw = zoomInputY;

            if (mw > 0 && distance < maxDistanceZoom)
            {
                distance += mw;
            }
            else if (mw < 0 && distance > minDistanceZoom)
            {
                distance += mw;
            }
        }

        if (timeDropCheck < 1f)
        {
            timeDropCheck += Time.deltaTime;
        }
        else if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
        {
            DropObject();
        }
    }

    public bool CheckHold()
    {
        return isObjectHeld;
    }

    public void ResetDrag(bool throwObj)
    {
        //if (dragHideWeapon) itemSwitcher.FreeHands(false);

        gameManager.LockPlayerControls(true, true, false);
        //gameManager.UIPreventOverlap(false);
        //gameManager.HideSprites(HideHelpType.Help);
        gameManager.isGrabbed = false;

        if (objectHeld && heldRigidbody)
        {
            if (fixedHold) objectHeld.transform.parent = oldParent;

            heldRigidbody.useGravity = true;
            heldRigidbody.freezeRotation = false;

            //Physics.IgnoreCollision(objectHeld.GetComponent<Collider>(), transform.root.GetComponent<Collider>(), false);
            objectHeld.SendMessage("OnRigidbodyRelease", SendMessageOptions.DontRequireReceiver);

            if (throwObj)
            {
                heldRigidbody.AddForce(playerCam.transform.forward * ThrowStrength, ForceMode.Impulse);
            }
            else if (fixedHold && fixedVelocityObj)
            {
                heldRigidbody.AddForce(fixedVelocityRigid.velocity, ForceMode.VelocityChange);
            }

            if (fixedVelocityObj)
            {
                Destroy(fixedVelocityObj);
            }
        }

        //interact.CrosshairVisible(true);
        GetComponent<ScriptManager>().ScriptEnabledGlobal = true;

        //delay.isEnabled = true;
        objectRaycast = null;
        objectHeld = null;
        heldRigidbody = null;
        heldDraggable = null;
        GrabObject = false;
        isObjectHeld = false;
        timeDropCheck = 0f;

        StartCoroutine(ResetZoom());
    }

    void DropObject()
    {
        ResetDrag(false);
    }

    void ThrowObject()
    {
        ResetDrag(true);
    }

    IEnumerator AntiSpam()
    {
        antiSpam = true;
        yield return new WaitForSeconds(spamWaitTime);
        antiSpam = false;
    }

    IEnumerator ResetZoom()
    {
        yield return new WaitUntil(() => !Input.GetKeyDown(KeyCode.O));
        //pfunc.zoomEnabled = true;
    }
}
