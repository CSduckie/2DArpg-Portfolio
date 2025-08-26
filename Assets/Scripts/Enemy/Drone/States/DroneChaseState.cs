using UnityEngine;

public class DroneChaseState : DroneStateBase
{
    private Vector2 moveDir;
    private float lostPlayerTimeCounter;
    private Vector2 playerPos;
    
    public override void Enter()
    {
        lostPlayerTimeCounter = 0;
        
        //进入追击状态 开启导航组件的使用
        drone.agent.enabled = true;
        drone.PlayAnimation("Patrol",0f);
    }

    public override void Update()
    {
        //如果没有发现玩家，开始读秒，如果读秒完成以后，则切换回Idle状态。
        if (!drone.isPlayerFound())
             lostPlayerTimeCounter += Time.deltaTime;
        else
             lostPlayerTimeCounter = 0;
        if (lostPlayerTimeCounter >= drone.lostPlayerTime)
        {
             drone.ChangeState(DroneState.Idle);
             return;
        }
        
        
        //以上检测通过以后实时获取玩家位置并且追击
        playerPos = drone.GetPlayerPos();
        drone.agent.SetDestination(playerPos);
        
        //这部分用于检查是否到达攻击距离，如果到达了，切换状态为AttackState
        if (Mathf.Abs(drone.transform.position.x - playerPos.x) < drone.attackDistance && Mathf.Abs(drone.transform.position.y - playerPos.y) < 0.5f)
        {
            drone.ChangeState(DroneState.Battle);
            return;
        }
        
        
        //TODO:及时对于玩家的位置做出flip相应
        //判断是否需要转身
        if (playerPos.x >= drone.transform.position.x)
        {
            if(!drone.isFacingRight)
                drone.Flip();
        }
        else
        {
            if(drone.isFacingRight)
                drone.Flip();
        }
    }
    
    public override void Exit()
    {
        //退出追击状态时取消导航组件的使用
        drone.agent.enabled = false;
    }

}
