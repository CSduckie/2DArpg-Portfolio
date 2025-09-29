using UnityEngine;

public class ShielderBullDashState : ShielderStateBase
{
    private float currentStateTimer;
    public override void Enter()
    {
        shielder.isBullAttack = true;
        //进入时锁定一次玩家
        if (shielder.playerTrans.position.x > shielder.transform.position.x)
        {
            if (!shielder.isFacingRight)
            {
                shielder.Flip();
            }
        }
        else
        {
            if(shielder.isFacingRight)
                shielder.Flip();
        }
        shielder.PlayAnimation("BullDash",0f);
        currentStateTimer = 0;
    }


    public override void Update()
    {
        currentStateTimer += Time.deltaTime;
        if (currentStateTimer >= shielder.bullDashStateTime)
        {
            shielder.ChangeState(ShielderState.Idle);
            return;
        }
        
        if(shielder.IsWallDetected())
            shielder.Flip();
    }
    public override void FixedUpdate()
    {
        shielder.rb.linearVelocity = new Vector2(shielder.bullDashSpeed * (shielder.isFacingRight? 1:-1), shielder.rb.linearVelocityY);
    }

    public override void Exit()
    {
        shielder.CoolBullDash();
        shielder.isBullAttack = false;
        shielder.shielderWeapons[2].SetActive(false);
    }
    
}
