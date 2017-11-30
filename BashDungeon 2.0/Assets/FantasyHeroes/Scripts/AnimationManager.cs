using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Play animation from character editor
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        public Character Dummy;

        [Header("UI")]
        public Text ClipName;

        private readonly List<string> _animationClips = new List<string> { "Stand", "Alert", "Attack", "AttackLunge", "Cast", "Walk", "Run", "Jump", "Hit", "Die" };
        private int _animationClipIndex;

        /// <summary>
        /// Called automatically on app start
        /// </summary>
        public void Start()
        {
            Reset();
        }

        /// <summary>
        /// Reset animation on start and weapon type change
        /// </summary>
        public void Reset()
        {
            PlayAnimation(0);
        }

        /// <summary>
        /// Change animation and play it
        /// </summary>
        /// <param name="direction">Pass 1 or -1 value to play forward / reverse</param>
        public void PlayAnimation(int direction)
        {
            _animationClipIndex += direction;

            if (_animationClipIndex < 0)
            {
                _animationClipIndex = _animationClips.Count - 1;
            }

            if (_animationClipIndex >= _animationClips.Count)
            {
                _animationClipIndex = 0;
            }

            var clipName = _animationClips[_animationClipIndex];

            clipName = ResolveAnimatiobClip(clipName);

            Dummy.Animator.SetTrigger("LoopAll");
            Dummy.Animator.Play(clipName);
            ClipName.text = clipName;
        }

        private string ResolveAnimatiobClip(string clipName)
        {
            switch (clipName)
            {
                case "Alert":
                    switch (Dummy.WeaponType)
                    {
                        case WeaponType.Melee1H:
                        case WeaponType.MeleeTween:
                        case WeaponType.Bow: return "Alert1H";
                        case WeaponType.Melee2H: return "Alert2H";
                        default: throw new NotImplementedException();
                    }
                case "Attack":
                    switch (Dummy.WeaponType)
                    {
                        case WeaponType.Melee1H: return "Attack1H";
                        case WeaponType.Melee2H: return "Attack2H";
                        case WeaponType.MeleeTween: return "AttackTween";
                        case WeaponType.Bow: return "Shot";
                        default: throw new NotImplementedException();
                    }
                case "AttackLunge":
                    switch (Dummy.WeaponType)
                    {
                        case WeaponType.Melee1H:
                        case WeaponType.Melee2H:
                        case WeaponType.MeleeTween:
                        case WeaponType.Bow: return "AttackLunge1H";
                        default: throw new NotImplementedException();
                    }
                case "Cast":
                    switch (Dummy.WeaponType)
                    {
                        case WeaponType.Melee1H:
                        case WeaponType.Melee2H:
                        case WeaponType.MeleeTween:
                        case WeaponType.Bow: return "Cast1H";
                        default: throw new NotImplementedException();
                    }
                default: return clipName;
            }
        }
    }
}