using UnityEngine;

public class AssassinStats : CharacterStats
{
    private AssassinController assassin;
    
    
    
    protected override void Start()
    {
        base.Start();
        assassin = GetComponent<AssassinController>();
    }
    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        //设置击退
        assassin.rb.linearVelocity = new Vector2(repelDir.x * attackDir,repelDir.y);
        
        //判断是否目前处于Stun状态，如果是，则受到双倍的伤害
        if (!assassin.isStun)
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
        
        assassin.GetHurt();
        
        //被攻击时看向玩家位置
        if (assassin.isFacingRight)
        {
            if (attackDir == 1)
            {
                assassin.Flip();
            }
        }
        else
        {
            if (attackDir == -1)
            {
                assassin.Flip();
            }
        }
    }

    protected override void OnDie()
    {
        assassin.Die();
    }
    
    
    public override void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        //设置击退
        assassin.rb.linearVelocity = new Vector2(_repelDir.x * _attackDir,_repelDir.y);
        assassin.GetStun();
    }
}
