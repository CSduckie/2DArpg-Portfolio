using UnityEngine;

public class PlayerMoveState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("Run",0f);
    }

    public override void Update()
    {
        if (player.inputDir == 0)
        {
            player.ChangeState(PlayerState.Idle);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ChangeState(PlayerState.Jump);
            return;
        }

        if (!player.IsGroundDetected())
        {
            player.ChangeState(PlayerState.Fall);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.ChangeState(PlayerState.Dash);
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && player.checkCanUseSkill(player.healSkillConfig))
        {
            player.ChangeState(PlayerState.HealSkill);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.ChangeState(PlayerState.StandAttack);
            return;
        }
        
        player.UpdateVelocity(true);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
