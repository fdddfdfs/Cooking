using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class Building
{
    private const float MaxBuildDistance = 100f;
    private const int OverlappedCollidersMaxSize = 10;
    
    private readonly Collider[] _overlappedColliders;
    private readonly LayerMask _layerMask;
    private readonly LayerMask _layerMaskForCollider;
    private readonly Preview _buildingPreview;
    private readonly Camera _playerCamera;
    
    private bool _isBuildable;
    private Material _greenMaterial;
    private Material _redMaterial;
    private GameObject _currentBuilding;
    private Material _currentBuildingMeshMaterial;

    public Building(Camera playerCamera, Material greenMaterial, Material redMaterial)
    {
        _playerCamera = playerCamera;
        _greenMaterial = greenMaterial;
        _redMaterial = redMaterial;
        
        _overlappedColliders = new Collider[OverlappedCollidersMaxSize];
        _layerMask = LayerMask.GetMask("Terrain");
        _layerMaskForCollider = LayerMask.GetMask("Terrain", "Default");
        _buildingPreview = new Preview();
    }

    public void UpdateBuilding(GameObject building)
    {
        if (_currentBuilding == building) return;
        
        _currentBuilding = building;
        _buildingPreview.UpdatePreview(building);
        _currentBuildingMeshMaterial = _buildingPreview.BuildingMesh.material;
    }

    public bool SetBuilding()
    {
        _currentBuilding = null;
        _buildingPreview.BuildingMesh.material = _currentBuildingMeshMaterial;

        return _isBuildable;
    }

    public void ChangeBuildingActive(bool state)
    {
        _currentBuilding.SetActive(state);
    }
    
    public void UpdateBuildableObject()
    {
        Vector3 newPosition = GetRayPosition(_layerMask);
        if (newPosition != default)
        {
            _buildingPreview.Building.transform.position =
                newPosition +
                Vector3.up * _buildingPreview.BuildingCollider.size.y / 2;
            
            _buildingPreview.ChangeActive(true);
            ChangePreviewMesh();
        }
        else
        {
            _buildingPreview.ChangeActive(false);
        }
    }

    private Vector3 GetRayPosition(LayerMask layerMask)
    {
        Ray mouseRay = _playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(mouseRay, out RaycastHit hit, MaxBuildDistance, layerMask) ? hit.point : default;
    }

    private void ChangePreviewMesh()
    {
        if (!IsCollidingWithOtherBuildings(_buildingPreview.BuildingCollider, _layerMaskForCollider))
        {
            ChangeModelMesh(_buildingPreview.BuildingMesh, true);
            _isBuildable = true;
        }
        else
        {
            ChangeModelMesh(_buildingPreview.BuildingMesh, false);
            _isBuildable = false;
        }
    }

    private bool IsCollidingWithOtherBuildings(
        BoxCollider previewCollider,
        LayerMask layerMask,
        Vector3 offset = default)
    {
        Vector3 scale = previewCollider.gameObject.transform.localScale;
        Vector3 colliderSize = previewCollider.size;
        Vector3 size = new(colliderSize.x * scale.x, colliderSize.y * scale.y, colliderSize.z * scale.z);
        int overlapCount = Physics.OverlapBoxNonAlloc(
            previewCollider.transform.TransformPoint(previewCollider.center) + offset,
            size / 2.1f,
            _overlappedColliders,
            previewCollider.gameObject.transform.rotation,
            layerMask);

        if (overlapCount > 0)
        {
            return true;
        }

        return false;
    }
    
    private void ChangeModelMesh(MeshRenderer mesh, bool previewState)
    {
        mesh.material = previewState ? _greenMaterial : _redMaterial;
    }
}