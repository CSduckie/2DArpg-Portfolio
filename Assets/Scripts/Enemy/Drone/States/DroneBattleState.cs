using UnityEngine;

public class DroneBattleState : DroneStateBase
{
    private Vector2 playerPos;
    public override void Enter()
    {
        drone.PlayAnimation("Idle",0f);
    }

    public override void Update()
    {
        if (drone.canAttack)
        {
            drone.ChangeState(DroneState.Attack);
            return;
        }
        
        //TODO:需要一个当玩家距离过远时，进入chase状态；
        if (Mathf.Abs(drone.GetPlayerPos().x - drone.transform.position.x) >= drone.attackDistance)
        {
            drone.ChangeState(DroneState.Chase);
            return;
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }

}
