using UnityEngine;

public class ArcherChaseState : ArcherStateBase
{
    public override void Enter()
    {
        archer.PlayAnimation("Run",0f);
        Debug.Log("追击");
    }
    
    public override void Update()
    {
        
        if (Mathf.Abs(archer.player.position.x - archer.transform.position.x) < archer.attackDistance)
        {
            archer.ChangeState(ArcherStates.Idle);
            return;
        }
        
    }

    public override void FixedUpdate()
    {
        float dir = archer.player.position.x - archer.transform.position.x > 0 ? 1 : -1;
        archer.rb.linearVelocity = new Vector2(archer.chaseSpeed * dir, archer.rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
