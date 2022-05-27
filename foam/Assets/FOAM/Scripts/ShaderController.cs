using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    
    [SerializeField] private float effectTime;
    public Material[] ChangeViewMaterials;
    //public GameObject respawnParticles;
    Material[] DefaultMaterials;

    MaterialPropertyBlock m_PropertyBlock;
    Renderer m_Renderer;
    Vector4 pos;
    Vector3 renderBounds;

    const string k_BoundsName = "_bounds";
    const string k_CutoffName = "_Cutoff";
    float m_Timer;
    float m_EndTime;

    bool m_Started = false;
    private bool isFirstPerson;
    private float minCutoff = 0.17f; //0.001f

    void Awake()
    {
        //respawnParticles.SetActive(false);
        m_PropertyBlock = new MaterialPropertyBlock();
        m_Renderer = GetComponentInChildren<Renderer>();
        DefaultMaterials = m_Renderer.materials;

        renderBounds = m_Renderer.bounds.size;
        pos.y = renderBounds.y;

        m_Renderer.GetPropertyBlock(m_PropertyBlock);
        m_PropertyBlock.SetVector(k_BoundsName, pos);
        m_PropertyBlock.SetFloat(k_CutoffName, 0.0001f);
        m_Renderer.SetPropertyBlock(m_PropertyBlock);

        pos = new Vector4(0, 0, 0, 0);

        m_Started = false;

        this.enabled = false;
    }

    void OnEnable()
    {
        //чёнить допишем
        
    }

    public void StartChangeViewEffect(bool isFP)
    {
        isFirstPerson = isFP;
        m_Renderer.materials = ChangeViewMaterials;
        m_Renderer.enabled = true;
        m_Started = true;
        if (isFirstPerson)
        {
            effectTime = 1f;
            m_Timer = 0.0f;
            Set(minCutoff);
        }
        else
        {
            effectTime = 2.5f;
            m_Timer = effectTime;
            Set(1f);
        }
    }

    void Update()
    {
        if (!m_Started)
            return;
        ChangeTransition(isFirstPerson);
        
    }

    private void ChangeTransition(bool isFirstPerson)
    {
        float cutoff = Mathf.Clamp(m_Timer / effectTime, 0.01f, 1.0f);
        Set(cutoff);

        if (isFirstPerson)
        {
            Debug.Log("first");
            m_Timer += Time.deltaTime;

            if (cutoff >= 1.0f)
            {
                m_Renderer.materials = DefaultMaterials;
                this.enabled = false;
            }
        }
        else
        {
            Debug.Log("second");
            m_Timer -= Time.deltaTime;

            if (cutoff <= minCutoff)
            {
                //m_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                //m_Renderer.materials = DefaultMaterials;
                this.enabled = false;
            }
        }
    }

    void Set(float cutoff)
    {
        renderBounds = m_Renderer.bounds.size;
        pos.y = renderBounds.y;
        m_Renderer.GetPropertyBlock(m_PropertyBlock);
        m_PropertyBlock.SetVector(k_BoundsName, pos);

        m_PropertyBlock.SetFloat(k_CutoffName, cutoff);
        m_Renderer.SetPropertyBlock(m_PropertyBlock);
    }

}
