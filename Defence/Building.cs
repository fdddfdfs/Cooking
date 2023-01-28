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
    
    private bool _isOnBuilding;
    private Material _greenMaterial;
    private Material _redMaterial;

    public Building(Camera playerCamera, Material greenMaterial, Material redMaterial, GameObject building)
    {
        _playerCamera = playerCamera;
        _greenMaterial = greenMaterial;
        _redMaterial = redMaterial;
        
        _overlappedColliders = new Collider[OverlappedCollidersMaxSize];
        _layerMask = LayerMask.GetMask("Terrain");
        _layerMaskForCollider = -1;
        _buildingPreview = new Preview();
        _buildingPreview.UpdatePreview(building);
    }
    
    public void UpdateBuildableObject()
    {
        Vector3 newPosition = GetRayPosition(_layerMask);
        if (newPosition != default)
        {
            _buildingPreview.Building.transform.position =
                newPosition + 
                Vector3.up * _buildingPreview.BuildingCollider.size.y/2;
            
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
            _isOnBuilding = true;
        }
        else
        {
            ChangeModelMesh(_buildingPreview.BuildingMesh, false);
            _isOnBuilding = false;
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
        
        if (overlapCount > 1)
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