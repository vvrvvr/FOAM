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

    //������ �� ������� � �������
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
        //��� ������ �������� ������ ������
        CameraSwitcher.SwitchCamera(firstPersonCam);

        //�������� ������ �� ��� �����������
        thirdPersonController = playerObject.GetComponent<ThirdPersonController>();
        thirdPersonIsometricController = playerObject.GetComponent<ThirdPersonIsometricController>();
        thirdPersonTwoDimentionController = playerObject.GetComponent<ThirdPersonTwoDimentionController>();

        //��� ������ ���� ��������  ������ ����������
        thirdPersonController.enabled = true;
        thirdPersonIsometricController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;
    }

    void Update()
    {
        SwitchFirstAndSecondView();
    }


    //����� ��� ������� �����
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

        //����� ������� ����������� ����������
        thirdPersonController.enabled = true;
        thirdPersonIsometricController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;
    }

    private void ChangeToThirdView()
    {
        //������ ������� �� ���������
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }

        //������������ ������ � ������ ������ �������
        CameraSwitcher.SwitchCamera(thirdPersonCam);
        Debug.Log("to third person");

        //����� ������� ����������� ����������
        thirdPersonController.enabled = true;
        thirdPersonIsometricController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;
    }

    private void ChangeToIsometricView()
    {
        //������ ������� �� ���������
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }

        //������������ ������ � ������ ������ �������
        CameraSwitcher.SwitchCamera(isometricCam);

        //����� ������� ����������� ����������
        thirdPersonIsometricController.enabled = true;
        thirdPersonController.enabled = false;
        thirdPersonTwoDimentionController.enabled = false;

        Debug.Log("to isometric");
    }


    public void ChangeToTwoDimensionView(Vector3 direction, bool isTwoDimentionLockControl)
    {
        //������ ������� �� ���������
        if (CameraSwitcher.IsActiveCamera(firstPersonCam))
        {
            shaderController.enabled = true;
            shaderController.StartChangeViewEffect(true);
        }

        //����������� � ������������ - ����� ����������� � ���� w,s 
        thirdPersonTwoDimentionController.isTwoDimentionLockControl = isTwoDimentionLockControl;
        thirdPersonTwoDimentionController.camRootDesiredRotation = direction;

        //������������ ������ � ������ ������ �������
        CameraSwitcher.SwitchCamera(twoDimensionCam);

        //����� ������� ����������� ����������
        thirdPersonTwoDimentionController.enabled = true;
        thirdPersonController.enabled = false;
        thirdPersonIsometricController.enabled = false;
    }

    public void ChangeToStaticThirdPerson(CinemachineVirtualCamera cam)
    {

    }
}
