using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SeeThroughWalls : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    [SerializeField] private Material wallMaterial;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] private Transform playerPosition;
    private float currentSize = 0f;
    private float desiredSize = 1f;
    private float duration = 0.3f;
    private bool isHit;


    void Update()
    {
        var dir = cam.transform.position - playerPosition.position;
        var ray = new Ray(playerPosition.position, dir.normalized);

        var rayHit = Physics.Raycast(ray, 3000, mask);
        if ( rayHit && !isHit)
        {
            isHit = true;
            DOTween.To(() => currentSize, x => currentSize = x, desiredSize, duration);
            StartCoroutine(SetWallMaterialSize(true));
            //Debug.Log("hit obj");
        }   

        if(!rayHit && isHit)
        {
            isHit = false;
            DOTween.To(() => currentSize, x => currentSize = x, 0, duration);
            StartCoroutine(SetWallMaterialSize(false));
            //Debug.Log("not hit obj");
        }
            

        var view = cam.WorldToViewportPoint(playerPosition.position);
        wallMaterial.SetVector(PosID, view);
    }

    private IEnumerator SetWallMaterialSize(bool isIncrease)
    {
        if (isIncrease)
        {
            while (currentSize < desiredSize  && isHit)
            {
                wallMaterial.SetFloat(SizeID, currentSize);
                Debug.Log("Coroutine working");
                yield return null;
            }
        }
        else
        {
            while (currentSize > 0.05f && !isHit)
            {
                wallMaterial.SetFloat(SizeID, currentSize);
                Debug.Log("Coroutine working");
                yield return null;
            }
        }
    }
}
