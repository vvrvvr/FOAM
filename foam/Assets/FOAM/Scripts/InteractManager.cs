using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ThunderWire.Utility;

public class InteractManager : MonoBehaviour
{
    public Camera mainCamera;
    //private DraggableObject dragRigidbody;

    [Header("Raycast")]
    public float RaycastRange = 3;
    public LayerMask cullLayers;
    public LayerMask interactLayers;

    [Header("Crosshair Textures")]
    public Sprite defaultCrosshair;
    public Sprite interactCrosshair;
    private Sprite default_interactCrosshair;

    [Header("Crosshair")]
    private Image CrosshairUI;
    public int crosshairSize = 5;
    public int interactSize = 10;

    [HideInInspector] public bool isHeld = false;
    [HideInInspector] public bool inUse;
    [HideInInspector] public GameObject RaycastObject;
    private GameObject LastRaycastObject;

    private int default_interactSize;
    private int default_crosshairSize;
    private bool UsePressed;

    private bool isPressed;
    private bool isDraggable;
    private bool isCorrectLayer;

    private void Awake()
    {
        mainCamera = ScriptManager.Instance.MainCamera;
        RaycastObject = null;
    }


    void Update()
    {
        Ray playerAim = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(playerAim, out RaycastHit hit, RaycastRange, cullLayers))
        {
            if (interactLayers.CompareLayer(hit.collider.gameObject.layer))
            {
                RaycastObject = hit.collider.gameObject;
                //isDraggable = (dragRigidbody = RaycastObject.GetComponent<DraggableObject>()) != null;
                isCorrectLayer = true;
                Debug.Log("hit interact object");


            }
            else if (RaycastObject)
            {
                isCorrectLayer = false;
            }
        }
        else if (RaycastObject)
        {
            isCorrectLayer = false;
        }

        if (!isCorrectLayer)
        {
            //ResetCrosshair();
            //CrosshairChange(false);
            //gameManager.HideSprites(HideHelpType.Interact);
            //interactItem = null;
            RaycastObject = null;
            //dynamicObj = null;
        }

        if (!RaycastObject)
        {
            //gameManager.HideSprites(HideHelpType.Interact);
            //CrosshairChange(false);
            //dynamicObj = null;
        }

    }

}
