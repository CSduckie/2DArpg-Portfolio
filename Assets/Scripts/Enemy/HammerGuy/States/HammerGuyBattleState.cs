using UnityEngine;

public class HammerGuyBattleState : HammerGuyStateBase
{
    private Vector2 playerPosition;
    public override void Enter()
    {
        hammerGuy.PlayAnimation("Idle",0f);
        hammerGuy.rb.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        playerPosition = GameManager.instance.player.transform.position;
        if (Mathf.Abs(hammerGuy.rb.position.x - playerPosition.x) < hammerGuy.attackRange)
        {
            if(hammerGuy.canAttack)
                hammerGuy.ChangeState(HammerGuyStates.Attack);
        }
        else
        {
            hammerGuy.ChangeState(HammerGuyStates.Chase);
        }
        return;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
