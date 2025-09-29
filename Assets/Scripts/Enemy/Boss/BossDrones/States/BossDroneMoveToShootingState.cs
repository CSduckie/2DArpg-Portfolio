using UnityEngine;

public class BossDroneMoveToShootingState : BossDroneStateBase
{
    public override void Enter()
    {
        bossDrone.PlayAnimation("Move",0f);
        
        float z = bossDrone.GetLookZAngle(bossDrone.transform.position,bossDrone.targetPos,0f);
        if (Mathf.Abs(z) > 90f)
        {
            bossDrone.transform.localScale = new Vector3(1,-1, 1);
        }
        else
        {
            bossDrone.transform.localScale = new Vector3(1 ,1, 1);
        }
        bossDrone.transform.rotation = Quaternion.Euler(0f, 0f, z);
        bossDrone.DoMoveTo(bossDrone.targetPos,0.3f,"MoveToShooting");
    }

    public override void Update()
    {
        base.Update();
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
