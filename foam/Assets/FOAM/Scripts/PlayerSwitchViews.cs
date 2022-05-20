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
    
    //camera roots
    [SerializeField] private Transform thirdPersonCameraRoot;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private ShaderController shaderController;
    [SerializeField] private Distance distaince;

    private bool isFirstPerson;

    //controllers
    private ThirdPersonController thirdPersonController;
    private ThirdPersonIsometricController thirdPersonIsometricController;

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
        CameraSwitcher.SwitchCamera(isometricCam);
        CameraSwitcher.SwitchCamera(thirdPersonCam);
        thirdPersonController = playerObject.GetComponent<ThirdPersonController>();
        thirdPersonIsometricController = playerObject.GetComponent<ThirdPersonIsometricController>();
        //CameraSwitcher.SwitchCamera(isometricCam); //�������

        //��� ������ ���� �������� ������ ��� � ����������
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
            if (isFirstPerson) //���� ��� ���������
            {                
                thirdPersonIsometricController.enabled = false;
                thirdPersonController.enabled = true;
                ChangeToThirdView(); //�������� �� ��� �� �������� ����
                isFirstPerson = !isFirstPerson;
            }
            else // ���� ��� �� �������� ����
            {
                isFirstPerson = !isFirstPerson;
                
                ChangeToIsometricView(); // �������� �� ���������
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
        //Debug.Log("to isometric");
    }

    public void ChangeToStaticThirdPerson(CinemachineVirtualCamera cam)
    {

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
