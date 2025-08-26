using UnityEngine;

public class TurretShootState : TurretStateBase
{
    public override void Enter()
    {
        turret.PlayAnimation("Shoot",0f);
        //在枪口位置生成子弹
        turret.ShootBullet();
        
        Debug.Log("进入射击状态");
    }
    
    public override void Update()
    {
        if (CheckAnimatorStateName("Shoot", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                turret.ChangeState(TurretState.Battle);
                return;
            }
        }
    }

    public override void Exit()
    {
        //启动攻击冷却携程
        turret.StartAttackCool();
    }

}
