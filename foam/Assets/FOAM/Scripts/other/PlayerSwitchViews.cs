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

    //ссылки на объекты и скрипты
    [SerializeField] private GameObject playerObject;
    [SerializeField] private ShaderController shaderController;
    [SerializeField] private Distance distaince;

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
        CameraSwitcher.SwitchCamera(firstPersonCam);

        //получаем ссылки на все контроллеры
        thirdPersonController = playerObject.GetComponent<ThirdPersonController>();
        thirdPersonIsometricController = playerObject.GetComponent<ThirdPersonIsometricController>();
        thirdPersonTwoDimentionController = playerObject.GetComponent<ThirdPersonTwoDimentionController>();

        //при старте игры включить  нужный контроллер
        thirdPersonController.enabled = true;
        thirdPersonIsometricController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;
    }

    void Update()
    {
        SwitchFirstAndSecondView();
    }


    //метод для отладки видов
    void SwitchFirstAndSecondView()
    {
        if (Input.GetKeyDown(KeyCode.F)) // third
        {
            ChangeToThirdView();
        }
        if (Input.GetKeyDown(KeyCode.G)) // isometric
        {
            ChangeToIsometricView();
        }
        if (Input.GetKeyDown(KeyCode.H)) // first person
        {
            ChangeToFirsView();
        }
        if (Input.GetKeyDown(KeyCode.J)) // 2d
        {
            ChangeToTwoDimensionView(new Vector3(0f, -90f, 0f), true);
        }
    }

    private void ChangeToFirsView()
    {
        CameraSwitcher.SwitchCamera(firstPersonCam);
        shaderController.enabled = true;
        shaderController.StartChangeViewEffect(false);
        Debug.Log("to first person");

        //выбор нужного контроллера управления
        thirdPersonController.enabled = true;
        thirdPersonIsometricController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;
    }

    private void ChangeToThirdView()
    {
        //запуск шейдера на персонаже
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }

        //переключение камеры в паблик статик скрипте
        CameraSwitcher.SwitchCamera(thirdPersonCam);
        Debug.Log("to third person");

        //выбор нужного контроллера управления
        thirdPersonController.enabled = true;
        thirdPersonIsometricController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;
    }

    private void ChangeToIsometricView()
    {
        //запуск шейдера на персонаже
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }

        //переключение камеры в паблик статик скрипте
        CameraSwitcher.SwitchCamera(isometricCam);

        //выбор нужного контроллера управления
        thirdPersonIsometricController.enabled = true;
        thirdPersonController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;

        Debug.Log("to isometric");
    }


    public void ChangeToTwoDimensionView(Vector3 direction, bool isTwoDimentionLockControl)
    {
        //запуск шейдера на персонаже
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }

        //манипуляции с контроллером - выбор направления и лока w,s 
        thirdPersonTwoDimentionController.isTwoDimentionLockControl = isTwoDimentionLockControl;
        thirdPersonTwoDimentionController.camRootDesiredRotation = direction;

        //переключение камеры в паблик статик скрипте
        CameraSwitcher.SwitchCamera(twoDimensionCam);

        //выбор нужного контроллера управления
        thirdPersonTwoDimentionController.enabled = true;
        thirdPersonController.enabled = false;
        thirdPersonIsometricController.enabled = false;
    }

    public void ChangeToStaticThirdPerson(CinemachineVirtualCamera cam)
    {

    }
}
