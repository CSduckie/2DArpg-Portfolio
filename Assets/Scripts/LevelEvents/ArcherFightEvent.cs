using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
public class ArcherFightEvent : EliteFightControl
{
    public ArcherController archer;
    private float targetSize = 10f;
    public float duration = 1f;
    public CinemachineCamera vcam;
    public CinemachineConfiner2D confiner;

    
    protected override void ReadyForEliteFight()
    {
        StartCoroutine(ActiveArcherAfterSec());
    }
    
    public void ArcherFightWon()
    {
        eliteFightCamera.Priority = 10;
        normalFollowCamera.Priority = 20;
        airwalls.SetActive(false);
        isEliteFightWon = true;
        StartCoroutine(WinEventCoroutine());
    }
    
    private IEnumerator ActiveArcherAfterSec()
    {
        yield return new WaitForSeconds(2f);
        archer.gameObject.SetActive(true);
        archer.StartEliteFight();
    }
    
    private IEnumerator WinEventCoroutine()
    {
        yield return new WaitForSeconds(3f);
        GameManager.instance.player.ChangeState(PlayerState.RestIn);
        yield return new WaitForSeconds(5f);
        StartZoom(targetSize,duration);
        yield return new WaitForSeconds(5f);
        SceneLoader.Instance.LoadSceneAsync("Level2");
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
        if (confiner != null)
            confiner.InvalidateLensCache();
    }
}
