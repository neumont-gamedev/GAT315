using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class UVScroll : MonoBehaviour
{
    [SerializeField] Vector2 scrollRate;
    MeshRenderer meshRenderer;
        
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    void Update()
    {
        Vector2 offset = Time.time * scrollRate;
        meshRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}
