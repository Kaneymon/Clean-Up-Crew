using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using FishySteamworks;
using Heathen.SteamworksIntegration;
using Steamworks;
using UnityEngine;

public class ProximityChat : NetworkBehaviour
{
    //voiceRecorder.
    [SerializeField] PlayerMicRecorder playerMicRecorder;
    [SerializeField] VoiceStream voiceStream;
    //need something like byte[] voice = voiceDataStream.getVoiceAudioData()
    //private bool pushToTalk = false; add this to sound settings.
    private bool pushToTalk = false;

    private void FixedUpdate()
    {
        if (pushToTalk)
        {
            //if recording already, return
            //voiceRecorder.startrecording
        }
        else
        {
            //voicerecorder.stop recording
        }
    }

    private void Start()
    {
        Debug.Log("start function");
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            Debug.Log("OnStartClient called, I am the owner.");
            playerMicRecorder.StartRecording();
        }
    }


    //hook this up to an event that pass through a byte[] array.
    public void SendVoiceData(byte[] data)
    {
        //Debug.Log("sending voice data local");
        SendVoiceDataServer(data);
    }

    [ServerRpc]
    private void SendVoiceDataServer(byte[] data, Channel channel = Channel.Unreliable)
    {
        //Debug.Log("server received voice data, sending global");
        ReceiveVoiceData(data);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void ReceiveVoiceData(byte[] data)
    {
        //Debug.Log("voice data received, playing audio.");
        voiceStream.PlayVoiceData(data);
    }



}
