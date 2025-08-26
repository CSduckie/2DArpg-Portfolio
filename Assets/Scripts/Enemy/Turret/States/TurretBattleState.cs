using UnityEngine;

public class TurretBattleState : TurretStateBase
{
    private float lostTargetTimeCounter;
    
    public override void Enter()
    {
        turret.PlayAnimation("Battle",0f);
        
        //进入战斗后为了保证公平，因此提前进行一次攻击冷却让玩家能够反应过来
        turret.StartAttackCool();
    }

    public override void Update()
    {
        if (!turret.isPlayerFound())
        {
            //开始进入丢失目标的读秒
            lostTargetTimeCounter += Time.deltaTime;
        }
        else
        {
            lostTargetTimeCounter = 0;
        }

        if (lostTargetTimeCounter >= turret.lostTargetTime)
        {
            turret.ChangeState(TurretState.Close);
            return;
        }
        
        if (turret.canAttack && turret.isPlayerFound())
        {
            turret.ChangeState(TurretState.Shoot);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
