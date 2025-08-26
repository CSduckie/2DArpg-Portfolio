using UnityEngine;

public class DroneStats : CharacterStats
{
    private DroneController drone;
    protected override void Start()
    {
        base.Start();
        drone = GetComponent<DroneController>();
    }
    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        //设置击退由于无人机没有重力效果，因此需要适当微调击退力度
        drone.rb.linearVelocity = new Vector2(repelDir.x/2 * attackDir,repelDir.y/2);
        
        currentHealth -= _damage;
        //Health UI Update
        onHealthChanged?.Invoke();
        
        //判断是否死亡
        if (currentHealth <= 0 && !isDead)
        {
            OnDie();
            return;
        }
        
        drone.GetHurt();
        
        //被攻击时看向玩家位置
        if (drone.isFacingRight)
        {
            if (attackDir == 1)
            {
                drone.Flip();
            }
        }
        else
        {
            if (attackDir == -1)
            {
                drone.Flip();
            }
        }
    }

    protected override void OnDie()
    {
        drone.Die();
    }
    
    public override void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        //设置击退
        drone.rb.linearVelocity = new Vector2(_repelDir.x * _attackDir,_repelDir.y);
        drone.GetStun();
    }
}
