using TMPro;
using UnityEngine;

public class TurretView : MonoBehaviour
{
    [SerializeField] private TMP_Text _hitText;
    [SerializeField] private TMP_Text _currentAmmo;

    public void UpdateAmmoCount(int count, int maxCount)
    {
        _currentAmmo.text = $"{count.ToString()}/{maxCount.ToString()}";
    }

    public void ChangeViewActive(bool active)
    {
        _hitText.gameObject.SetActive(active);
        _currentAmmo.gameObject.SetActive(active);
    }
}