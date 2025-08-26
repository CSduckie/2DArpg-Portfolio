using UnityEngine;

public class TurretStats : CharacterStats
{
    private TurretController turret;
    protected override void Start()
    {
        base.Start();
        turret = GetComponent<TurretController>();
    }
    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        //由于炮台是固定位置，所以不需要有击退的应用，直接触发扣血即可
        
        currentHealth -= _damage;
        //Health UI Update
        onHealthChanged?.Invoke();
        
        
        //判断是否死亡
        if (currentHealth <= 0 && !isDead)
        {
            OnDie();
            return;
        }
        
        turret.GetHurt();
    }

    protected override void OnDie()
    {
        turret.Die();
    }
}
