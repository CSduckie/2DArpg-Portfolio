using UnityEngine;

public class AssassinAppearState : AssassinStateBase
{
    public override void Enter()
    {
        assassin.PlayAnimation("Appear",0f);
    }
    public override void Update()
    {
        if (CheckAnimatorStateName("Appear", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                assassin.ChangeState(AssassinStates.Idle);
                return;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
