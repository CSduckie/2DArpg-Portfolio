using UnityEngine;

public class AssassinDieState : AssassinStateBase
{
    public override void Enter()
    {
        assassin.PlayAnimation("Die",0f);
        assassin.isDead = true;
    }
    public override void Update()
    {
        assassin.rb.linearVelocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

}
