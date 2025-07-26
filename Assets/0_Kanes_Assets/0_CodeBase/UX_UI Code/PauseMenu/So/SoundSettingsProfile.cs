using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettingsProfile", menuName = "Scriptable Objects/SoundSettingsProfile")]
public class SoundSettingsProfile : ScriptableObject
{
    //this will be how i store players settings profiles.
    //scriptable objects can be easily Serialized into JSON.

    [Header("Audio")]
    public int microphoneDeviceIndex = 0;
    public float masterVolume = 0.8f;
    public float musicVolume = 0.8f;
    public float sfxVolume = 1f;
    public int micSampleRate = 16000;
    public bool pushToTalk = false;

    //[Tooltip("Optional override for audio output device name (if supported by plugin)")]
    //public string outputDeviceName;  this will reqcuire some kind of plugin to do, so lets shelve it for now and disable that setting input.


    //============ TODO!!!
    // i need to add some kind of event that triggers when the scriptable object is changed. so classes like PlayerMicRecorder can subscribe to it and refresh their microphone device.
}
