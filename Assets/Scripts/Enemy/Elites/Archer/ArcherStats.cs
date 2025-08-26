using UnityEngine;

public class ArcherStats : CharacterStats
{
    private ArcherController archer;
    
    public System.Action onCoolingChanged;
    
    [Header("精英射手失衡")]
    public float currentCooling { get; private set; }
    
    
    protected override void Start()
    {
        base.Start();
        archer = GetComponent<ArcherController>();
    }
    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        
        archer.rb.linearVelocity = new Vector2(repelDir.x * attackDir,repelDir.y);
        
        currentHealth -= _damage;
        //Health UI Update
        onHealthChanged?.Invoke();
        
        //判断是否死亡
        if (currentHealth <= 0 && !isDead)
        {
            OnDie();
            return;
        }
        
        archer.GetHurt();
        
        //被攻击时看向玩家位置
        if (archer.isFacingRight)
        {
            if (attackDir == 1)
            {
                archer.Flip();
            }
        }
        else
        {
            if (attackDir == -1)
            {
                archer.Flip();
            }
        }
    }

    protected override void OnDie()
    {
        archer.Die();
    }
    
    public override void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        //设置击退
        archer.rb.linearVelocity = new Vector2(_repelDir.x * _attackDir,_repelDir.y);
        if (archer.isSpecialAttack)
        {
            archer.GetStun();
        }
        else if (archer.isNormalAttack)
            GettingStunValue(maxExoEnergy.GetValue()/2);
    }
    
    //增加失衡
    private void GettingStunValue(float _value)
    {
        currentCooling += _value;
        if (currentCooling >= maxExoEnergy.GetValue())
        {
            archer.GetStun();
        }
        
        onCoolingChanged?.Invoke();
    }

    //射手控制器中于Stun状态呼叫，然后呼叫UI
    public void clearCoolingUI()
    {
        currentCooling = 0;
        onCoolingChanged?.Invoke();
    }
    
}
