using UnityEngine;

public class PlayerStandAttackState : PlayerStateBase
{
    //当前的普攻段数
    private int currentAttackIndex;
    public int CurrentAttackIndex 
    {
        get => currentAttackIndex; 
        set {
            if(value >= player.standAttackConfig.Length) currentAttackIndex = 0;
            else currentAttackIndex = value;
        }
    }
    private enum AttackChildState
    {
        ATK,
        End
    }
    
    private AttackChildState attackChildState;
    private AttackChildState attackchildState
    {
        get =>attackChildState;
        set{
            attackChildState = value;
            switch(attackChildState)
            {
                case AttackChildState.ATK:
                    player.PlayAnimation(player.standAttackConfig[currentAttackIndex].AnimationName,0f);
                    player.CanSwitch = false;//用于防止出现跳过某段攻击的bug
                    break;
                case AttackChildState.End:
                    player.PlayAnimation(player.standAttackConfig[currentAttackIndex].EndAnimationName,0f);
                    player.canInput = false;//进入收尾动画重置预输入标记
                    break;
            }

        }
    }
    public override void Enter()
    {
        CurrentAttackIndex = 0;
        player.rb.linearVelocity = Vector2.zero;
        attackchildState = AttackChildState.ATK;
        player.isUsingSkill = true;
    }
    
    public override void Update()
    {
        switch(attackchildState)
        {
            case AttackChildState.ATK:
                ATKonUpdate();
                //Apply Movement
                player.rb.linearVelocity = new Vector2(player.attackMovement[currentAttackIndex] * (player.isFacingRight ? 1 : -1) * Mathf.Abs(player.inputDir), player.rb.linearVelocityY);
                break;
            case AttackChildState.End:
                EndOnUpdate();
                break;
        }
    }
    private bool normalATK;
    private void ATKonUpdate()
    {
        if (player.canInput && Input.GetMouseButtonDown(0))
        {
            normalATK = true;
            player.canInput = false;//检测到有效输入则关闭预输入标记
        }
        
        //收刀检测
        if(CheckAnimatorStateName(player.standAttackConfig[currentAttackIndex].AnimationName, out float animationTime))
        {
            if(animationTime >= 0.9f)
            {
                attackchildState = AttackChildState.End;
                return;
            }
        }
        
        //继续攻击检测
        if(CheckStandAttack() && CurrentAttackIndex != 2 && normalATK)
        {
            CurrentAttackIndex ++;
            normalATK = false;
            attackchildState = AttackChildState.ATK;
            return;
        }
    }
    
    private bool CheckStandAttack()
    {
        return player.CanSwitch;
        //当前动画播放完之后，进入
    }
    
    private void EndOnUpdate()
    {
        //TODO:检测移动
        if (CheckAnimatorStateName(player.standAttackConfig[currentAttackIndex].EndAnimationName, out float animationTime))
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
            
            if (animationTime >= 0.9f)
            {
                player.ChangeState(PlayerState.Idle);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        player.isUsingSkill = false;
        //TODO:需要优化碰撞盒子的关闭逻辑，目前基于动画帧事件触发的攻击盒可能遇到被打断的情况导致的盒子无法关闭
        foreach (var attackBox in player.attackBoxes)
        {
            attackBox.GetComponent<PlayerWeapon>().StopAllCoro();
            attackBox.SetActive(false);
        }
    }
}
