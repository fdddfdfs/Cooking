using UnityEngine;

public abstract class MenuWithView<TView> : Menu  where TView: MonoBehaviour
{
    protected readonly TView _view;
    
    public override bool IsActive => _view.gameObject.activeSelf;

    protected MenuWithView(Canvas canvas, string menuViewResourceName)
    {
        _view = ResourcesLoader.InstantiateLoadComponent<TView>(menuViewResourceName);
        _view.transform.SetParent(canvas.transform, false);
    }

    public override void SetAsLastSibling()
    {
        _view.transform.SetAsLastSibling();
    }

    public override void ChangeMenuActive(bool state, bool affectCursor = true)
    {
        base.ChangeMenuActive(state, affectCursor);
        
        _view.gameObject.SetActive(state);
    }
}