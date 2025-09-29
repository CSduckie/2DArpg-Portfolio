using UnityEngine;

public class HammerGuyHurtState : HammerGuyStateBase
{
    public override void Enter()
    {
        hammerGuy.PlayAnimation("Hurt",0f);
    }
    
    public override void Update()
    {
        if (CheckAnimatorStateName("Hurt", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                hammerGuy.ChangeState(HammerGuyStates.Battle);
                return;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
