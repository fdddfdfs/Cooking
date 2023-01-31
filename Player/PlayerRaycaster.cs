using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRaycaster : IUpdatable
{
    private readonly Camera _camera;

    private Collider _previousHit;
    private IRaycastable _previousRaycastable;
    
    public PlayerRaycaster(Camera camera)
    {
        _camera = camera;
    }


    public void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (_previousHit != hit.collider)
            {
                _previousRaycastable?.UnHit();
                
                _previousHit = hit.collider;
                _previousRaycastable = hit.collider.gameObject.GetComponent<IRaycastable>();
                
                _previousRaycastable?.Hit();
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                _previousRaycastable?.Use();
            }
        }
    }
}