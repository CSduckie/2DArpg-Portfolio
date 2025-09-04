using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        player.PlayAnimation("Idle",0f);
        //reset doublejump info
        player.canDoubleJump = true;
        
    }

    public override void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            player.ChangeState(PlayerState.Move);
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ChangeState(PlayerState.Jump);
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.ChangeState(PlayerState.StandAttack);
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
        
        if (!player.IsGroundDetected())
        {
            player.ChangeState(PlayerState.Fall);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && player.checkCanUseSkill(player.slashSkillConfig))
        {
            player.ChangeState(PlayerState.SlashSkill);
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.ChangeState(PlayerState.UltSkill);
            return;
        }
        
        //TODO:需要修改跟能量绑定
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.ChangeState(PlayerState.ShadowCutSkill);
            return;
        }
        
        
        player.UpdateVelocity(false);
    }

    public override void Exit()
    {
        //Debug.Log(player.rb.linearVelocityX);
    }
}
