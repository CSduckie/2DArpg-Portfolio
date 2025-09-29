using UnityEngine;

public class BossDroneGoBackState : BossDroneStateBase
{
    public override void Enter()
    {
        float z = bossDrone.GetLookZAngle(bossDrone.transform.position,bossDrone.boss.transform.position,0f);
        if (Mathf.Abs(z) > 90f)
        {
            bossDrone.transform.localScale = new Vector3(1,-1, 1);
        }
        else
        {
            bossDrone.transform.localScale = new Vector3(1,1, 1);
        }
        
        bossDrone.transform.rotation = Quaternion.Euler(0f, 0f, z);
        bossDrone.DoMoveTo(bossDrone.boss.GetComponent<Transform>().position,1f,"GoBackToBoss");
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
