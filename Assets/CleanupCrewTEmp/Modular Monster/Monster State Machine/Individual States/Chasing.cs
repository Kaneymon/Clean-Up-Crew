using UnityEngine;
using FishNet.Object;
public class Chasing : NetworkBehaviour, IMonsterState
{

    public void Enter(Monster monster)
    {
        //set player as target
    }

    public void BehaviourUpdate(Monster monster)
    {
        //check line of sight, and other senses to see if the player target pos can be refreshed or not.
    }

    public void Exit(Monster monster)
    {
        
    }
}