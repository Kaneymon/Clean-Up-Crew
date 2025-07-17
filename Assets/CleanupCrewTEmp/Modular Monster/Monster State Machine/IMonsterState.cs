public interface IMonsterState
{
    void Enter(Monster monster);
    void BehaviourUpdate(Monster monster);
    void Exit(Monster monster);
}