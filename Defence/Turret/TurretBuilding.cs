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
    private const float FireForce = 10;

    [SerializeField] private Transform _turretGun;

    private IEnemiesCollection _targets;
    private TurretView _view;
    private Inventory _inventory;
    private ItemData _ammoData;
    private Pool<GameObject> _ammoPool;
    
    private IEnumerator<GameObject> _targetEnumerator;
    private bool _isEnabled;
    private int _enemyLayer;
    private MeshRenderer _turretBarrelMesh;
    
    private bool _isCooldown;
    private int _ammo;

    private float _rotationVelocity;

    public void Init(
        IEnemiesCollection targets,
        TurretView view,
        Inventory inventory,
        ItemData ammoData,
        Pool<GameObject> ammoPool)
    {
        _targets = targets;
        _view = view;
        _inventory = inventory;
        _ammoData = ammoData;
        _ammoPool = ammoPool;
        
        _enemyLayer = LayerMask.NameToLayer("Mole");
        _turretBarrelMesh = _turretGun.GetComponentInChildren<MeshRenderer>();
    }

    public void Activate()
    {
        _isEnabled = true;
    }
    
    public void Hit()
    {
        _view.ChangeViewActive(true, _ammo < MaxAmmo);
        _view.UpdateAmmoCount(_ammo, MaxAmmo);
    }

    public void UnHit()
    {
        _view.ChangeViewActive(false, false);
    }

    public void Use()
    {
        if (_ammo == MaxAmmo) return;
        
        if (_inventory.RemoveItem(_ammoData, 1))
        {
            _ammo += 1;
            _view.UpdateAmmoCount(_ammo, MaxAmmo);
        }
    }

    private void Update()
    {
        if (!_isEnabled) return;

        GameObject target = GetTarget();

        bool isPointedOnTarget = RotateTurret(target.transform.position);

        if (!isPointedOnTarget || _isCooldown || _ammo == 0) return;

        Vector3 currentPosition = GetBarrelEndPosition();
        if (Physics.Raycast(
                currentPosition,
                target.transform.position - currentPosition,
                out RaycastHit hit,
                TurretDistance,
                Physics.AllLayers))
        {
            if (hit.collider.gameObject.layer == _enemyLayer)
            {
                Fire(target.transform);
                Cooldown();
            }
        }
    }

    private Vector3 GetBarrelEndPosition()
    {
        Vector3 position = _turretBarrelMesh.transform.position;
        Vector3 offset = _turretBarrelMesh.transform.forward * _turretBarrelMesh.bounds.size.magnitude / 2;

        return position + offset;
    }

    private void Fire(Transform target)
    {
        _ammo -= 1;
        _view.UpdateAmmoCount(_ammo, MaxAmmo);
        GameObject ammo = _ammoPool.GetItem();
        ammo.transform.position = GetBarrelEndPosition();

        Vector3 direction = (target.transform.position - ammo.transform.position).normalized;
        ammo.GetComponent<Rigidbody>().AddForce(direction * FireForce, ForceMode.Impulse);
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
}