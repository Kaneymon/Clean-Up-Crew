
public interface IOpenClosableMenu
{
    public void Open();    //this will just be triggered by button events, other events, maybe input events or errors etc.
    public void Close();    
    public bool menuActiveState {  get; }
}
