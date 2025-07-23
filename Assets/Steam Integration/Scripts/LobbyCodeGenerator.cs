using Steamworks;
using UnityEngine;

public static class LobbyCodeGenerator
{

    private const string Alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    private const char PaddingChar = '0';

    public static string UlongToLobbyCode(ulong SteamIDLobby, int length = 11)//now i cant recall if this works from the left or the right of CSteamID. we need it to work from the right as its more random.
    {
        Debug.Log($"converting ulong {SteamIDLobby}");

        var sb = new System.Text.StringBuilder();
        while (SteamIDLobby > 0)
        {
            int index = (int)(SteamIDLobby % (ulong)Alphabet.Length);
            sb.Insert(0, Alphabet[index]);
            SteamIDLobby /= (ulong)Alphabet.Length;
        }

        //string result = sb.ToString().PadLeft(length, PaddingChar);
        string result = sb.ToString();

        Debug.Log($" to lobby code {result}");
        return result;
    }

    public static ulong LobbyCodeToUlong(string code)
    {
        code = code.TrimStart(PaddingChar); // remove any padding

        ulong result = 0;
        foreach (char c in code)
        {
            result *= (ulong)Alphabet.Length;
            result += (ulong)Alphabet.IndexOf(c);
        }

        Debug.Log($"converting lobby code {code} to ulong {result}");
        return result;
    }

//to half the size of the lobby CODES. i could probably truncate the first 9 numbers, they dont seem to change often. i read somewhere that only the last 32 bits of a ulong are actually used? 
//i cant find the resources to help me understand what i can and cant do with these CsteamID's so ill have to settle for 12 chars.

}
