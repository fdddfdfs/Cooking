using UnityEngine;
using UnityEngine.AI;

public interface IMoleTargetter 
{
    ITargettable GetCurrentTarget();
    Vector3 GetHolePosition();
    NavMeshAgent GetMoleAgent();
    void FindNewTarget();
}
