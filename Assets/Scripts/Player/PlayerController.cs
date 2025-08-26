using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IStateMachineOwner
{
    //TODO:需要修改
    public float stunTime = 10;

    public PlayerStats playerStats { get; private set; }
    public PlayerSprite sprite;
    private StateMachine stateMachine;
    public Rigidbody2D rb;
    public float inputDir;
    
    [Header("摄像机跟随")] 
    private CameraFollow cameraFollow;
    [SerializeField] private GameObject cameraFollowPoint;
    
    [Header("状态标记")]
    public bool isHurt;
    public bool isDead;
    public bool isStun;
    public bool canInput;
    public bool CanSwitch;
    
    [HideInInspector]
    public bool isDefending;
    public bool isWallJumping;
    [HideInInspector]
    public bool isUsingSkill;
    
    [Header("防御和完美防御")]
    //检测完美防御
    public bool canPerfectDefend;
    [SerializeField] private float perfectDefendWindow;
    [HideInInspector] public bool triggerDefendAnim;
    
    [Header("移动参数")] 
    public float MoveSpeed;
    public float JumpSpeed;
    public float DoubleJumpSpeed;
    public float[] attackMovement;
    public Vector2 wallJumpSpeed;
    
    [HideInInspector]
    public bool canDoubleJump;
    public float dashSpeed;
    public float acceleration;
    public float deceleration;
    private float currentSpeed;
    [SerializeField] private float maxFallSpeed;
    
    [Header("转身")]
    public bool isFacingRight = true;
    
    [Header("碰撞检测")]
    [SerializeField] protected Transform groundCheck;
	[SerializeField] protected Vector3 groundleftOffset;
	[SerializeField] protected Vector3 groundRightOffset;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;  
    [SerializeField] protected LayerMask whatIsWall;
    
    [Header("攻击配置文件")]
    public SkillConfig[] standAttackConfig;
    [Header("治疗技能配置文件")] 
    public SkillConfig healSkillConfig;
    [Header("攻击判定框")] 
    public GameObject[] attackBoxes;
    [Header("斩击技能配置文件")] 
    public SkillConfig slashSkillConfig;
    public float slashChargeTime;
    //当前的充能段数
    [HideInInspector]
    public int currentChargeStage;
    
    
    void Start()
    {
        stateMachine = new StateMachine();
        playerStats = GetComponent<PlayerStats>();
        stateMachine.Init(this);
        ChangeState(PlayerState.Idle);
        cameraFollow = cameraFollowPoint.GetComponent<CameraFollow>();
    }

    //状态机
    public void ChangeState(PlayerState playerState)
    {
        switch(playerState)
        {
            case PlayerState.Idle:
                stateMachine.ChangeState<PlayerIdleState>();
                break;
            case PlayerState.Move:
                stateMachine.ChangeState<PlayerMoveState>();
                break;
            case PlayerState.Jump:
                stateMachine.ChangeState<PlayerJumpState>();
                break;
            case PlayerState.Fall:
                stateMachine.ChangeState<PlayerFallState>();
                break;
            case PlayerState.StandAttack:
                stateMachine.ChangeState<PlayerStandAttackState>();
                break;
            case PlayerState.Dash:
                stateMachine.ChangeState<PlayerDashState>();
                break;
            case PlayerState.DoubleJump:
                stateMachine.ChangeState<PlayerDoubleJumpState>();
                break;
            case PlayerState.Hurt:
                stateMachine.ChangeState<PlayerHurtState>();
                break;
            case PlayerState.Stun:
                stateMachine.ChangeState<PlayerStunState>();
                break;
            case PlayerState.HealSkill:
                stateMachine.ChangeState<PlayerRecoverSkillState>();
                break;
            case PlayerState.Die:
                stateMachine.ChangeState<PlayerDieState>();
                break;
            case PlayerState.Defend:
                stateMachine.ChangeState<PlayerDefendState>();
                break;
            case PlayerState.PerfectDefend:
                break;
            case PlayerState.DefendSuccessful:
                stateMachine.ChangeState<PlayerDefendSuccessfulState>();
                break;
            case PlayerState.WallJump:
                stateMachine.ChangeState<PlayerWallJumpState>();
                break;
            case PlayerState.WallSlide:
                stateMachine.ChangeState<PlayerWallSlideState>();
                break;
            case PlayerState.SlashSkill:
                stateMachine.ChangeState<PlayerSlashSkillState>();
                break;
        }
    }
    
    
    //动画调用
    public void PlayAnimation(string animationName,float fixedTransitionDuration = 0.25f)
    {
        sprite.anim.CrossFadeInFixedTime(animationName,fixedTransitionDuration);
    }


    #region Flip Functions
    //转身函数
    //用于处理有操作的情况下的转身行为
    public void Flip()
    {
        if(isFacingRight)
        {
            if (inputDir < 0)
            {
                transform.rotation = Quaternion.Euler(0,180f,0);
                isFacingRight = !isFacingRight;
                cameraFollow.CallTurn();
            }
        }
        else
        {
            if (inputDir > 0)
            {
                transform.rotation = Quaternion.Euler(0,0f,0);
                isFacingRight = !isFacingRight;
                cameraFollow.CallTurn();
            }
        }
    }
    //用于处理没有操作的情况下的转身行为
    public void FlipWithNoInput()
    {
        if(isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0,180f,0);
            isFacingRight = !isFacingRight;
            cameraFollow.CallTurn();
        }
        else
        {

            transform.rotation = Quaternion.Euler(0,0f,0);
            isFacingRight = !isFacingRight;
            cameraFollow.CallTurn();
        }
    }
    

    #endregion
    
    
    public void Update()
    {
        if(!(isHurt || isDead || isStun || isDefending || isWallJumping || isUsingSkill))
            Flip();
        
        currentSpeed = rb.linearVelocityX;
        //获取操作
        inputDir = Input.GetAxisRaw("Horizontal");

        FallSpeedLimit();
        
        CheckForDefendInput();
    }


    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,
        Vector2.down, groundCheckDistance, whatIsGround) || Physics2D.Raycast(groundCheck.position + groundleftOffset,
        Vector2.down, groundCheckDistance, whatIsGround) || Physics2D.Raycast(groundCheck.position + groundRightOffset,
        Vector2.down, groundCheckDistance, whatIsGround);
    
    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * (isFacingRight?1:-1), wallCheckDistance, whatIsWall);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

		Gizmos.DrawLine(groundCheck.position + groundleftOffset,
            new Vector3((groundCheck.position + groundleftOffset).x, (groundCheck.position + groundleftOffset).y - groundCheckDistance));

		Gizmos.DrawLine(groundCheck.position + groundRightOffset,
            new Vector3((groundCheck.position + groundRightOffset).x, (groundCheck.position + groundRightOffset).y - groundCheckDistance));

        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * (isFacingRight?1:-1) , wallCheck.position.y));
    }
    #endregion


    #region 玩家的速度更新，下落速度限制相关
    //速度更新逻辑
    //如果是需要输入的状态中更新速度，那么传入true，如果是Idle那种减速运动，传入false
    public void UpdateVelocity(bool _isHavingInput)
    {
        if (_isHavingInput)
        {
            //惯性加速减速
            float targetSpeed = inputDir * MoveSpeed;
            //计算加速还是减速
            float speedDiff = targetSpeed - currentSpeed;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            //计算速度变化
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);
            currentSpeed += movement * Time.deltaTime;
            //应用速度
            rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocityY);
        }
        else
        {
            //惯性加速减速
            float targetSpeed = 0 * MoveSpeed;
            //计算加速还是减速
            float speedDiff = targetSpeed - currentSpeed;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            //计算速度变化
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);
            currentSpeed += movement * Time.deltaTime;
            //应用速度
            rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocityY);
        }
    }

    private void FallSpeedLimit()
    {
        if (rb.linearVelocityY <= -maxFallSpeed)
            rb.linearVelocityY = -maxFallSpeed;
    }
    
    #endregion
    
    
    #region 玩家的一些无视大部分条件的状态切换
    public void GetHurt()
    {
        if (isDead) 
            return;
        ChangeState(PlayerState.Hurt);
        isHurt = true;
    }
    public void GetStun()
    {
        if(isDead)
            return;
        ChangeState(PlayerState.Stun);
        isStun = true;
    }
    public void Die()
    {
        ChangeState(PlayerState.Die);
        isDead = true;
    }
    
    //TODO:可能需要修改写在状态机内部
    private void CheckForDefendInput()
    {
        if (!isHurt && !isDead && IsGroundDetected() && Input.GetMouseButton(1) &&!isDefending)
        {
            ChangeState(PlayerState.Defend);
            return;
        }
    }
    
    #endregion
    
    
    public bool checkCanUseSkill(SkillConfig _skillConfig)
    {
        return _skillConfig.releaseData.attackData.hitData.energyCost <= playerStats.GetCurrentEnergy();
    }
    
    
    #region 使用携程来判定玩家的完美防御,以及完美防御的触发函数
    //TODO:增加效果
    public void TriggerPerfectDefend()
    {
        Debug.Log("完美防御触发!");
        return;
    }
    
    //用于通知玩家激活播放格挡成功的动画
    public void TriggerNormalDefend()
    {
        //通知防御状态逻辑
        PlayerDefendState defenseState =  (PlayerDefendState)stateMachine.CurrentState;
        defenseState.TriggerDefend();
    }
    
    
    //玩家的完美防御判定计时器
    public void PerfectDefendCheck()
    {
        StartCoroutine(SetPerfectDefendBool());
    }
    private IEnumerator SetPerfectDefendBool()
    {
        canPerfectDefend = true;
        yield return new WaitForSeconds(perfectDefendWindow); 
        canPerfectDefend = false;
    }
    
    #endregion
}
