using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System.Linq;
public class EnemyFight : MonoBehaviour
{
    public CinemachineCamera eliteFightCamera;
    public CinemachineCamera normalFollowCamera;
    
    
    public float wave1wave2Duration = 2f;
    public List<GameObject> enemies1;
    public List<GameObject> enemies2;
    private bool hasTriggeredWave2;
    private bool sceneTransitionTriggered = false;
    public GameObject airWall;
    
    private void Update()
    {
        // 检查第一波是否全部死亡
        bool allDead = enemies1.All(enemy => enemy.GetComponent<EnemyController>().isDead);

        if (allDead && !hasTriggeredWave2)
        {
            StartCoroutine(ActiveWave2AllEnemiesCoro());
            hasTriggeredWave2 = true; // 防止多次触发
        }
        
        //如果hasTriggeredWave2，监听是否第二波敌人全死了，如果是，那么就进入专场逻辑
        if (hasTriggeredWave2)
        {
            bool Wave2allDead = enemies2.All(enemy => enemy.GetComponent<EnemyController>().isDead);
            {
                if (Wave2allDead && !sceneTransitionTriggered)
                {
                    sceneTransitionTriggered = true;
                    Debug.Log("你赢了");
                    airWall.SetActive(false);
                    eliteFightCamera.Priority = 10;
                    normalFollowCamera.Priority = 20;
                }
            }
        }
    }

    

    #region 玩家进入触发逻辑
    
    //触发器判断玩家进入
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            eliteFightCamera.Priority = 20;
            normalFollowCamera.Priority = 10;
            ActiveWave1AllEnemies();
            airWall.SetActive(true);
        }
    }
    
    private void ActiveWave1AllEnemies()
    {
        foreach (var enemy in enemies1)
        {
            enemy.gameObject.SetActive(true);
        }
    }
    
    IEnumerator ActiveWave2AllEnemiesCoro()
    {
        yield return new WaitForSeconds(wave1wave2Duration);
        ActiveWave2AllEnemies();
    }
    
    private void ActiveWave2AllEnemies()
    {
        foreach (var enemy in enemies2)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    #endregion


}
