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
    public float duration = 1f;
    public float wave1wave2Duration = 2f;
    public List<GameObject> enemies1;
    public List<GameObject> enemies2;
    private bool hasTriggeredWave2;
    public void StartZoom()
    {
        StartCoroutine(ZoomCoroutine(targetSize, duration));
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
                if (Wave2allDead)
                {
                    Debug.Log("你赢了");
                }
            }
        }
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

    IEnumerator ActiveWave2AllEnemiesCoro()
    {
        yield return new WaitForSeconds(wave1wave2Duration);
        ActiveWave2AllEnemies();
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            StartZoom();
    }

    private void ActiveWave1AllEnemies()
    {
        foreach (var enemy in enemies1)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    private void ActiveWave2AllEnemies()
    {
        foreach (var enemy in enemies2)
        {
            enemy.gameObject.SetActive(true);
        }
    }
}
