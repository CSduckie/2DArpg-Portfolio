using UnityEngine;

public class BossUI : EnemyUI
{
    private BossStats bossStats;
    
    protected override void Start()
    {
        base.Start();
        //进行一波转换，好让该脚本能够访问archer内部的自身属性
        bossStats = enemyCharacterStats as BossStats;
        bossStats.onCoolingChanged += UpdateCoolUI;
    }

    private void UpdateCoolUI()
    {
        coolingImage.fillAmount = bossStats.currentCooling / bossStats.maxExoEnergy.GetValue();
        
    }
}
