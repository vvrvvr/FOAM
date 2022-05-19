using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PlayerSwitchViews : MonoBehaviour
{
    //cameras
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private CinemachineVirtualCamera isometricCam;
    
    //camera roots
    [SerializeField] private Transform thirdPersonCameraRoot;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private ShaderController shaderController;
    [SerializeField] private Distance distaince;

    private bool isFirstPerson;
    private ThirdPersonController thirdPersonController;

    private void OnEnable()
    {
        CameraSwitcher.Register(firstPersonCam);
        CameraSwitcher.Register(thirdPersonCam);
        CameraSwitcher.Register(isometricCam);

    }
    private void OnDisable()
    {
        CameraSwitcher.Unregister(firstPersonCam);
        CameraSwitcher.Unregister(thirdPersonCam);
        CameraSwitcher.Unregister(isometricCam);
    }

    private void Start()
    {
        thirdPersonController = playerObject.GetComponent<ThirdPersonController>();

        //при старте игры включить нужный вид и контроллер
        thirdPersonController.enabled = true;
    }

    void Update()
    {
        SwitchFirstAndSecondView();
    }

    void SwitchFirstAndSecondView()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFirstPerson) //если щас изометрия
            {
                ChangeToThirdView(); //изменить на вид от третьего лица
                isFirstPerson = !isFirstPerson;
            }
            else
            {
                ChangeToIsometricView();
                isFirstPerson = !isFirstPerson;
            }
        }
    }
    public void ChangeControllerScript(bool isFirstPerson)
    {
        if(isFirstPerson)
        {
            thirdPersonController.enabled = true;
        }
        else
        {
            thirdPersonController.enabled = false;
        }
        
    }

    private void ChangeToFirsView()
    {
        CameraSwitcher.SwitchCamera(firstPersonCam);

        shaderController.enabled = true;

        shaderController.StartChangeViewEffect(false);

        Debug.Log("to first person");
    }

    private void ChangeToThirdView()
    {
        CameraSwitcher.SwitchCamera(thirdPersonCam);

        shaderController.enabled = true;

        shaderController.StartChangeViewEffect(true);

        Debug.Log("to third person");
    }

    private void ChangeToIsometricView()
    {
        
    }
    public void ChangeToStaticThirdPerson(CinemachineVirtualCamera cam)
    {

    }

}
