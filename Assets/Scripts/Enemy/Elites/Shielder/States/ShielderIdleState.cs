using UnityEngine;

public class ShielderIdleState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("Idle",0f);
    }

    public override void Update()
    {
        //检查是否可以使用bullDash
        if (shielder.canBullDash)
        {
            float skillChance = Random.value;
            if (skillChance <= shielder.useBullDashChance)
            {
                shielder.ChangeState(ShielderState.BullDash);
                return;
            }
        }

        if (Mathf.Abs(shielder.playerTrans.position.x - shielder.transform.position.x) <= shielder.attackRange)
        {
            float skillChance = Random.value;
            if (skillChance <= shielder.useMultiCloseATKChance)
            {
                shielder.ChangeState(ShielderState.MultiCloseAttack); 
            }
            else if (skillChance <= shielder.useCloseATKChance)
            {
                shielder.ChangeState(ShielderState.CloseAttack); 
            }
            return;
        }
        
        
        //检查，进入攻击距离，攻击
        if (Mathf.Abs(shielder.playerTrans.position.x - shielder.transform.position.x) > shielder.attackRange)
        {
            shielder.ChangeState(ShielderState.Walk);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
