using FishNet.Object;
using UnityEngine;

public class ClassChoiceTube : NetworkBehaviour, IInteractable
{
    [SerializeField] DefaultClasses tubeClass;
    public void Interaction(PlayerController player)
    {
        PlayerManager.Instance.PlayerRequestClassChangeServer(player.OwnerId, tubeClass);
    }
}


