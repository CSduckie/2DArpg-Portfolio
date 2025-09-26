using UnityEngine;

public class TurretBattleState : TurretStateBase
{
    private float bulletActiveCounter;
    public override void Enter()
    {
        bulletActiveCounter = 0;
        turret.PlayAnimation("Attack",0f);
    }

    public override void Update()
    {
        if (!turret.isPlayerFound())
        {
            turret.ChangeState(TurretState.Idle);
            return;
        }
        
        //每间隔一段时间启动一次hitbox，造成伤害
        
        bulletActiveCounter += Time.deltaTime;
        if (bulletActiveCounter >= turret.attackDuration)
        {
            if (turret.bullet.gameObject.activeSelf)
            {
                turret.bullet.gameObject.SetActive(false);
            }
            else
            {
                turret.bullet.gameObject.SetActive(true);
            }
            bulletActiveCounter = 0;
        }
        
    }
    
    
    
    public override void Exit()
    {
        turret.bullet.gameObject.SetActive(false);
    }
}
