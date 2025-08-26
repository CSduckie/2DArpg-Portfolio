using UnityEngine;

public class TurretStateBase : StateBase
{
    protected TurretController turret;

    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
        turret = (TurretController)owner;
    }

    protected virtual bool CheckAnimatorStateName(string stateName,out float normalizedTime )
    {
        //处于动画过渡阶段的考虑，先判断下一个状态
        AnimatorStateInfo nextInfo = turret.sprite.anim.GetNextAnimatorStateInfo(0);
        if(nextInfo.IsName(stateName))
        {
            normalizedTime = nextInfo.normalizedTime;
            return true;
        }
        AnimatorStateInfo info = turret.sprite.anim.GetCurrentAnimatorStateInfo(0);
        normalizedTime = info.normalizedTime;
        return info.IsName(stateName);
        
    }
}
