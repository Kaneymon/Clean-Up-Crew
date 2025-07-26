using UnityEngine;
using UnityEngine.Events;

public class ToggleableMenu : MonoBehaviour, IOpenClosableMenu
{
    public bool menuActiveState { get => gameObject.activeSelf;}
    [SerializeField] UnityEvent openMenuEvent;
    [SerializeField] UnityEvent closeMenuEvent;
    [SerializeField] GameObject TargetObject;
    public void Close()
    {
        gameObject.SetActive(false);
        closeMenuEvent?.Invoke();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        openMenuEvent?.Invoke();
    }

    private void Start()
    {
        //if(TargetObject == gameObject) { Debug.Log("the target object cannot be the object holding this component, make the target a child."); return; }
        if (UiManager.instance != null)
            UiManager.instance.RegisterMenu(this);
    }
    private void OnDestroy()
    {
        if (UiManager.instance != null)
            UiManager.instance.UnregisterMenu(this);
    }

}
