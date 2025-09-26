using UnityEngine;

public class HammerGuyAttackState : HammerGuyStateBase
{
    public override void Enter()
    {
        hammerGuy.rb.linearVelocity = Vector2.zero;
        hammerGuy.PlayAnimation("Attack",0f);
        hammerGuy.canAttack = false;
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Attack", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                hammerGuy.ChangeState(HammerGuyStates.Battle);
                return;
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public override void Exit()
    {
        hammerGuy.CoolAttack();
    }

}
