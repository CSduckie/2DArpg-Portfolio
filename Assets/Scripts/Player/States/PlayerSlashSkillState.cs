using UnityEngine;

public class PlayerSlashSkillState : PlayerStateBase
{
    //计时器，用于计算角色三蓄时间
    private float currentChargeTimer;
    private enum SlashSkillChildState
    {
        ToCharge,
        Charging,
        Slash
    }
    
    private SlashSkillChildState slashChildState;
    private SlashSkillChildState slashchildState
    {
        get =>slashChildState;
        set{
            slashChildState = value;
            switch(slashChildState)
            {
                case SlashSkillChildState.ToCharge:
                    player.PlayAnimation("ToCharge",0f);
                    break;
                case SlashSkillChildState.Charging:
                    //进入蓄力阶段就自动消耗一次
                    player.currentChargeStage = 1;
                    player.playerStats.UseSkill(player.slashSkillConfig.releaseData.attackData.hitData.energyCost);
                    player.PlayAnimation("Charging",0f);
                    break;
                case SlashSkillChildState.Slash:
                    player.PlayAnimation("Slash",0f);
                    break;
            }

        }
    }
    public override void Enter()
    {
        currentChargeTimer = 0;
        player.rb.linearVelocity = Vector2.zero;
        slashchildState = SlashSkillChildState.ToCharge;
        player.isUsingSkill = true;
    }
    
    public override void Update()
    {
        switch(slashchildState)
        {
            case SlashSkillChildState.ToCharge:
                ToChargeOnUpdate();
                break;
            case SlashSkillChildState.Charging:
                ChargeOnUpdate();
                break;
            case SlashSkillChildState.Slash:
                SlashOnUpdate();
                break;
        }
    }

    private void ToChargeOnUpdate()
    {
        if (CheckAnimatorStateName("ToCharge", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                slashchildState = SlashSkillChildState.Charging;
            }
        }
    }

    private void ChargeOnUpdate()
    {
        //如果充能段数达到最大，或者是当前的能量已经不足以继续充能了
        if (player.currentChargeStage >= 3 || !player.checkCanUseSkill(player.slashSkillConfig))
        {
            slashchildState = SlashSkillChildState.Slash;
            return;
        }

        //玩家主动停止充能
        if (Input.GetKeyUp(KeyCode.E))
        {
            slashchildState = SlashSkillChildState.Slash;
            return;
        }
        
        
        if (currentChargeTimer < player.slashChargeTime)
        {
            currentChargeTimer += Time.deltaTime;
        }
        else
        {
            currentChargeTimer = 0;
            player.playerStats.UseSkill(player.slashSkillConfig.releaseData.attackData.hitData.energyCost);
            player.currentChargeStage++;
        }
        
    }

    private void SlashOnUpdate()
    {
        if (CheckAnimatorStateName("Slash", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                player.ChangeState(PlayerState.Idle);
            }
        }
    }
    
    public override void Exit()
    {
        player.isUsingSkill = false;
        player.currentChargeStage = 0;
    }
}
