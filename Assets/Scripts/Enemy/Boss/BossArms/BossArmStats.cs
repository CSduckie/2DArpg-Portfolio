using UnityEngine;

public class BossArmStats : CharacterStats
{
    private BossArmController bossArm;
    
    protected override void Start()
    {
        base.Start();
        bossArm = GetComponent<BossArmController>();
    }

    
    public override void TakeDamage(float _damage,float _stunValue, int attackDir,Vector2 repelDir,CharacterStats _attackerStats)
    {
        bossArm.boss.bossStats.GettingStunValue(_stunValue);
    }

    public override void GetPerfectDefend(int _attackDir, Vector2 _repelDir)
    {
        bossArm.boss.bossStats.GettingStunValue(bossArm.armHitBox.GetComponent<EnemyWeapon>().stunValue * 10f);
        //设置击退
        bossArm.Flip();
    }
}
