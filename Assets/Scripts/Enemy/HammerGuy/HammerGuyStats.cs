using UnityEngine;

public class HammerGuyStats : CharacterStats
{
    private HammerGuyController hammerGuy;
    
    protected override void Start()
    {
        base.Start();
        hammerGuy = GetComponent<HammerGuyController>();
    }
    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        //设置击退
        hammerGuy.rb.linearVelocity = new Vector2(repelDir.x * attackDir,repelDir.y);
        
        //判断是否目前处于Stun状态，如果是，则受到双倍的伤害
        if (!hammerGuy.isStun)
            currentHealth -= _damage;
        else
            currentHealth -= 2 * _damage;
        //Health UI Update
        onHealthChanged?.Invoke();
        
        //判断是否死亡
        if (currentHealth <= 0 && !isDead)
        {
            OnDie();
            return;
        }
        
        hammerGuy.GetHurt();
        
        //被攻击时看向玩家位置
        if (hammerGuy.isFacingRight)
        {
            if (attackDir == 1)
            {
                hammerGuy.Flip();
            }
        }
        else
        {
            if (attackDir == -1)
            {
                hammerGuy.Flip();
            }
        }
    }

    protected override void OnDie()
    {
        hammerGuy.Die();
    }
    
    
    public override void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        //设置击退
        hammerGuy.rb.linearVelocity = new Vector2(_repelDir.x * _attackDir,_repelDir.y);
        hammerGuy.GetStun();
    }
}
