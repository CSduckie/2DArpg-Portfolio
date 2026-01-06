using UnityEngine;

public class BossLaserAOESkillState : BossStateBase
{
    public override void Enter()
    {
        //启动激光
        boss.laser.laserDirection.x = -2.2f;
        boss.laser.gameObject.SetActive(true);
    }

    public override void Update()
    {
        //以一定旋转速度扫过
        boss.laser.laserDirection.x += boss.rotateSpeed * Time.deltaTime;
        if(boss.laser.laserDirection.x > 2.2f)
            boss.ChangeState(BossStates.Idle);
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public override void Exit()
    {
        boss.canLaser = false;
        boss.CoolLaserSkill();
        boss.StartSpwanEffectCoroutine();
        boss.laser.gameObject.SetActive(false);
    }
    
}
