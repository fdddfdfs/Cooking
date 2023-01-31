using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurretBuilding : MonoBehaviour, IRaycastable
{
    private const float TurretDistance = 30;
    private const float RotationSmoothTime = 0.3f;
    private const float SameRotationTolerance = 5;
    private const int HitCooldownMilliseconds = 5000;
    private const int MaxAmmo = 5;

    [SerializeField] private Transform _turretGun;

    private IEnemiesCollection _targets;
    private TurretView _view;
    private Inventory _inventory;
    private ItemData _ammoData;
    
    private IEnumerator<GameObject> _targetEnumerator;
    private bool _isEnabled;
    private int _enemyLayer;
    private MeshRenderer _turretBarrelMesh;
    
    private bool _isCooldown;
    private int _ammo;

    private float _rotationVelocity;

    public void Init(IEnemiesCollection targets, TurretView view, Inventory inventory, ItemData ammoData)
    {
        _targets = targets;
        _view = view;
        _inventory = inventory;
        _ammoData = ammoData;
        
        _enemyLayer = LayerMask.NameToLayer("Mole");
        _turretBarrelMesh = _turretGun.GetComponentInChildren<MeshRenderer>();
    }

    public void Activate()
    {
        _isEnabled = true;
    }

    private void Update()
    {
        if (!_isEnabled) return;

        GameObject target = GetTarget();

        bool isPointedOnTarget = RotateTurret(target.transform.position);

        if (!isPointedOnTarget || _isCooldown || _ammo == 0) {Debug.Log("returned"); return;}
        
        Vector3 currentPosition = _turretBarrelMesh.transform.position +
                                  _turretBarrelMesh.transform.forward * _turretBarrelMesh.bounds.size.magnitude / 2;
        if (Physics.Raycast(
                currentPosition,
                target.transform.position - currentPosition,
                out RaycastHit hit,
                TurretDistance,
                Physics.AllLayers))
        {
            if (hit.collider.gameObject.layer == _enemyLayer)
            {
                Fire(target.transform.position);
                Cooldown();
            }
        }
    }

    private void Fire(Vector3 targetPosition)
    {
        _ammo -= 1;
        Debug.Log("Hit");
    }

    private async void Cooldown()
    {
        _isCooldown = true;

        await Task.Delay(HitCooldownMilliseconds, OnDisableCancellationToken.Token)
            .ContinueWith(OnDisableCancellationToken.EmptyTask);

        _isCooldown = false;
    }

    private bool RotateTurret(Vector3 targetPosition)
    {
        float targetRotation = Vector3.SignedAngle(
            transform.forward,
            targetPosition - transform.position,
            Vector3.up);
        
        Vector3 currentRotation = _turretGun.transform.localRotation.eulerAngles;
        float rotation = Mathf.SmoothDampAngle(
            currentRotation.y,
            targetRotation,
            ref _rotationVelocity,
            RotationSmoothTime);
        
        _turretGun.transform.localRotation = Quaternion.Euler(currentRotation.x, rotation, currentRotation.z);
        
        return Mathf.Abs(targetRotation - rotation) % 360 < SameRotationTolerance;
    }

    private GameObject GetTarget()
    {
        if (_targetEnumerator == null)
        {
            _targetEnumerator = _targets.GetEnemies().GetEnumerator();
            _targetEnumerator.MoveNext();
        }

        return _targetEnumerator.Current;
    }

    public void Hit()
    {
        _view.ChangeViewActive(true);
        _view.UpdateAmmoCount(_ammo, MaxAmmo);
    }

    public void UnHit()
    {
        _view.ChangeViewActive(false);
    }

    public void Use()
    {
        if (_inventory.RemoveItem(_ammoData, 1))
        {
            _ammo += 1;
        }
    }
}