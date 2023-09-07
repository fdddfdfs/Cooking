using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplosiveAmmo : MonoBehaviour
{
    private const float ExplosionForce = 50;
    private const float ExplosionRadius = 10;
    private const int ExplosionHideTimeMilliseconds = 5000;
    
    [SerializeField] private List<Rigidbody> _parts;

    private List<StartTransform> _startTransforms;
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();    
        
        _startTransforms = new List<StartTransform>();
        foreach (Rigidbody part in _parts)
        {
            Transform partTransform = part.transform;
            _startTransforms.Add(new StartTransform(partTransform.localPosition, partTransform.localRotation));
        }   
    }
    
    private void Start()
    {
        ChangeActive(false);
    }

    private void Explode()
    {
        ChangeActive(true);
        _rigidbody.isKinematic = true;
        
        foreach (Rigidbody part in _parts)
        {
            part.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
        }

        _rigidbody.isKinematic = false;
        HideInTime();
    }

    private async void HideInTime()
    {
        CancellationToken token = OnDisableCancellationToken.Token;
        
        await Task.Delay(ExplosionHideTimeMilliseconds, token)
            .ContinueWith(OnDisableCancellationToken.EmptyTask);

        if (token.IsCancellationRequested) return;
        
        Hide();
    }

    private void Hide()
    {
        ChangeActive(false);
        
        for (var i = 0; i < _parts.Count; i++)
        {
            Transform partTransform = _parts[i].transform;
            StartTransform partStartTransform = _startTransforms[i];
            
            partTransform.localPosition = partStartTransform.LocalPosition;
            partTransform.localRotation = partStartTransform.LocalRotation;
        }

        gameObject.SetActive(false);
    }

    private void ChangeActive(bool active)
    {
        foreach (Rigidbody part in _parts)
        {
            part.isKinematic = !active;
            part.detectCollisions = active;
        }
    }
    
    private struct StartTransform
    {
        public readonly Vector3 LocalPosition;
        public readonly Quaternion LocalRotation;

        public StartTransform(Vector3 localPosition, Quaternion localRotation)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
        }
    }

    private void OnCollisionEnter(Collision _)
    {
        Explode();
    }
}