using UnityEngine;

public class ArcherStunState : ArcherStateBase
{
    private float stunStateTimer;
    
    public override void Enter()
    {
        archer.PlayAnimation("Hurt",0f);
        archer.isStun = true;
        stunStateTimer = 0;
    }

    public override void Update()
    {
        stunStateTimer += Time.deltaTime;
        if (stunStateTimer >= archer.stunTime)
        {
            archer.isStun = false;
            stunStateTimer = archer.stunTime;
        }
        
        if(CheckAnimatorStateName("Hurt",out float animationTime))
        {
            if (animationTime >= 0.9f && !archer.isStun)
            {
                archer.ChangeState(ArcherStates.Teleport);
                return;
            }
        }
    }
    
    public override void Exit()
    {
        archer.GetComponent<ArcherStats>().clearCoolingUI();
        archer.isStun = false;
    }

}
