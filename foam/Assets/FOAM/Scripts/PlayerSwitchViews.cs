using UnityEngine;
using Cinemachine;

public class PlayerSwitchViews : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            firstPersonCam.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            firstPersonCam.gameObject.SetActive(false);
        }


    }
}
