using UnityEngine;

public class DronePatrolState : DroneStateBase
{
    private Vector3 targetPoint;
    private Vector2 moveDir;
    
    public override void Enter()
    {
        targetPoint = drone.GetNewPoint();
        
        //判断是否需要转身
        if (targetPoint.x >= drone.transform.position.x)
        {
            if(!drone.isFacingRight)
                drone.Flip();
        }
        else
        {
            if(drone.isFacingRight)
                drone.Flip();
        }
        
        
        moveDir = targetPoint - drone.transform.position;
        moveDir = moveDir.normalized;
        
        //播放动画
        drone.PlayAnimation("Patrol",0f);
    }
    
    public override void Update()
    {
        //检测与玩家的距离是否小于一定值,或者运动到了targetPoint
        if (drone.isPlayerFound())
        {
            drone.ChangeState(DroneState.Chase);
            return;
        }
        //如果运动到了目标位置或者碰墙，那么停止移动
        if (Mathf.Abs(targetPoint.x - drone.transform.position.x) < 0.1f)
        {
            drone.ChangeState(DroneState.Idle);
            return;
        }
        
        drone.rb.linearVelocity = moveDir * drone.moveSpeed;
    }
    

    public override void Exit()
    {
        base.Exit();
    }

}
