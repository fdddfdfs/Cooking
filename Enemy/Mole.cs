using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Mole : MonoBehaviour, ITrappable
{
    public void ActivateTrap()
    {
        Debug.Log("Trapped");
    }
}