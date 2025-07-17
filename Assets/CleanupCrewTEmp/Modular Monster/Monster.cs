using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    //this base class needs to enable me to easily create a new monster with custom stats and default behaviour which can be modified from the subclass.
    //default monsters need to be 
    // [attackable and killable]
    // [capable of pathing and targeting]
    // [have common senses/detection]
    // [capable of attacking players]
    // [and somehow... have easily assigned state and animations]
    // and all of this needs to be rewritable through protected virtual methods.

    //version 1 will be attackable, killable, have detection, pathing, simple behaviour states.
    [SerializeField] protected float health;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float turnSpeed;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackRate;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float visionRange;
    [SerializeField] protected float hearingRange;

    
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    public virtual void ApplyEffect(string effect)
    {
        // i have no idea how im going to approach adding effects.
    }

    protected virtual void Die()
    {
        Debug.Log("monster died");
        Destroy(gameObject);
    }
}