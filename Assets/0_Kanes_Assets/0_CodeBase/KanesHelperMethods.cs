using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO.Compression;
using System.IO;

public static class KanesHelperMethods
{
    //====================================== [STACK HELPERS] =============================
    public static void RemoveAllFromStack<T>(Stack<T> stack, Predicate<T> match)
    {
        Stack<T> tempStack = new Stack<T>();
        while (stack.Count > 0)
        {
            T current = stack.Pop();
            if (!match(current))
            {
                tempStack.Push(current);
            }
        }

        while (tempStack.Count > 0)
        {
            stack.Push(tempStack.Pop());
        }
    }

    public static void RemoveFromStack<T>(Stack<T> stack, T target)
    {
        Stack<T> tempStack = new Stack<T>();
        while (stack.Count > 0)
        {
            T current = stack.Pop();
            if (!EqualityComparer<T>.Default.Equals(current, target))
            {
                tempStack.Push(current);
            }
        }

        while (tempStack.Count > 0)
        {
            stack.Push(tempStack.Pop());
        }
    }


    //================ [DATA CONVERSION FOR NETWORKING] ====================================
    // Convert float (-1.0f to 1.0f) to 16-bit PCM byte array
    public static byte[] FloatToPCM16(float[] samples)
    {
        byte[] bytes = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short val = (short)(Mathf.Clamp(samples[i], -1f, 1f) * short.MaxValue);
            bytes[i * 2] = (byte)(val & 0xff);
            bytes[i * 2 + 1] = (byte)((val >> 8) & 0xff);
        }
        return bytes;
    }

    // Convert 16-bit PCM byte array back to float
    public static float[] PCM16ToFloat(byte[] bytes)
    {
        int len = bytes.Length / 2;
        float[] samples = new float[len];
        for (int i = 0; i < len; i++)
        {
            short val = (short)(bytes[i * 2] | (bytes[i * 2 + 1] << 8));
            samples[i] = val / 32768f;
        }
        return samples;
    }

    public static byte[] Compress(byte[] data)
    {
        using (var output = new MemoryStream())
        {
            using (var gzip = new System.IO.Compression.GZipStream(output, CompressionMode.Compress))
            {
                gzip.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }
    }

}
