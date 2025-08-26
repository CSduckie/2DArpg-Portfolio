using UnityEngine;

public class DroneStateBase : StateBase
{
    protected DroneController drone;

    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
        drone = (DroneController)owner;
    }

    protected virtual bool CheckAnimatorStateName(string stateName,out float normalizedTime )
    {
        //处于动画过渡阶段的考虑，先判断下一个状态
        AnimatorStateInfo nextInfo = drone.sprite.anim.GetNextAnimatorStateInfo(0);
        if(nextInfo.IsName(stateName))
        {
            normalizedTime = nextInfo.normalizedTime;
            return true;
        }
        AnimatorStateInfo info = drone.sprite.anim.GetCurrentAnimatorStateInfo(0);
        normalizedTime = info.normalizedTime;
        return info.IsName(stateName);
        
    }
}
