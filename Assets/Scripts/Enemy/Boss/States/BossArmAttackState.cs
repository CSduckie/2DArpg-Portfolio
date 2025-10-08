using DG.Tweening;
using UnityEngine;

public class BossArmAttackState : BossStateBase
{
    private float currentStateTimer;
    private bool startCountng;
    private Tween current;
    private enum ArmAttackChildState
    {
        SpwaningArms,
        Attack,
        EndAttack
    }
    
    private ArmAttackChildState armAttackChildState;
    private ArmAttackChildState armAttackchildState
    {
        get =>armAttackChildState;
        set{
            armAttackChildState = value;
            switch(armAttackChildState)
            {
                case ArmAttackChildState.SpwaningArms:
                    boss.SpwanClaws();
                    Debug.Log("Spwaning arms");
                    break;
                case ArmAttackChildState.Attack:
                    current = boss.transform.DOMove(boss.bossArmAttackPos.position, 2f).OnComplete(() => SetArmsToAttack());
                    break;
                case ArmAttackChildState.EndAttack:
                    //开始移动到最上方，完成后，让arms退场,然后回到Idle
                    current = boss.transform.DOMove(boss.heightPoint.position, 2f).OnComplete(() => SetArmsToLeaveAndReturnToIdle());
                    break;
            }
        }
    }
    
    public override void Enter()
    {
        armAttackchildState = ArmAttackChildState.SpwaningArms;
        boss.canUseClaw = false;
        currentStateTimer = 0;
        startCountng = false;
    }
    
    public override void Update()
    {
        switch(armAttackchildState)
        {
            case ArmAttackChildState.SpwaningArms:
                SPArmsOnUpdate();
                break;
            case ArmAttackChildState.Attack:
                ArmAttackOnUpdate();
                break;
            case ArmAttackChildState.EndAttack:
                if(boss.bossArms[0].gameObject.activeSelf == false && boss.bossArms[0].gameObject.activeSelf == false)
                    boss.ChangeState(BossStates.Idle);
                break;
        }
    }

    private void SPArmsOnUpdate()
    {
        if (boss.spwanCompleted)
        {
            armAttackchildState = ArmAttackChildState.Attack;
        }
    }

    private void ArmAttackOnUpdate()
    {
        if(!startCountng)
            return;
        
        //使用计时器，时间到达，就让两个物体设置回Idle状态
        currentStateTimer += Time.deltaTime;
        if (currentStateTimer >= 10f)
        {
            foreach (var arm in boss.bossArms)
            {
                arm.ChangeState(BossArmState.Idle);
            }
            armAttackchildState = ArmAttackChildState.EndAttack;
            //关闭计时器
            startCountng = false;
        }
    }
    
    //用于调用手臂进入攻击状态的函数
    private void SetArmsToAttack()
    {
        foreach (var arm in boss.bossArms)
        {
            arm.ChangeState(BossArmState.Attack);
        }
        //启动计时器
        startCountng = true;
    }

    private void SetArmsToLeaveAndReturnToIdle()
    {
        foreach (var arm in boss.bossArms)
        {
            arm.ChangeState(BossArmState.Leave);
        }
        return;
    }

    public override void Exit()
    {
        boss.CoolClawSkill();
        //TODO:希望在技能结束以后积累一定的stun值
    }
}
