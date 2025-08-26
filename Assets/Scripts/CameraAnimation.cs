using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
public class CameraAnimation : MonoBehaviour
{
    public CinemachineCamera vcam;
    public CinemachineConfiner2D confiner;
    public float targetSize = 3f;
    public float duration = 1f;
    public List<GameObject> enemies;
    
    
    public void StartZoom()
    {
        StartCoroutine(ZoomCoroutine(targetSize, duration));
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
        ActiveAllEnemies();
        if (confiner != null)
            confiner.InvalidateLensCache();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            StartZoom();
    }

    private void ActiveAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }
}
