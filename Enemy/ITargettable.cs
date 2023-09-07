using UnityEngine;

public interface ITargettable
{
    Vector3 Position { get; }
    void TakeDamage(int damage);
}
