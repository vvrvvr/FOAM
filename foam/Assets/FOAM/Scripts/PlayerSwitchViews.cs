using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PlayerSwitchViews : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private GameObject playerObject;

    [SerializeField] private Transform thirdPersonCameraRoot;

    [SerializeField] private Distance distaince;

    private bool isFirstPerson;
    private ThirdPersonController thirdPersonController;

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
            if (isFirstPerson)
            {
                firstPersonCam.gameObject.SetActive(false);
                thirdPersonCam.gameObject.SetActive(true);
                isFirstPerson = false;
                Debug.Log("to third person");
            }
            else
            {
                
                //firstPersonCameraRoot.rotation = Quaternion.Euler(Vector3.back);
                firstPersonCam.gameObject.SetActive(true);
                thirdPersonCam.gameObject.SetActive(false);
                isFirstPerson = true;
                Debug.Log("to first person");
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

}
