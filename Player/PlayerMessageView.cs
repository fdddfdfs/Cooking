using TMPro;
using UnityEngine;

public class PlayerMessageView : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageText;

    public void SetText(string text)
    {
        _messageText.text = text;
    }

    public void ClearText()
    {
        _messageText.text = string.Empty;
    }
}