using UnityEngine;

public class ClassChoiceTube : MonoBehaviour, IInteractable
{
    [SerializeField] DefaultClasses tubeClass;
    public void Interaction(PlayerController player)
    {
        PlayerManager.Instance.PlayerRequestClassChangeServer(player.OwnerId, tubeClass);
    }
}


