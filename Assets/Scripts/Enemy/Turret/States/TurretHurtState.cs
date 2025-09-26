using UnityEngine;

public class TurretHurtState : TurretStateBase
{
    public override void Enter()
    {
        turret.PlayAnimation("Hurt",0f);
    }

    public override void Update()
    {

        if (CheckAnimatorStateName("Hurt", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                turret.ChangeState(TurretState.Idle);
                return;
            }
        }
    }
    
    public override void Exit()
    {

    }

}
