using UnityEngine;

public class BossDroneAttackState : BossStateBase
{
    public override void Enter()
    {
        boss.canCallDrone = false;
        boss.PlayAnimation("ControllingDrones",0f);
        //生成无人机
        boss.SpwanDrone();
    }

    public override void Update()
    {
        if (boss.droneNum == 0)
        {
            boss.ChangeState(BossStates.Idle);
            return;
        }
    }
    
    public override void Exit()
    {
        boss.CoolDroneAttackSkill();
    }

}
