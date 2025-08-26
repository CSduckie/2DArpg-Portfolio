using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public System.Action onHealthChanged;
    protected float currentHealth;
    protected float currentEnergy;
    public bool isDead { get; private set; }
    
    
    [Header("Defensive stats")]
    public Stats maxHp;
    public Stats maxExoEnergy;

    
    protected virtual void Start()
    {
        currentHealth = maxHp.GetValue();
        currentEnergy = maxExoEnergy.GetValue();
    }

    protected virtual void Update()
    {
        
    }


    public virtual void TakeDamage(float _damage,float _stunValue,int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {

    }

    protected virtual void OnDie()
    {
        
    }

    public virtual void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        
    }
    
    /// <summary>
    /// UI内容更新相关
    /// </summary>
    /// <returns></returns>
    ///
    ///
    ///
    /// 
    //TODO:修改成可以随着玩家升级修改的内容
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public float GetMaxHP()
    {
        return maxHp.GetValue();
    }

    public float GetMaxEnergy()
    {
        return maxExoEnergy.GetValue();
    }
    
    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }
}
