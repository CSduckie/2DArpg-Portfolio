using UnityEngine;

public class AssassinChaseState : AssassinStateBase
{
    public override void Enter()
    {
        assassin.PlayAnimation("Chase",0f);
    }

    public override void Update()
    {
        if (Mathf.Abs(assassin.player.transform.position.x - assassin.transform.position.x) < assassin.attackDistance)
        {
            assassin.ChangeState(AssassinStates.Idle);
            return;
        }
    }

    public override void FixedUpdate()
    {
        float dir = assassin.player.transform.position.x - assassin.transform.position.x > 0 ? 1 : -1;
        assassin.rb.linearVelocity = new Vector2(assassin.chaseSpeed * dir, assassin.rb.linearVelocityY);
    }
    
    public override void Exit()
    {
        assassin.rb.linearVelocity = Vector2.zero;
    }

}
