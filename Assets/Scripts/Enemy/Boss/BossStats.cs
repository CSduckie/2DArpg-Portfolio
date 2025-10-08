using UnityEngine;

public class BossStats : CharacterStats
{
    private BossController boss;
    public System.Action onCoolingChanged;
    
    [Header("Boss失衡")]
    public float currentCooling { get; private set; }
    
    
    protected override void Start()
    {
        base.Start();
        boss = GetComponent<BossController>();
    }

    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        //BOSS不做击退处理,并且只有stun状态才会扣血
        if (boss.isStun)
        {
            currentHealth -= _damage;
        }
        
        boss.GetComponentInChildren<BossSprite>().StartFlash();
        
        Debug.Log(currentHealth);
        
        //Health UI Update
        onHealthChanged?.Invoke();
        
        //判断是否死亡
        if (currentHealth <= 0 && !isDead)
        {
            OnDie();
            return;
        }
        
        //boss.GetHurt();
        GettingStunValue(_stunValue);

    }

    protected override void OnDie()
    {
        boss.Die();
    }
    
    public override void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        Debug.Log("被玩家完美格挡");
        GettingStunValue(50);
    }
    
    //增加失衡
    public void GettingStunValue(float _value)
    {
        currentCooling += _value;
        if (currentCooling >= maxExoEnergy.GetValue())
        {
            boss.GetStun();
        }
        
        onCoolingChanged?.Invoke();
    }

    //控制器中于Stun状态呼叫，然后呼叫UI
    public void clearCoolingUI()
    {
        currentCooling = 0;
        onCoolingChanged?.Invoke();
    }
}
