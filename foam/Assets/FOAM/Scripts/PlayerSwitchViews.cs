using UnityEngine;
using Cinemachine;
using StarterAssets;
using System.IO;
using System.Collections;

public class PlayerSwitchViews : MonoBehaviour
{
    //cameras
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private CinemachineVirtualCamera isometricCam;
    [SerializeField] private CinemachineVirtualCamera twoDimensionCam;
    
    //camera roots
    [SerializeField] private Transform thirdPersonCameraRoot;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private ShaderController shaderController;
    [SerializeField] private Distance distaince;

    private bool isFirstPerson;

    //controllers
    private ThirdPersonController thirdPersonController;
    private ThirdPersonIsometricController thirdPersonIsometricController;
    private ThirdPersonTwoDimentionController thirdPersonTwoDimentionController;

    private void OnEnable()
    {
        CameraSwitcher.Register(firstPersonCam);
        CameraSwitcher.Register(thirdPersonCam);
        CameraSwitcher.Register(isometricCam);
        CameraSwitcher.Register(twoDimensionCam);

    }
    private void OnDisable()
    {
        CameraSwitcher.Unregister(firstPersonCam);
        CameraSwitcher.Unregister(thirdPersonCam);
        CameraSwitcher.Unregister(isometricCam);
        CameraSwitcher.Unregister(twoDimensionCam);
    }

    private void Start()
    {
        //при старте выбираем нужную камеру
        CameraSwitcher.SwitchCamera(twoDimensionCam);

        //получаем ссылки на все контроллеры
        thirdPersonController = playerObject.GetComponent<ThirdPersonController>();
        thirdPersonIsometricController = playerObject.GetComponent<ThirdPersonIsometricController>();
        thirdPersonTwoDimentionController = playerObject.GetComponent<ThirdPersonTwoDimentionController>();

        //при старте игры включить  контроллер
        thirdPersonController.enabled = false;
        thirdPersonIsometricController.enabled = false;
        thirdPersonTwoDimentionController.enabled = true;
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
                thirdPersonIsometricController.enabled = false;
                thirdPersonController.enabled = true;
                ChangeToThirdView(); //изменить на вид от третьего лица
                isFirstPerson = !isFirstPerson;
            }
            else // если вид от тертьего лица
            {
                isFirstPerson = !isFirstPerson;
                
                ChangeToIsometricView(); // изменить на изометрию
                thirdPersonIsometricController.enabled = true;
                thirdPersonController.enabled = false;
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

        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }

        //Debug.Log("to third person");
    }

    private void ChangeToIsometricView()
    {
        CameraSwitcher.SwitchCamera(isometricCam);
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }
        //Debug.Log("to isometric");
    }

    public void ChangeToStaticThirdPerson(CinemachineVirtualCamera cam)
    {

    }

    public void ChangeToTwoDimension()
    {
        CameraSwitcher.SwitchCamera(twoDimensionCam);
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }
    }

    IEnumerator WaitA()
    {
        yield return new WaitForSeconds(1f);
        
    }
    IEnumerator WaitB()
    {
        yield return new WaitForSeconds(1f);
       
    }

}
