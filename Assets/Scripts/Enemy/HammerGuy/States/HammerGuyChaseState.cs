using UnityEngine;

public class HammerGuyChaseState : HammerGuyStateBase
{
    private Vector2 playerPosition;
    public override void Enter()
    {
        hammerGuy.PlayAnimation("Walk",0f);
    }

    public override void Update()
    {
        playerPosition = GameManager.instance.player.transform.position;
        
        //面向玩家
        if (playerPosition.x > hammerGuy.transform.position.x)
        {
            if (!hammerGuy.isFacingRight)
            {
                hammerGuy.Flip();
            }
        }
        else
        {
            if(hammerGuy.isFacingRight)
                hammerGuy.Flip();
        }
        
        
        if (Mathf.Abs(hammerGuy.rb.position.x - playerPosition.x) < hammerGuy.attackRange)
        {
            hammerGuy.ChangeState(HammerGuyStates.Battle);
            return;
        }
    }

    public override void FixedUpdate()
    {
        hammerGuy.rb.linearVelocity =  new Vector2(hammerGuy.chaseSpeed * (hammerGuy.isFacingRight? 1:-1), hammerGuy.rb.linearVelocityY);
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
