using System;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private SkillConfig skillConfig;

    private EnemyController enemy;
    private float weaponDamage;
    public float stunValue { get; private set; }
    private Vector2 repelDir;
    public float impulseValue { get;private set; }
    private void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
        weaponDamage = skillConfig.releaseData.attackData.hitData.value;
        repelDir = skillConfig.releaseData.attackData.hitData.RepelVelocity;
        stunValue = skillConfig.releaseData.attackData.hitData.stunValue;
        impulseValue = skillConfig.releaseData.attackData.ScreenImpulseValue;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.isDead)
                return;

            CharacterStats attackerStats = GetComponentInParent<CharacterStats>();
            
            int attackDir = enemy.isFacingRight ? 1 : -1;
            
            
            //TODO:后续需要添加damage的修改值
            //此处传入攻击者自身的伤害数值脚本，用于反伤等计算
            other.GetComponent<CharacterStats>().TakeDamage(weaponDamage,stunValue,attackDir,repelDir,attackerStats);
        }
    }
    
}
