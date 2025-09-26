using UnityEngine;

public class PlayerStats : CharacterStats
{
    [Header("用于更新失衡量条的UI事件")]
    public System.Action onCoolingChanged;
    public System.Action onEnergyChanged;
    
    private PlayerController player;
    
    [SerializeField] private float coolingRecoverSpeed;
    private float currentCooling;
    private float currentHealSkillAmount;

    [SerializeField] private float energyRecoverSpeed;
    
    
    [Header("属于玩家的Stats")]
    public Stats maxExoCooling;
    public Stats healSkillAmount;
    
    
    protected override void Start()
    {
        base.Start();
        player = GetComponent<PlayerController>();
        currentCooling = 0;
        currentHealSkillAmount = healSkillAmount.GetValue();
    }
    protected override void Update()
    {
        //按一定时间减少失衡条
        CoolingRecover();
        
        //按照一定自然时间回复能量条
        EnergyRecover();
    }
    
    //失衡条回复函数
    private void CoolingRecover()
    {
        if (currentCooling >= 0)
        {
            currentCooling -= Time.deltaTime * coolingRecoverSpeed;
            onCoolingChanged?.Invoke();
        }
    }

    private void EnergyRecover()
    {
        if (currentEnergy < maxExoEnergy.GetValue())
        {
            currentEnergy += Time.deltaTime * energyRecoverSpeed;
            onEnergyChanged?.Invoke();
        }
    }
    
    
    public override void TakeDamage(float _damage,float _stunValue,int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        //判定一个前置条件：玩家是否处于防御姿态，如果处于防御姿态，则判定攻击是否可以阻挡
        if (player.isDefending && (player.isFacingRight?1:-1) != attackDir)
        {
             //检查是否完美格挡
             if (player.canPerfectDefend)
             {
                 //通知玩家进入完美格挡状态
                 player.TriggerPerfectDefend();
                 //同时回传给攻击者脚本，要求其触发被弹反逻辑
                 _attackerStats.GetPerfectDefend(-attackDir,repelDir);
             }
             else
             {
                //玩家没有在完美格挡，因此累计量条,进行击退，但是免疫伤害
                currentCooling += _stunValue;
                player.rb.linearVelocity =new Vector2(repelDir.x * attackDir,0);
                //同时判断以下这次积累完毕后玩家是否会进入硬直
                if (currentCooling >= maxExoCooling.GetValue())
                    player.GetStun();
                else
                    player.TriggerNormalDefend();
             }
             return;
        }
        
        
        currentHealth -= _damage;
        
        //先判定是否处于一个正在Stun的状态，如果是，那么清空Stun条并且造成额外伤害
        if (!player.isStun)
            currentCooling += _stunValue;
        else
        {
            currentCooling = 0;
            currentHealth -= 50*_damage;
        }
        
        player.rb.linearVelocity =new Vector2(repelDir.x * attackDir,repelDir.y);
        
        if(currentCooling >= maxExoCooling.GetValue())
            player.GetStun();
        else
            player.GetHurt();
        
        
        if (currentHealth <= 0 && !isDead)
            OnDie();
        
        //Health UI Update
        onHealthChanged?.Invoke();
        //cooling UI Update
        onCoolingChanged?.Invoke();
        
        Debug.Log("DIR:" + attackDir);
        //被攻击时看向敌人位置
        if (player.isFacingRight)
        {
            if (attackDir == 1)
            {
                player.FlipWithNoInput();
            }
        }
        else
        {
            if (attackDir == -1)
            {
                player.FlipWithNoInput();
            }
        }
    }

    public void TakeDamagebyTraps(float _damage, float _stunValue, int attackDir, Vector2 repelDir)
    {
        currentHealth -= _damage;
        //先判定是否处于一个正在Stun的状态，如果是，那么清空Stun条并且造成额外伤害
        if (!player.isStun)
            currentCooling += _stunValue;
        else
        {
            currentCooling = 0;
            currentHealth -= 50*_damage;
        }
        
        player.rb.linearVelocity =new Vector2(repelDir.x * attackDir,repelDir.y);
        
        if(currentCooling >= maxExoCooling.GetValue())
            player.GetStun();
        else
            player.GetHurt();
        
        
        if (currentHealth <= 0 && !isDead)
            OnDie();
        
        //Health UI Update
        onHealthChanged?.Invoke();
        //cooling UI Update
        onCoolingChanged?.Invoke();
        
        //被攻击时看向敌人位置
        if (player.isFacingRight)
        {
            if (attackDir == 1)
            {
                player.Flip();
            }
        }
        else
        {
            if (attackDir == -1)
            {
                player.Flip();
            }
        }
    }
    
    
    //在状态机中调用的治疗技能函数
    public void Heal()
    {
        currentHealth += currentHealSkillAmount;
        
        if (currentHealth >= GetMaxHP())
        {
            currentHealth = GetMaxHP();
        }
        
        currentCooling = 0;
        
        onCoolingChanged?.Invoke();
        onHealthChanged?.Invoke();
    }
    //在状态机中调用的使用技能消耗能量的函数
    public void UseSkill(float _energyCost)
    {
        currentEnergy -= _energyCost;
        onEnergyChanged?.Invoke();
    }
    
    
    protected override void OnDie()
    {
        player.Die();
    }
    
    #region 定义于玩家状态中的玩家专属状态获取值函数

    public float GetCurrentCooling()
    {
        return currentCooling;
    }
    
    public float GetMaxExoCooling()
    {
        return maxExoCooling.GetValue();
    }
    
    #endregion
}
