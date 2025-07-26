using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] ToggleableMenu SettingsMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject);}

        openMenus = new Stack<IOpenClosableMenu> ();
        closedMenus = new List<IOpenClosableMenu>();
    }

    // handle closing menus in a centralized way using interfaces and stuff.
    private Stack<IOpenClosableMenu> openMenus;//when the user presses ESC i want to check through each of these, and the moment i encounter one which is open, i will close it, pop it, push it onto the other stack and stop the operation.
    private List<IOpenClosableMenu> closedMenus;
    public List<GameObject> playerFreezingMenus;

    public void RegisterMenu(IOpenClosableMenu menu)
    {
        if (menu == null) { Debug.Log("youve just pushed a null menu mate, ive stopped it."); return; }
        if (openMenus.Contains(menu) || closedMenus.Contains(menu)) { Debug.Log("already registered that one fella."); return; }

        if (menu.menuActiveState == true) { openMenus.Push(menu); }
        else { closedMenus.Add(menu); }
        Debug.Log($"registration of MENU {menu} IS COMPLETE!");
    }

    public void UnregisterMenu(IOpenClosableMenu menu)
    {
        if (menu == null) { Debug.Log("menu is null, cant unregister."); return; }
        if (closedMenus.Contains(menu)) {  closedMenus.Remove(menu); }
        if (openMenus.Contains(menu))
        {
            KanesHelperMethods.RemoveFromStack(openMenus,menu);
        }
    }

    public void CloseTopMenu()
    {
        if (openMenus.TryPop(out IOpenClosableMenu menu) )
        {
            if (menu == null) { Debug.Log("i've just popped a null menu? whats going on ere??"); return; }

            menu.Close();
            closedMenus.Add(menu);
        }
    }

    public void OpenMenu(IOpenClosableMenu menu)
    {
        if (menu == null) { Debug.Log("tried Opening null menu! sort it out mate!"); return; }
        if (openMenus.Contains(menu)) { Debug.Log("this menu is open already! sort it out mate!"); return; }

        if (closedMenus.Contains(menu))
        {
            menu.Open();
            closedMenus.Remove(menu);
            openMenus.Push(menu);
        }
        else
        {
            Debug.Log("Couldnt find menu in closedMenus List, it must be unregistered");
        }
    }

    public bool IsAFreezingMenuOpen()
    {
        bool result = false;
        foreach (var menu in playerFreezingMenus)
        {
            result = result || menu.activeSelf;
        }
        return result;
    }

    public void OpenSettingsMenu()
    {
        OpenMenu(SettingsMenu);
    }
}
