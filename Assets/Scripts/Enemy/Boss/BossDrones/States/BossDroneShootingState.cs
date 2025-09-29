using UnityEngine;

public class BossDroneShootingState : BossDroneStateBase
{
    public override void Enter()
    {
        bossDrone.PlayAnimation("Idle",0f);
        float z = bossDrone.GetLookZAngle(bossDrone.transform.position,bossDrone.posToAttack,0f);
        if (Mathf.Abs(z) > 90f)
        {
            bossDrone.transform.localScale = new Vector3(1,-1, 1);
        }
        else
        {
            bossDrone.transform.localScale = new Vector3( 1 , 1, 1);
        }
        bossDrone.transform.rotation = Quaternion.Euler(0f, 0f, z);
        
        //启动攻击携程，攻击完毕后，切换状态
        bossDrone.StartAttack();
    }

    public override void Exit()
    {
        bossDrone.DisableWeapon();
    }

    public override void Update()
    {
        if (CheckAnimatorStateName("Attack", out float animationTime))
        {
            if (animationTime >= 0.9f)
            {
                bossDrone.ChangeState(BossDroneState.GoBackToBossPos);
                return;
            }
        }
    }

}
