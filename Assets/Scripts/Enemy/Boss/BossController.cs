using System.Collections;
using UnityEngine;

public class BossController : EnemyController
{
    [Header("Boss无人机技能")] 
    public GameObject bossDronePrefab;
    public int bossDroneSpawnNum;
    public float bossDroneSpwanDuration;
    public Transform[] bossDroneHookPos;
    public Vector2[] droneAttackingPosOffset;
    //无人机数量，用于追踪是否所有无人机回到boss处
    [HideInInspector] public int droneNum;
    
    [Header("Boss飞行参数")] 
    public Transform heightPoint;

    public float bossSpeed;
    public float bossClimbUpSpeed;
    
    [Header("Boss机械爪攻击技能")]
    public GameObject bossClaws;
    public int spwanClawNum;
    public float spwanClawDuration;
    
    [Header("Boss强杀技能参数")]
    public float chaseSpeed;
    public int currentForceKillChargeStage;
    public GameObject[] sparkVFX;
    
    //玩家位置
    private Vector2 playerPos;
    protected override void Start()
    {
        base.Start();
        playerPos = GameManager.instance.player.transform.position;
        //TODO:添加过场后，删除这个代码
        ChangeState(BossStates.Idle);
    }
    
    //状态机
    public void ChangeState(BossStates bossStates)
    {
        switch(bossStates)
        {
            case BossStates.Idle:
                stateMachine.ChangeState<BossIdleState>();
                break;
            case BossStates.DroneAttack:
                stateMachine.ChangeState<BossDroneAttackState>();
                break;
            case BossStates.SmashAttack:
                stateMachine.ChangeState<BossSmashAttackState>();
                break;
            case BossStates.ArmSmashAttack:
                stateMachine.ChangeState<BossArmAttackState>();
                break;
            case BossStates.ForceKillSkill:
                stateMachine.ChangeState<BossForceKillSkill>();
                break;
        }
    }

    #region 无人机生成技能携程
    public void SpwanDrone()
    {
        droneNum = 4;
        StartCoroutine(spwanDronesCoroutine(bossDroneSpawnNum,bossDroneSpwanDuration));
    }

    private IEnumerator spwanDronesCoroutine(int _numToSpwan, float spwanDuration)
    {
        Vector2 playerCurrentPos = GameManager.instance.player.transform.position;
        for (int i = 0; i < _numToSpwan; i++)
        {
            var newDrone = Instantiate(bossDronePrefab, transform.position, Quaternion.identity);
            BossDroneController droneScript = newDrone.GetComponent<BossDroneController>();
            droneScript.Init(droneAttackingPosOffset[i] + new Vector2(playerCurrentPos.x,playerCurrentPos.y),playerCurrentPos,1,this,bossDroneHookPos[i].position);
            yield return new WaitForSeconds(spwanDuration);
        }
    }
    #endregion
    
    #region 机械爪技能携程
    public void SpwanClaws()
    {
        StartCoroutine(spwanClawsCoroutine( spwanClawNum, spwanClawDuration)); 
    }
    private IEnumerator spwanClawsCoroutine(int _numToSpwan, float spwanClawDuration)
    {
        for (int i = 0; i < _numToSpwan; i++)
        {
            var newClaw = Instantiate(bossClaws, new Vector2(GameManager.instance.player.transform.position.x,13), Quaternion.identity);
            yield return new WaitForSeconds(spwanClawDuration);
        }
    }
    #endregion


    #region 强杀技能携程
    
    //TODO:需要优化
    public void enableRandomSparks()
    {
        int index = Random.Range(0, sparkVFX.Length);
        if (index == 0 || index == 2)
        {
            float yAxis = Random.Range(1.5f, 9f);
            VFXManager.Instance.SpawnVFX(sparkVFX[index],new Vector2(sparkVFX[index].transform.position.x,yAxis),sparkVFX[index].transform.rotation.eulerAngles,3f);
        }
        else
        {
            float xAxis = Random.Range(-6f, 11.5f);
            VFXManager.Instance.SpawnVFX(sparkVFX[index],new Vector2(xAxis,sparkVFX[index].transform.position.y),sparkVFX[index].transform.rotation.eulerAngles,3f);
        }
    }
    
    #endregion
}
