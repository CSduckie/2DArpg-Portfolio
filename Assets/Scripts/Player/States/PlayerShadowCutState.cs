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
                    player.PlayAnimation("",0f);
                    break;
                case ShadowCutChildState.Charging:
                    player.PlayAnimation("",0f);
                    break;
                case ShadowCutChildState.Casting:
                    player.PlayAnimation("",0f);
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

    }
    
    public override void Exit()
    {
        player.isUsingSkill = false;
    }
    
    
    
    
    #region 状态内置函数

    private void ToChargeOnUpdate()
    {
        
    }

    private void ChargeOnUpdate()
    {
        
        
    }

    private void CastOnUpdate()
    {
        
    }
    
    
    #endregion
}
