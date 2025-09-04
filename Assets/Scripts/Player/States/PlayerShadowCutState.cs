using UnityEngine;

public class PlayerShadowCutState : PlayerStateBase
{
    private enum ShadowCutChildState
    {
        ToCharge,
        Charging,
        Casting,
    }
    
    private ShadowCutChildState shadowCutChildState;
    private ShadowCutChildState shadowCutchildState
    {
        get =>shadowCutChildState;
        set{
            shadowCutChildState = value;
            switch(shadowCutChildState)
            {
                case ShadowCutChildState.ToCharge:
                    player.PlayAnimation("ToShadowCut",0f);
                    break;
                case ShadowCutChildState.Charging:
                    player.PlayAnimation("ShadowCutCharging",0f);
                    break;
                case ShadowCutChildState.Casting:
					//执行一次使用逻辑
                    player.UseShadowCutSkill();
                    player.playerStats.UseSkill(player.shadowCutSkillConfig.releaseData.attackData.hitData.energyCost);
                    shadowCutchildState = ShadowCutChildState.Charging;
                    break;
            }
        }
    }
    
    public override void Enter()
    {
        shadowCutchildState = ShadowCutChildState.ToCharge;
        player.isUsingSkill = true;
    }

    public override void Update()
    {
        switch(shadowCutchildState)
        {
            case ShadowCutChildState.ToCharge:
                ToChargeOnUpdate();
                break;
            case ShadowCutChildState.Charging:
                ChargeOnUpdate();
                break;
            case ShadowCutChildState.Casting:
                CastOnUpdate();
                break;
        }

        if (!player.checkCanUseSkill(player.shadowCutSkillConfig))
        {
            player.ChangeState(PlayerState.Idle);
        }
    }
    
    public override void Exit()
    {
        player.isUsingSkill = false;
    }
    
    
    
    
    #region 状态内置函数

    private void ToChargeOnUpdate()
    {
        if (CheckAnimatorStateName("ToShadowCut", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                shadowCutchildState = ShadowCutChildState.Charging;
            }
        }
        //检测，如果没有按下按键就转换为idle
        if (Input.GetKeyUp(KeyCode.Q))
        {
            player.ChangeState(PlayerState.Idle);
        }
    }

    private void ChargeOnUpdate()
    {
		if (CheckAnimatorStateName("ShadowCutCharging", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                shadowCutchildState = ShadowCutChildState.Casting;
            }
        }
        //检测，如果没有按下按键就转换为idle
        if (Input.GetKeyUp(KeyCode.Q))
        {
            player.ChangeState(PlayerState.Idle);
        }
    }

    private void CastOnUpdate()
    {
        
    }
    
    
    #endregion
}
