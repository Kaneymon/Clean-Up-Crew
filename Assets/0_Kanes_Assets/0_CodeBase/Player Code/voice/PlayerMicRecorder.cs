using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMicRecorder : MonoBehaviour
{
    [SerializeField] SoundSettingsProfile profile;
    enum e_MicResult //these will be used as states for helping us control our class behaviour.
    {
        RecordingOK,
        RecordingNoData,
        RecordingRestricted,
        RecordingNotRecording,
        RecordingNotInitialized
    }
    e_MicResult recordingState;

    [Serializable]
    public class ByteArrayEvent : UnityEvent<byte[]>
    { }

    [Range(0f, 1f)]
    public float bufferLength = 0.25f;

    //occurs every frame where the playerMicRecorder has an available voice payload to give.
    public ByteArrayEvent eventVoiceStream;

    [ReadOnly(true)]
    [SerializeField]
    private bool isRecording = false;
    private string selectedMicrophone;
    AudioClip micClip;
    private float packetCounter = 0;
    public bool IsRecording = false;


    //functionality needed:
    //start recording.
    //stop recording.
    //check mic state.
    //convertClipTobyteBuffer

    private void Update()
    {
        UpdateRecordingState();
        HandleMicrophoneData();
    }

    public void StartRecording()
    {
        
        if (profile.microphoneDeviceIndex < 0 || profile.microphoneDeviceIndex >= Microphone.devices.Length)
        {
            Debug.LogError("Invalid microphone index");
            return;
        }
        
        selectedMicrophone = Microphone.devices[profile.microphoneDeviceIndex];
        packetCounter = bufferLength;
        IsRecording = true;

        micClip = Microphone.Start(selectedMicrophone, true, 1, profile.micSampleRate);
        lastClipheadTime = Microphone.GetPosition(selectedMicrophone);


        if (micClip.frequency != profile.micSampleRate)
        {
            Debug.LogWarning($"Mic sample rate mismatch! Requested: {profile.micSampleRate}, Got: {micClip.frequency}");
            //some systems wont honor the requested sample rate, so this is here to alert of a potential reason for issues down the lines.
        }
    }

    public void StopRecording()
    {
        IsRecording = false;
        Microphone.End(selectedMicrophone);
    }

    int lastClipheadTime = 0;
    int currentClipheadTime = 0;
    
    private void HandleMicrophoneData()
    {
        if (!IsRecording) { return; }

        packetCounter -= Time.unscaledDeltaTime;
        if (packetCounter >= 0) { return; }
        packetCounter = bufferLength;
        Debug.Log(recordingState.ToString() + " " + recordingState);
        switch (recordingState)
        {
            
            case e_MicResult.RecordingOK:
                //pull data from the audio clip using GetData()
                //convert data into a PCM byte[] array.
                //pass data through to whatever wants it by invoking voiceStream event.
                currentClipheadTime = Microphone.GetPosition(selectedMicrophone);
                int sampleCount = (currentClipheadTime - lastClipheadTime + micClip.samples) % micClip.samples;
                if (sampleCount > 0)
                {
                    float[] buffer = new float[sampleCount];
                    micClip.GetData(buffer, lastClipheadTime); // read from last read position to current read position
                    lastClipheadTime = currentClipheadTime;

                    byte[] networkbuffer = KanesHelperMethods.FloatToPCM16(buffer);

                    //I MUST LOOK INTO COMPRESSION FOR BYTE ARRAYS.
                    //THE EASIEST OPTION IS UZING GZIP APPARENTLY FOR 2:1 GAINS.
                    //ALSO THERE IS OPUSDOTNET WHICH IS 16:1 GAINS. ILL TEST EACH ONE INDIVIDUALLY.

                    eventVoiceStream.Invoke(networkbuffer);
                    // process buffer (send over network, etc.)
                }
                break;
            case e_MicResult.RecordingNoData:

                break;
            case e_MicResult.RecordingRestricted:
                break;
            case e_MicResult.RecordingNotRecording:
                break;
            case e_MicResult.RecordingNotInitialized:
                break;
            default:
                break;
        }
    }

    private void UpdateRecordingState()
    {
        if (IsRecording && Microphone.IsRecording(selectedMicrophone))
        {
            recordingState = e_MicResult.RecordingOK;
        }
        else if (IsRecording && !Microphone.IsRecording(selectedMicrophone))
        {
            recordingState = e_MicResult.RecordingNotRecording;
        }
        else if (micClip == null)
        {
            recordingState = e_MicResult.RecordingNotInitialized;
        }
        else if (micClip.length == 0)
        {
            recordingState = e_MicResult.RecordingNoData;
        }
        else
        {
            recordingState = e_MicResult.RecordingRestricted; // this is supposed to be used for chatbans but im just gonna use it as a debugging case for now.
        }
    }
}
