using UnityEngine;

public class ShielderUI : EnemyUI
{
    private ShielderStats shielderStats;
    
    protected override void Start()
    {
        base.Start();
        //进行一波转换，好让该脚本能够访问archer内部的自身属性
        shielderStats = enemyCharacterStats as ShielderStats;
        shielderStats.onCoolingChanged += UpdateCoolUI;
    }

    private void UpdateCoolUI()
    {
        coolingImage.fillAmount = shielderStats.currentCooling / shielderStats.maxExoEnergy.GetValue();
    }
}
