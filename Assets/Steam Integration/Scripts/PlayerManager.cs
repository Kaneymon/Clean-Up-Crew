using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Transporting;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private Dictionary<int, PlayerData> players = new Dictionary<int, PlayerData>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void RegisterPlayer(int connectionId, PlayerData data)
    {
        if (!players.ContainsKey(connectionId))
        {
            players.Add(connectionId, data);
            print("player: " + data.playerName + " is _ Id: " + data.connectionId);
        }
    }

    public void UnregisterPlayer(int connectionId)
    {
        players.Remove(connectionId);
    }

    public PlayerData GetPlayer(int connectionId)
    {
        return players.TryGetValue(connectionId, out var data) ? data : null;
    }

    public IEnumerable<PlayerData> GetAllPlayers() => players.Values;

    [ServerRpc(RequireOwnership = false)]
    public void PlayerRequestClassChangeServer(int connectionId, DefaultClasses newClassChoice, NetworkConnection sender = null)
    {
        CharacterClass newClass = CharacterClass.EnumToCharacterClass(newClassChoice);

        if (IsClassOccupied(newClass)) { Debug.Log($"{newClass.Name} is an occupied class!"); return; }

        if (players.TryGetValue(connectionId, out var player))
        {
            player.classChoice = newClass;
            //player.controller.SetPlayerClass(newClassChoice);
            Debug.Log($"Set class of player {connectionId} to {newClass.Name}");
            return;
        }
        else
        {
            Debug.LogWarning($"Player {connectionId} not found.");
        }
    }

    public bool IsClassOccupied(CharacterClass characterClass)
    {
        bool isOccupied = false;
        foreach (var player in players.Values)
        {
            if(player.classChoice == characterClass) {isOccupied = true;}
        }
        return isOccupied;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.X))
        {
            foreach (KeyValuePair<int, PlayerData> entry in players)
            {
                Debug.Log($"[PlayerManager] ID: {entry.Key}, Name: {entry.Value.playerName}, Class: {entry.Value.classChoice.Name}, Controller: {entry.Value.controller}");
            }
        }
    }
}
