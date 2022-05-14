using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PlayerSwitchViews : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private GameObject playerObject;

    [SerializeField] private Transform firstPersonCameraRoot;
    [SerializeField] private Transform thirdPersonCameraRoot;

    private bool isFirstPerson;
    private FirstPersonController firstPersonController;
    private ThirdPersonController thirdPersonController;

    private void Start()
    {
        firstPersonController = playerObject.GetComponent<FirstPersonController>();
        thirdPersonController = playerObject.GetComponent<ThirdPersonController>();

        //при старте игры включить нужный вид и контроллер
        firstPersonController.enabled = false;
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
            if (isFirstPerson)
            {
                thirdPersonCameraRoot.rotation = Quaternion.Euler(Vector3.back);
                firstPersonCam.gameObject.SetActive(false);
                thirdPersonCam.gameObject.SetActive(true);
                isFirstPerson = false;
                firstPersonController.enabled = false;
                thirdPersonController.enabled = true;
                Debug.Log("to third person");
            }
            else
            {
                firstPersonCameraRoot.rotation = Quaternion.Euler(Vector3.back);
                firstPersonCam.gameObject.SetActive(true);
                thirdPersonCam.gameObject.SetActive(false);
                isFirstPerson = true;
                firstPersonController.enabled = true;
                thirdPersonController.enabled = false;
                Debug.Log("to first person");
            }
        }
    }
}
