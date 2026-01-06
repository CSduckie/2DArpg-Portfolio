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
        bool isFacingRight = player.isFacingRight;
        if(!isFacingRight)
        {
            VFXManager.Instance.SpawnVFX(player.standAttackConfig[_attackIndex].releaseData.SpawnObj.prefab, 
            new Vector3(player.transform.position.x - player.standAttackConfig[_attackIndex].releaseData.SpawnObj.position.x, player.transform.position.y + player.standAttackConfig[_attackIndex].releaseData.SpawnObj.position.y, player.transform.position.z + player.standAttackConfig[_attackIndex].releaseData.SpawnObj.position.z),
            new Vector3(player.standAttackConfig[_attackIndex].releaseData.SpawnObj.rotation.x, 180-player.standAttackConfig[_attackIndex].releaseData.SpawnObj.rotation.y, player.standAttackConfig[_attackIndex].releaseData.SpawnObj.rotation.z),
            player.standAttackConfig[_attackIndex].releaseData.SpawnObj.Scale,
            player.standAttackConfig[_attackIndex].releaseData.SpawnObj.Time);
        }
        else
        {
            VFXManager.Instance.SpawnVFX(player.standAttackConfig[_attackIndex].releaseData.SpawnObj.prefab, 
            player.transform.position + player.standAttackConfig[_attackIndex].releaseData.SpawnObj.position,
            player.standAttackConfig[_attackIndex].releaseData.SpawnObj.rotation,
            player.standAttackConfig[_attackIndex].releaseData.SpawnObj.Scale,
            player.standAttackConfig[_attackIndex].releaseData.SpawnObj.Time);
        }
    }

    public void DisableAttackBox(int _attackIndex)
    {
        player.attackBoxes[_attackIndex].SetActive(false);
    }

    #endregion
}
