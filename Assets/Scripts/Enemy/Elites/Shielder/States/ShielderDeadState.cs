using UnityEngine;

public class ShielderDeadState : ShielderStateBase
{
    public override void Enter()
    {
        shielder.PlayAnimation("Dead",0f);
        shielder.isDead = true;
        shielder.enemyUI.SetActive(false);
        shielder.GetComponentInParent<ShielderFightControl>().ShielderFightWon();
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
