using UnityEngine;

public class PlayerDefendSuccessfulState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("DefendSuccess",0f);
    }
    public override void Update()
    {
        if (CheckAnimatorStateName("DefendSuccess", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                player.ChangeState(PlayerState.Defend);
                return;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
