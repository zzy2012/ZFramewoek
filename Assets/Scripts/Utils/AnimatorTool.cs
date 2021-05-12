using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画工具类
/// </summary>
public static class AnimatorTool
{
    /// <summary>
    /// 给动画添加新的事件
    /// </summary>
    /// <param name="_Animator"> 人物的状态机</param>
    /// <param name="_ClipName">AnimationClip的名称</param>
    /// <param name="_EventFunctionName">方法名</param>
    /// <param name="_time">时间</param>
    public static void AddAnimationEvent(Animator _Animator, string _ClipName, string _EventFunctionName, float _time)
    {
        bool isExit = false;
        AnimationClip[] _clips = _Animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < _clips.Length; i++)
        {
            if (_clips[i].name == _ClipName)
            {
                AnimationEvent _event = new AnimationEvent();
                _event.functionName = _EventFunctionName;
                _event.time = _time;
                _clips[i].AddEvent(_event);
                isExit = true;
                break;
            }
        }
        _Animator.Rebind();
        if (isExit == false)
        {
            Debug.LogError("在" + _ClipName + "上添加动画事件" + _EventFunctionName + "失败");
        }
    }
    /// <summary>
    /// 给动画添加新的事件
    /// </summary>
    /// <param name="_Animator"> 人物的状态机</param>
    /// <param name="_ClipName">AnimationClip的名称</param>
    /// <param name="_EventFunctionName">方法名</param>
    /// <param name="_time">时间</param>
    public static void AddAnimationEvent(Animator _Animator, string _ClipName, string _EventFunctionName,string _StringParameter,float _time)
    {
        
        bool isExit = false;
        AnimationClip[] _clips = _Animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < _clips.Length; i++)
        {
            if (_clips[i].name == _ClipName)
            {
                AnimationEvent _event = new AnimationEvent();
                _event.functionName = _EventFunctionName;
                _event.time = _time;
                _event.stringParameter = _StringParameter;
                _clips[i].AddEvent(_event);
                isExit = true;
                break;
            }
        }
        _Animator.Rebind();
        if (isExit == false)
        {
            Debug.LogError("在" + _ClipName + "上添加动画事件" + _EventFunctionName + "失败");
        }
    }
    /// <summary>
    /// 清除所有的事件
    /// </summary>
    public static void ClearAllEvent(Animator animator)
    {
        AnimationClip[] _clips = animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < _clips.Length; i++)
        {
            _clips[i].events = default(AnimationEvent[]);
        }
    }
    /// <summary>
    /// 根据动画名字获取动画片段
    /// </summary>
    /// <param name="animator">Animator</param>
    /// <param name="clip">动画片段</param>
    /// <returns></returns>
    ///获取动画状态机animator的动画clip的播放持续时长
    public static float GetClipLength(Animator animator, string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float clipLength=0f;
        foreach (var item in clips)
        {
            if (item.name == clipName)
            {
                clipLength = item.length;
            }
        }
        if (clipLength == 0)
        {
            Debug.LogError("当前动画状态机中没有名为:" + clipName + "的动画片段");
;        }
        return clipLength;
    }
}
