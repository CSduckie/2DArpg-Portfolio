using UnityEngine;

public class BossArmAttackState : BossStateBase
{
    public override void Enter()
    {
        boss.SpwanClaws();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
