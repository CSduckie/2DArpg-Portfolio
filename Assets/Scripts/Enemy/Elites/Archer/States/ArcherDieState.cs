using UnityEngine;

public class ArcherDieState : ArcherStateBase
{
    public override void Enter()
    {
        archer.PlayAnimation("Dead",0f);
        archer.isDead = true;
        archer.enemyUI.SetActive(false);
        archer.GetComponentInParent<ArcherFightEvent>().ArcherFightWon();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
