using System.Collections.Generic;
using UnityEngine;

public class VoiceDataStream : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource outputSource;
    public SampleRateMethod sampleRateMethod = SampleRateMethod.Native;

    [Range(11025, 48000)]
    public uint customSampleRate = 28000;

    [Range(0f, 3f)]
    public float playbackDelay = 0.25f;

    // Internal state
    private int sampleRate;
    private Queue<float> audioBuffer = new Queue<float>(48000);
    private AudioClip microphoneClip;
    private string microphoneDevice;
    private int lastMicPosition = 0;

    private void Start()
    {
        InitializeSampleRate();
        InitializeMicrophone();
        InitializeAudioOutput();
        PrebufferSilence();
    }

    private void Update()
    {
        if (!Microphone.IsRecording(microphoneDevice) || microphoneClip == null)
            return;

        ProcessNewMicrophoneSamples();
    }

    private void InitializeSampleRate()
    {
        if (sampleRateMethod == SampleRateMethod.Native)
        {
            sampleRate = AudioSettings.outputSampleRate;
        }
        else if (sampleRateMethod == SampleRateMethod.Custom)
        {
            sampleRate = (int)customSampleRate;
        }
        else
        {
            sampleRate = 44100; // Fallback
        }
    }

    private void InitializeMicrophone()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone devices found.");
            return;
        }

        //microphoneDevice = Microphone.devices[11];
        microphoneClip = Microphone.Start(microphoneDevice, true, 1, sampleRate);
        lastMicPosition = 0;
    }

    private void InitializeAudioOutput()
    {
        if (outputSource == null)
        {
            Debug.LogError("No AudioSource assigned for output.");
            return;
        }

        outputSource.loop = true;
        outputSource.clip = AudioClip.Create(
            "MicPlayback",
            sampleRate * 2,
            1,
            sampleRate,
            true,
            OnAudioRead
        );

        //outputSource.Play();
    }

    private void PrebufferSilence()
    {
        if (playbackDelay <= 0f)
            return;

        int silentSamples = Mathf.CeilToInt(sampleRate * playbackDelay);
        for (int i = 0; i < silentSamples; i++)
        {
            audioBuffer.Enqueue(0f);
        }
    }

    private void ProcessNewMicrophoneSamples()
    {
        int currentMicPosition = Microphone.GetPosition(microphoneDevice);

        // Handle mic loop-around
        if (currentMicPosition < lastMicPosition)
        {
            lastMicPosition = 0;
        }

        int newSamples = currentMicPosition - lastMicPosition;
        if (newSamples <= 0)
            return;

        float[] micData = new float[newSamples];
        microphoneClip.GetData(micData, lastMicPosition);

        foreach (float sample in micData)
        {
            audioBuffer.Enqueue(sample);
        }

        lastMicPosition = currentMicPosition;
    }

    private void OnAudioRead(float[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (audioBuffer.Count > 0)
            {
                data[i] = audioBuffer.Dequeue();
            }
            else
            {
                data[i] = 0f;
            }
        }
    }
}

public enum SampleRateMethod
{
    Optimal, // Placeholder only
    Native,
    Custom
}
