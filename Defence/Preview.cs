using UnityEngine;

public class Preview
{
    public GameObject Building { get; private set; }

    public BoxCollider BuildingCollider { get; private set; }
    
    public MeshRenderer BuildingMesh { get; private set; }

    public void UpdatePreview(GameObject building)
    {
        Building = building;
        BuildingCollider = building.GetComponent<BoxCollider>();
        BuildingMesh = building.GetComponent<MeshRenderer>();
    }

    public void ChangeActive(bool state)
    {
        Building.SetActive(state);
    }
}