using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void SpawnVFX(GameObject _vfxObject, Vector2 _position,Vector3 _rotation, Vector3 _scale, float lifeTime)
    {
        if (_vfxObject == null) return;
        GameObject vfx = Instantiate(_vfxObject, _position, Quaternion.Euler(_rotation));
        vfx.transform.localScale = _scale;
        Destroy(vfx, lifeTime); // 一定时间后销毁，避免内存泄漏
    }
}
