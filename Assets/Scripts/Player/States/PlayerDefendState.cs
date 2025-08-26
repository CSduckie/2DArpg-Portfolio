using UnityEngine;

//TODO:使用子状态来实现，用于支持更多功能
public class PlayerDefendState : PlayerStateBase
{
    private enum ChildState
    {
        Idle,
        Trigger,
    }
    
    private ChildState defenseChildState;
    private ChildState DefenseChildState
    {
        get =>defenseChildState;
        set
        {
            defenseChildState = value;
            switch(defenseChildState)
            {
                case ChildState.Idle:
                    player.PlayAnimation("DefenceActive",0f); 
                    break;
                case ChildState.Trigger:
                    player.PlayAnimation("DefendSuccess",0f);
                    break;
            }
        }
    }
    
    
    public override void Enter()
    {
        player.isDefending = true;
        
        //启用完美防御判定
        player.PerfectDefendCheck();
        DefenseChildState = ChildState.Idle;
    }

    public override void Update()
    {
        switch(defenseChildState)
        {
            case ChildState.Idle:
                if (player.triggerDefendAnim)
                {
                    DefenseChildState = ChildState.Trigger;
                    player.triggerDefendAnim = false;
                    return;
                }
                
                //检测切换回Idle的条件
                if (!Input.GetMouseButton(1))
                {
                    player.ChangeState(PlayerState.Idle);
                    return;
                }
                
                player.UpdateVelocity(false);
                break;
            case ChildState.Trigger:
                //检测动画播放是否完毕，如果完毕则回到Idle
                if (CheckAnimatorStateName("DefendSuccess", out float animationTime))
                {
                    if (animationTime >= 0.9f)
                    {
                        DefenseChildState = ChildState.Idle;
                    }
                }
                break;
        }
        
    }
    
    public override void Exit()
    {
        //重置子状态
        DefenseChildState = ChildState.Idle;
        player.isDefending = false;
    }

    public void TriggerDefend()
    {
        if(DefenseChildState == ChildState.Idle)
        {
            DefenseChildState = ChildState.Trigger;
        }
    }
}
