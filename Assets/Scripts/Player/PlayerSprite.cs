using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    public Animator anim;
    [SerializeField]
    private PlayerController player;
    
    #region 攻击相关的动画事件
    public void EnablePreInput()
    {
        player.canInput = true;
    }
    public void OnSkillOver()
    {
        player.CanSwitch = true;
    }

    public void EnableAttackBox(int _attackIndex)
    {
        player.attackBoxes[_attackIndex].SetActive(true);
        VFXManager.Instance.SpawnVFX(player.standAttackConfig[_attackIndex].releaseData.SpawnObj.prefab, 
            player.transform.position,new Vector3(0,0,0),player.standAttackConfig[_attackIndex].releaseData.SpawnObj.Time);
    }

    public void DisableAttackBox(int _attackIndex)
    {
        player.attackBoxes[_attackIndex].SetActive(false);
    }

    #endregion
}
