using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script defining Draggable Objects.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class DraggableObject : MonoBehaviour
{
    public enum DragSoundType { None, MoreTimes, Once }

    
    private Collider m_collider;
    private Rigidbody m_rigidbody;

    //[HideInInspector]
    public bool isInCollision = false;

    [Header("Drag Settings")]
    public float dragDistance = 2f;
    public bool dragAndUse = false;
    public bool automaticDistance = true;

    [Header("Drag Sounds")]
    public DragSoundType soundType = DragSoundType.None;
    public AudioClip dragSound;
    [Range(0, 2)] public float dragVolume = 1f;

    private float initialDrag;
    private float initialAngularDrag;
    private Vector3 voxelSize;
    private Vector3[] voxels;

    private bool isPlayedOnce;
    private bool isPlayed;

    void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_rigidbody = GetComponent<Rigidbody>();

        initialDrag = m_rigidbody.drag;
        initialAngularDrag = m_rigidbody.angularDrag;

    }



    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            isInCollision = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            isInCollision = false;
        }
    }

    public void OnRigidbodyDrag()
    {
        //if (!isPlayedOnce && dragSound && soundType != DragSoundType.None)
        //{
        //    if (soundType == DragSoundType.MoreTimes)
        //    {
        //        AudioSource.PlayClipAtPoint(dragSound, transform.position, dragVolume);
        //    }
        //    else if (!isPlayed)
        //    {
        //        AudioSource.PlayClipAtPoint(dragSound, transform.position, dragVolume);
        //        isPlayed = true;
        //    }

        //    isPlayedOnce = true;
        //}
    }

    public void OnRigidbodyRelease()
    {
        //isPlayedOnce = false;
    }

    void OnDrawGizmos()
    {
        if (voxels != null)
        {
            for (int i = 0; i < voxels.Length; i++)
            {
                Gizmos.color = Color.magenta - new Color(0f, 0f, 0f, 0.75f);
                Gizmos.DrawCube(transform.TransformPoint(voxels[i]), voxelSize * 0.8f);
            }
        }
    }

    //public Dictionary<string, object> OnSave()
    //{
    //    return new Dictionary<string, object>()
    //    {
    //        { "position", transform.position },
    //        { "rotation", transform.eulerAngles },
    //        { "isPlayed", isPlayed },
    //        { "rigidbody_kinematic", GetComponent<Rigidbody>().isKinematic },
    //        { "rigidbody_gravity", GetComponent<Rigidbody>().useGravity },
    //        { "rigidbody_freeze", GetComponent<Rigidbody>().freezeRotation },
    //    };
    //}

    //public void OnLoad(JToken token)
    //{
    //    transform.position = token["position"].ToObject<Vector3>();
    //    transform.eulerAngles = token["rotation"].ToObject<Vector3>();
    //    isPlayed = (bool)token["isPlayed"];
    //    GetComponent<Rigidbody>().isKinematic = (bool)token["rigidbody_kinematic"];
    //    GetComponent<Rigidbody>().useGravity = (bool)token["rigidbody_gravity"];
    //    GetComponent<Rigidbody>().freezeRotation = (bool)token["rigidbody_freeze"];
    //}
}
