using UnityEngine;

public class BossIdleState : BossStateBase
{
    public override void Enter()
    {
        boss.PlayAnimation("Idle",0f);
    }

    public override void Update()
    {
        if (boss.transform.position.y <= boss.heightPoint.transform.position.y)
        {
            boss.rb.linearVelocityY = boss.bossClimbUpSpeed;
        }
        else
        {
            boss.rb.linearVelocityY = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            boss.ChangeState(BossStates.SmashAttack);
            return;
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
