using UnityEngine;

public class BossForceKillSkill : BossStateBase
{
    private float spwanVFXTimer;
    public override void Enter()
    {
        boss.PlayAnimation("ForceKillCharge",0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        spwanVFXTimer += Time.deltaTime;
        if (spwanVFXTimer >= 2f)
        {
            boss.enableRandomSparks();
            spwanVFXTimer = 0;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
