using UnityEngine;

public class ArcherUI : EnemyUI
{
    private ArcherStats archerStats;
    
    protected override void Start()
    {
        base.Start();
        //进行一波转换，好让该脚本能够访问archer内部的自身属性
        archerStats = enemyCharacterStats as ArcherStats;
        archerStats.onCoolingChanged += UpdateCoolUI;
    }

    private void UpdateCoolUI()
    {
        Debug.Log("uPDATED");
        coolingImage.fillAmount = archerStats.currentCooling / archerStats.maxExoEnergy.GetValue();
    }
}
