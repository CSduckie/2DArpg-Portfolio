using UnityEngine;
public class DroneAttackState : DroneStateBase
{
    public override void Enter()
    {
        drone.PlayAnimation("Attack",0f);
    }
    
    public override void Update()
    {
        if (CheckAnimatorStateName("Attack", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                drone.ChangeState(DroneState.Battle);
                //启动攻击冷却携程
                drone.AttackCooling();
                return;
            }
        }
    }

    public override void Exit()
    {
        //退出时手动强制关闭攻击判定盒子
        drone.droneWeapon.SetActive(false);
    }

}
