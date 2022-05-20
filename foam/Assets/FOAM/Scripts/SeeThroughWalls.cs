using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughWalls : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    [SerializeField] private Material wallMaterial;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform playerPosition;


    void Update()
    {
        var dir = cam.transform.position - playerPosition.position;
        var ray = new Ray(playerPosition.position, dir.normalized);

        if (Physics.Raycast(ray, 3000, mask))
            wallMaterial.SetFloat(SizeID, 1);
        else
            wallMaterial.SetFloat(SizeID, 0);

        var view = cam.WorldToViewportPoint(playerPosition.position);
        wallMaterial.SetVector(PosID, view);
    }
}
