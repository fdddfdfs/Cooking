using System;
using UnityEngine;

public class LastStagePlant : MonoBehaviour
{
    private MeshRenderer _mesh;
    private Material _defaultMaterial;

    public void ChangeMaterial(Material newMaterial)
    {
        _mesh.material = newMaterial ? newMaterial : _defaultMaterial;
    }

    private void Awake()
    {
        _mesh = GetComponentInChildren<MeshRenderer>();

        if (!_mesh)
        {
            throw new Exception("Cannot find MeshRenderer component");
        }
        
        _defaultMaterial = _mesh.material;
    }

    private void OnDisable()
    {
        _mesh.material = _defaultMaterial;
    }
}