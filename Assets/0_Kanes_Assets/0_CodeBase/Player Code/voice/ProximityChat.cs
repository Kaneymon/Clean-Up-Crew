using FishNet.Object;
using FishNet.Transporting;
using Heathen.SteamworksIntegration;
using UnityEngine;
using UnityEngine.tvOS;

public class ProximityChat : NetworkBehaviour
{
    [SerializeField] VoiceDataStream VoiceDataStream;

    private void Awake()
    {

    }

    public void SendVoiceData(byte[] data)
    {
        Debug.Log("sending voice data local");
        SendVoiceDataServer(data);
    }

    [ServerRpc]
    private void SendVoiceDataServer(byte[] data, Channel channel = Channel.Unreliable)
    {
        Debug.Log("server received voice data, sending global");
        ReceiveVoiceData(data);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void ReceiveVoiceData(byte[] data)
    {
        Debug.Log("voice data received,           playing audio.");
        //play audio
    }
}
