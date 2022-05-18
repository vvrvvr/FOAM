using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform player;
    [SerializeField] Material playerMaterial;
    [SerializeField] Shader shader;
    [SerializeField] Shader shaderLit;
    [SerializeField] PlayerSwitchViews pSw;
    bool isChahged = false;
    public bool isChangeControlScript;
    private float distance;

    
    // Start is called before the first frame update
    void Start()
    {
        playerMaterial.shader = shaderLit;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(cam.position, player.position);
        Debug.Log(distance);
        
        if(!isChahged && distance < 1)
        {
            playerMaterial.shader = shader;
            isChahged = true;
            //Debug.Log("shader changed");
        }
        if(isChahged && distance > 1)
        {
            playerMaterial.shader = shaderLit;
            isChahged = false;
        }
        if (isChangeControlScript)
            CheckDistanceToChangeView();
    }
    void CheckDistanceToChangeView()
    {
        if(distance >= 3f)
        {
            pSw.ChangeControllerScript(true);
            isChangeControlScript = false;
            Debug.Log("distance to change");
        }
    }
}
