using UnityEngine;

public class PlayerUltimateSkillState : PlayerStateBase
{
    private float currentChargeTimer;
    private bool skillUsed;
    private enum UltSkillChildState
    {
        ToCharge,
        Charging,
        Casting,
    }
    
    private UltSkillChildState ultSkillChildState;
    private UltSkillChildState ultSkillchildState
    {
        get =>ultSkillChildState;
        set{
            ultSkillChildState = value;
            switch(ultSkillChildState)
            {
                case UltSkillChildState.ToCharge:
                    player.PlayAnimation("UltToCharge",0f);
                    break;
                case UltSkillChildState.Charging:
                    player.PlayAnimation("UltCharging",0f);
                    break;
                case UltSkillChildState.Casting:
                    player.PlayAnimation("UltCast",0f);
                    break;
            }
        }
    }
    
    
    public override void Enter()
    {
        ultSkillchildState = UltSkillChildState.ToCharge;
        skillUsed = false;
        player.isUsingSkill = true;
    }
    
    
    public override void Update()
    {
        switch(ultSkillchildState)
        {
            case UltSkillChildState.ToCharge:
                ToChargeOnUpdate();
                break;
            case UltSkillChildState.Charging:
                ChargeOnUpdate();
                break;
            case UltSkillChildState.Casting:
                CastOnUpdate();
                break;
        }
        
    }

    public override void Exit()
    {
        player.isUsingSkill = false;
    }


    #region 状态内置函数
    
    private void ToChargeOnUpdate()
    {
        if (CheckAnimatorStateName("UltToCharge", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                ultSkillchildState = UltSkillChildState.Charging;
            }
        }
    }

    private void ChargeOnUpdate()
    {
        if (CheckAnimatorStateName("UltCharging", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                ultSkillchildState = UltSkillChildState.Casting;
            }
        }
    }

    private void CastOnUpdate()
    {
        
        if (CheckAnimatorStateName("UltCast", out float animationTime))
        {
            if (animationTime >= 0.4f && !skillUsed)
            {
                player.UseUlt();
                skillUsed = true;
            }
            if (animationTime >= 0.9f)
            {
                player.ChangeState(PlayerState.Idle);
            }
        }
    }
    #endregion
    
    
}
