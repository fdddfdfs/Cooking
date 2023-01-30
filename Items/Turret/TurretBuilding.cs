using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurretBuilding : MonoBehaviour
{
    private const float TurretDistance = 30;
    private const float RotationSmoothTime = 0.3f;
    private const float SameRotationTolerance = 5;
    private const int HitCooldownMilliseconds = 5000;

    [SerializeField] private Transform _turretGun;

    private IEnemiesCollection _targets;
    private IEnumerator<GameObject> _targetEnumerator;
    private bool _isEnabled;
    
    private int _enemyLayer;
    private MeshRenderer _turretBarrelMesh;
    private bool _isCooldown;

    private float _rotationVelocity;
    
    public void Init(IEnemiesCollection targets)
    {
        _targets = targets;
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

        if (!isPointedOnTarget || _isCooldown) {Debug.Log("returned"); return;}
        
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
                Hit(target.transform.position);
                Cooldown();
            }
        }
    }

    private void Hit(Vector3 targetPosition)
    {
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
}