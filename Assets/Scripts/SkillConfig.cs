using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Config/Skill")]
public class SkillConfig : ScriptableObject
{
    //技能的动画名称
    public string AnimationName;
    public string EndAnimationName;
    public Skill_ReleaseData releaseData;
}

/// <summary>
/// 技能释放数据
/// </summary>
[Serializable]
public class Skill_ReleaseData
{
    //播放粒子效果
    public Skill_SpawnObj SpawnObj;
    //技能音效
    public AudioClip audioClip;
    //判定技能是否可以旋转
    public bool canRotate;
    public Skill_AttackData attackData;

}

/// <summary>
/// 技能产生物体
/// </summary>
[Serializable]
public class Skill_SpawnObj
{
    //产生的预制体
    public GameObject prefab;
    //生成的音效
    public AudioClip audioClip;
    //生成位置
    public Vector3 position;
    //生成角度
    public Vector3 rotation;
    //缩放
    public Vector3 Scale = Vector3.one;
    //延迟Time
    public float Time;
    
}

/// <summary>
/// 技能攻击数据
/// </summary>

[Serializable]
public class Skill_AttackData
{
    public AudioClip audioClip;

    //TODO:命中数据::

    public Skill_HitData hitData;
    //屏幕震动
    public float ScreenImpulseValue;
    //卡肉效果
    public float FreezeFrameTime;
    //游戏时间停止
    public float FreezeGameTime;

}
/// <summary>
/// 受伤数据，用于传递实际伤害等
/// </summary>

[Serializable]
public class Skill_HitData
{
    //伤害数值
    //用于回复技能时，则是用作回复数值
    public float value;
    public Skill_SpawnObj[] SpawnObj;
    public float energyCost;
    //积累的失衡能量值
    public float stunValue;
    //给敌人的硬直Time
    public float HardTime;
    //击飞击退的程度
    public Vector2 RepelVelocity;
    //击飞击退的过度事件(力度)
    public float RepelTime;
}
