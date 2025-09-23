using UnityEngine;

public class ShielderWalkState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("Walk",0f);
    }
    
    public override void Update()
    {
        if (shielder.playerTrans.position.x > shielder.transform.position.x)
        {
            if (!shielder.isFacingRight)
            {
                shielder.Flip();
            }
        }
        else
        {
            if(shielder.isFacingRight)
                shielder.Flip();
        }
        
        //检查是否可以使用远程攻击
        if (shielder.canRangeATK)
        {
            if (Mathf.Abs(shielder.playerTrans.position.x - shielder.transform.position.x) > shielder.attackRange &&
                Mathf.Abs(shielder.playerTrans.position.x - shielder.transform.position.x) <= shielder.rangeAttackRange)
            {
                float skillChance = Random.value;
                if (skillChance <= shielder.useRangeATKChance)
                {
                    shielder.ChangeState(ShielderState.RangeAttack);
                    return;
                }
            }
        }
        
        //检查，进入攻击距离
        if (Mathf.Abs(shielder.playerTrans.position.x - shielder.transform.position.x) < shielder.attackRange)
        {
            shielder.ChangeState(ShielderState.Idle);
        }
        
    }

    public override void FixedUpdate()
    {
        float dir = (shielder.playerTrans.position.x - shielder.transform.position.x) > 0 ? 1 : -1;
        shielder.rb.linearVelocity = new Vector2(shielder.chaseSpeed * dir, shielder.rb.linearVelocityY);
    }

    public override void Exit()
    {
        shielder.rb.linearVelocity = Vector2.zero;
    }

}
