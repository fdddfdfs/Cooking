using UnityEngine;

public abstract class Menu
{
    public abstract bool IsActive { get; }
    
    public virtual void ChangeMenuActive(bool state, bool affectCursor = true)
    {
        if (IsActive == state) return;

        if (!affectCursor) return;
        
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    public abstract void SetAsLastSibling();

    public virtual bool SwapActive()
    {
        ChangeMenuActive(!IsActive);

        return IsActive;
    }
}