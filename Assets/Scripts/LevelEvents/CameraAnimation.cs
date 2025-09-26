using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using System.Linq;
public class CameraAnimation : MonoBehaviour
{
    public CinemachineCamera vcam;
    public CinemachineConfiner2D confiner;
    public float targetSize = 3f;
    private float originalSize;
    public float duration = 1f;
    public float wave1wave2Duration = 2f;
    public List<GameObject> enemies1;
    public List<GameObject> enemies2;
    private bool hasTriggeredWave2;
    private bool sceneTransitionTriggered = false;

    private void Start()
    {
        originalSize = vcam.Lens.OrthographicSize;
    }
    
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
                    StartCoroutine(WinEventCoroutine());
                }
            }
        }
    }
    
    #region 玩家关卡一获胜后逻辑

    private IEnumerator WinEventCoroutine()
    {
        yield return new WaitForSeconds(5f);
        StartZoom(originalSize,duration);
        GameManager.instance.player.ChangeState(PlayerState.RestIn);
        yield return new WaitForSeconds(5f);
        SceneLoader.Instance.LoadSceneAsync("Level2");
    }
    
    #endregion
    
    

    #region 玩家进入触发逻辑
    
    //触发器判断玩家进入
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            //玩家进入，开始镜头动画
            StartZoom(targetSize,duration);
    }
    private void StartZoom(float _targetSize, float _duration)
    {
        //携程调用
        StartCoroutine(ZoomCoroutine(_targetSize, _duration));
    }
    IEnumerator ZoomCoroutine(float newSize, float time)
    {
        var lens = vcam.Lens; // 取结构体
        float startSize = lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            lens.OrthographicSize = Mathf.Lerp(startSize, newSize, elapsed / time);
            vcam.Lens = lens; // 必须写回，结构体不自动引用

            // 关键：更新 Confiner 边界缓存
            if (confiner != null)
                confiner.InvalidateLensCache();

            yield return null;
            lens = vcam.Lens; // 重新取出最新值
        }

        lens.OrthographicSize = newSize;
        vcam.Lens = lens;
        ActiveWave1AllEnemies();
        if (confiner != null)
            confiner.InvalidateLensCache();
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
