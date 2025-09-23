using UnityEngine;

public class ShielderHurtState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.isHurt = true;
        shielder.PlayAnimation("Hurt",0f);
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Hurt", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                shielder.ChangeState(ShielderState.Idle);
                return;
            }
        }

    }

    public override void Exit()
    {
        shielder.isHurt = false;
    }

}
