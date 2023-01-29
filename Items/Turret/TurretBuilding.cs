using UnityEngine;

public class TurretBuilding : MonoBehaviour
{
    private const float TurretDistance = 30;
    private const float RotationSmoothTime = 0.3f;
    private const float SameRotationTolerance = 5;

    [SerializeField] private Transform _turretGun;

    private IEnemiesCollection _targets;
    private bool _isEnabled;
    private int _enemyLayer;

    private float _rotationVelocity;
    
    public void Init(IEnemiesCollection targets)
    {
        _isEnabled = true;
        _targets = targets;
        _enemyLayer = LayerMask.GetMask("Mole");
    }

    private void Update()
    {
        if (!_isEnabled) return;

        GameObject target = GetTarget();

        bool isPointedOnTarget = RotateTurret(target.transform.position);

        if (!isPointedOnTarget) return;

        Vector3 currentPosition = transform.position;
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
            }
        }
    }

    private void Hit(Vector3 targetPosition)
    {
        
    }

    private bool RotateTurret(Vector3 targetPosition)
    {
        float targetRotation = Vector3.SignedAngle(transform.position, targetPosition, Vector3.up);
        
        float rotation = Mathf.SmoothDampAngle(
            transform.eulerAngles.x,
            targetRotation,
            ref _rotationVelocity,
            RotationSmoothTime);

        Vector3 currentRotation = _turretGun.transform.localRotation.eulerAngles;
        _turretGun.transform.localRotation = Quaternion.Euler(currentRotation.x, rotation, currentRotation.z);

        return targetRotation - rotation < SameRotationTolerance;
    }

    private GameObject GetTarget()
    {
        return _targets.GetEnemies().GetEnumerator().Current;
    }
}