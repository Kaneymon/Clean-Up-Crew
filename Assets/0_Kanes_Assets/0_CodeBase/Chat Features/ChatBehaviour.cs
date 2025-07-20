using UnityEngine;
using FishNet.Object;
using UnityEngine.UI;
using TMPro;
using Steamworks;
public class ChatBehaviour : NetworkBehaviour
{
    //i need dis/enable chatbox function.
    //i need function to set up typing in chatbox.
    //i need event to send message as broadcast.
    //i need event listener the receive message.
    //i need the message to show in the chat when received.

    [SerializeField] GameObject messagePrefab;
    [SerializeField] TMP_InputField chatInput;
    [SerializeField] GameObject chatBox;
    [SerializeField] GameObject messagesGrid;

    private bool isOpen = false;

    public static ChatBehaviour instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        chatBox.SetActive(false);
    }

    public bool GetChatBehaviourAvailable()
    {
        return enabled;
    }

    public void OpenChatBox()
    {
        chatBox.SetActive(true);
        chatInput.gameObject.SetActive(true);
        chatInput.text = "";
        ActivateTyping();
        isOpen = true;
    }

    public void CloseChatBox()
    {
        chatBox.SetActive(false);
        chatInput.gameObject.SetActive(false);
        isOpen = false;
    }

    public bool GetIsChatBoxOpen()
    {
        return isOpen;
    }

    public void GlimpseMessagesGridOnly()
    {
        //this should be seperate from the prexisting chat box.
        //i need a "Glimpse" box which only shows 1 text message element, temporaily.
        //the glimpse box is a 1 element queue.
    }

    private void ActivateTyping()
    {
        chatInput.ActivateInputField();
    }

    public void TrySendMessage()
    {
        if (chatBox.activeSelf == false) { return; }
        if (string.IsNullOrEmpty(chatInput.text) ) { return;}

        string message = SteamFriends.GetPersonaName().ToString() + ": " + chatInput.text;
        Debug.Log(SteamFriends.GetPersonaName().ToString() + ": " + "sending this message to observers LOCAL: " + message);
        SendMessageAllServer(message);
        chatInput.text = "";
        chatInput.ActivateInputField();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendMessageAllServer(string message)
    {
        Debug.Log(SteamFriends.GetPersonaName().ToString() + ": " + "sending this message to observers: " + message);
        BroadcastMessageObserversRpc(message);
    }

    [ObserversRpc]
    private void BroadcastMessageObserversRpc(string message)
    {
        Debug.Log(SteamFriends.GetPersonaName().ToString() + ": " + "Received message: " + message);
        SpawnNewMessageObject(message);
    }

    public void SpawnNewMessageObject(string messageText)
    {
        //if(!IsOwner) return;
        Debug.Log("spawning message instance");
        GameObject newMsg = Instantiate(messagePrefab, messagesGrid.transform);
        newMsg.GetComponent<TMP_Text>().text = messageText;
    }
}
