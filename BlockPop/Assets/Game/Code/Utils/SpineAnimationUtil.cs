using System;
using Spine;
using Spine.Unity;
using UnityEngine.XR;

namespace Game
{
    
    /// <summary>
    /// Spine 动画工具
    /// </summary>
    public static class SpineAnimationUtil
    {

        /// <summary>
        /// 播放Spine动画
        /// </summary>
        /// <param name="target"></param>
        /// <param name="animName"></param>
        /// <param name="callback"></param>
        /// <param name="isLoop"></param>
        /// <param name="trackIndex"></param>
        public static void PlayAnim(this SkeletonGraphic target, string animName, Action callback = null, bool isLoop = false, int trackIndex = 0)
        {
            if (target != null)
            {
                var entry = target.AnimationState.SetAnimation(trackIndex, animName, isLoop);
                AnimationState.TrackEntryDelegate handler = null;
                handler = (e) =>
                {
                    callback?.Invoke();
                    e.Complete -= handler;
                };
                entry.Complete += handler;
            }
        }
        
        /// <summary>
        /// 停止Spine动画
        /// </summary>
        /// <param name="target"></param>
        /// <param name="trackIndex"></param>
        /// <param name="mixDuration"></param>
        public static void StopAnim(this SkeletonGraphic target, int trackIndex = 0, float mixDuration = 0)
        {
            target.AnimationState.SetEmptyAnimation(trackIndex, mixDuration);
        }
        
    }
}