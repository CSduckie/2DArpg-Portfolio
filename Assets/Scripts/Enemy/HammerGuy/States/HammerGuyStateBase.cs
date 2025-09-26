using UnityEngine;

public class HammerGuyStateBase : StateBase
{
    protected HammerGuyController hammerGuy;

    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
        hammerGuy = (HammerGuyController)owner;
    }

    protected virtual bool CheckAnimatorStateName(string stateName,out float normalizedTime )
    {
        //处于动画过渡阶段的考虑，先判断下一个状态
        AnimatorStateInfo nextInfo = hammerGuy.sprite.anim.GetNextAnimatorStateInfo(0);
        if(nextInfo.IsName(stateName))
        {
            normalizedTime = nextInfo.normalizedTime;
            return true;
        }
        AnimatorStateInfo info = hammerGuy.sprite.anim.GetCurrentAnimatorStateInfo(0);
        normalizedTime = info.normalizedTime;
        return info.IsName(stateName);
        
    }
}
