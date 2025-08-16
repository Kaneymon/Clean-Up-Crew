using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class StartGameButton : MonoBehaviour, IInteractable //for some reason i cant use networkBehaviours in the main menu scene, i have no clue why. it just makes shit bug out.
{
    [SerializeField] private UnityEvent startGameEventFx;
    private float waitTime = 1.5f;

    public void Interaction(PlayerController player)
    {
        // Request server to start game after delay
        RequestStartGameServer(waitTime);
    }

    //[ServerRpc(RequireOwnership = true)]
    private void RequestStartGameServer(float delay)
    {
        TriggerStartFX(); // fire effects immediately
        StartCoroutine(DelayedStart(delay));
    }

    private IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartGame();
    }

    //[Server]
    private void StartGame()
    {
        string[] scenesToClose = { "MainMenuScene" };
        BootstrapNetworkManager.ChangeNetworkScene("GameScene", scenesToClose);
    }

    //[Server]
    private void TriggerStartFX()
    {
        BroadcastStartFXToClients();
    }

    //[ObserversRpc]
    private void BroadcastStartFXToClients()
    {
        startGameEventFx.Invoke();
    }
}
    