using UnityEngine;

public class ShielderStats : CharacterStats
{
    private ShielderController shielder;
    
    public System.Action onCoolingChanged;
    
    [Header("精英失衡")]
    public float currentCooling { get; private set; }
    
    
    protected override void Start()
    {
        base.Start();
        shielder = GetComponent<ShielderController>();
    }
    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        if (shielder.isSleeping)
            return;
        
        shielder.rb.linearVelocity = new Vector2(repelDir.x * attackDir,0);

        if (!shielder.isStun)
            currentHealth -= _damage;
        else
            currentHealth -= 2 * _damage;
        
        shielder.GetComponentInChildren<ShielderSprite>().StartFlash();
        
        //Health UI Update
        onHealthChanged?.Invoke();
        
        //判断是否死亡
        if (currentHealth <= 0 && !isDead)
        {
            OnDie();
            return;
        }
        
        shielder.GetHurt();
        GettingStunValue(_stunValue);
        //被攻击时看向玩家位置
        if (shielder.isFacingRight)
        {
            if (attackDir == 1)
            {
                shielder.Flip();
            }
        }
        else
        {
            if (attackDir == -1)
            {
                shielder.Flip();
            }
        }
    }

    protected override void OnDie()
    {
        shielder.Die();
    }
    
    public override void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        Debug.Log("被玩家完美格挡");
        //检测当前处于什么状态，如果是普通状态则:
        if (shielder.isBullAttack)
        {
            GettingStunValue(1000);
        }
        else
        {
            shielder.GetStunning();
            GettingStunValue(5);
        }
    }
    
    //增加失衡
    private void GettingStunValue(float _value)
    {
        currentCooling += _value;
        if (currentCooling >= maxExoEnergy.GetValue())
        {
            shielder.GetStun();
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
