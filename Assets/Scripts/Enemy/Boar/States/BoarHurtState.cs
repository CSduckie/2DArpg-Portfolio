using UnityEngine;

public class BoarHurtState : BoarStateBase
{
    public override void Enter()
    {
        //boar.rb.linearVelocity = Vector2.zero;
        boar.PlayAnimation("Hurt",0f);
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Hurt", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                boar.ChangeState(BoarState.Patrol);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        boar.isHurt = false;
    }
}
