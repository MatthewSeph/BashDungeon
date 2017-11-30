using System.Collections.Generic;
using UnityEngine;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Character presentation in editor
    /// </summary>
    public class Character : MonoBehaviour
    {
        [Header("Body")]
        public Sprite Head;
        public Sprite Ears;
        public Sprite Hair;
        public SpriteMask HairMask;
        public Sprite Eyebrows;
        public Sprite Eyes;
        public Sprite Mouth;
        public Sprite Beard;
        public Sprite BodyArmL;
        public Sprite BodyArmR;
        public Sprite BodyForearmL;
        public Sprite BodyForearmR;
        public Sprite BodyHandL;
        public Sprite BodyHandR;
        public Sprite BodyLeg;
        public Sprite BodyPelvis;
        public Sprite BodyShin;
        public Sprite BodyTorso;

        [Header("Equipment")]
        public Sprite Helmet;
        public Sprite HelmetMask;
        public Sprite PrimaryMeleeWeapon;
        public Sprite SecondaryMeleeWeapon;
        public Sprite ArmorArmL;
        public Sprite ArmorArmR;
        public Sprite ArmorForearmL;
        public Sprite ArmorForearmR;
        public Sprite ArmorHandL;
        public Sprite ArmorHandR;
        public Sprite ArmorLeg;
        public Sprite ArmorPelvis;
        public Sprite ArmorShin;
        public Sprite ArmorTorso;
        public Sprite Back;
        public Sprite Shield;
        public Sprite BowArrow;
        public Sprite BowLimb;
        public Sprite BowRiser;

        [Header("Renderers")]
        public SpriteRenderer HeadRenderer;
        public SpriteRenderer EarsRenderer;
        public SpriteRenderer HairRenderer;
        public SpriteRenderer EyebrowsRenderer;
        public SpriteRenderer EyesRenderer;
        public SpriteRenderer MouthRenderer;
        public SpriteRenderer BeardRenderer;
        public SpriteRenderer BodyArmLRenderer;
        public List<SpriteRenderer> BodyArmRRenderers;
        public List<SpriteRenderer> BodyForearmLRenderers;
        public List<SpriteRenderer> BodyForearmRRenderers;
        public List<SpriteRenderer> BodyHandLRenderers;
        public List<SpriteRenderer> BodyHandRRenderers;
        public List<SpriteRenderer> BodyLegRenderers;
        public SpriteRenderer BodyPelvisRenderer;
        public List<SpriteRenderer> BodyShinRenderers;
        public SpriteRenderer BodyTorsoRenderer;
        public SpriteRenderer HelmetRenderer;
        public SpriteRenderer PrimaryMeleeWeaponRenderer;
        public SpriteRenderer SecondaryMeleeWeaponRenderer;
        public SpriteRenderer ArmorArmLRenderer;
        public List<SpriteRenderer> ArmorArmRRenderers;
        public List<SpriteRenderer> ArmorForearmLRenderers;
        public List<SpriteRenderer> ArmorForearmRRenderers;
        public List<SpriteRenderer> ArmorHandLRenderers;
        public List<SpriteRenderer> ArmorHandRRenderers;
        public List<SpriteRenderer> ArmorLegRenderers;
        public SpriteRenderer ArmorPelvisRenderer;
        public List<SpriteRenderer> ArmorShinRenderers;
        public SpriteRenderer ArmorTorsoRenderer;
        public SpriteRenderer BackRenderer;
        public SpriteRenderer ShieldRenderer;
        public List<SpriteRenderer> BowArrowRenderers;
        public List<SpriteRenderer> BowLimbRenderers;
        public List<SpriteRenderer> BowRiserRenderers;

        [Header("Animation")]
        public Animator Animator;
        public WeaponType WeaponType;

        /// <summary>
        /// Called automatically when something was changed
        /// </summary>
        public void OnValidate()
        {
            if (Head == null) return;

            Initialize();
        }
        
        /// <summary>
        /// Called automatically when object was started
        /// </summary>
        public void Start()
        {
            HairMask.isCustomRangeActive = true;
            HairMask.frontSortingOrder = HairRenderer.sortingOrder;
            HairMask.backSortingOrder = HairRenderer.sortingOrder - 1;
        }

        /// <summary>
        /// Initialize character renderers with selected sprites
        /// </summary>
        public void Initialize()
        {
            HeadRenderer.sprite = Head;
            HairRenderer.sprite = Hair;
            HairRenderer.maskInteraction = HelmetMask == null ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleOutsideMask;
            HairMask.sprite = HelmetMask;
            EarsRenderer.sprite = Ears;
            EyebrowsRenderer.sprite = Eyebrows;
            EyesRenderer.sprite = Eyes;
            MouthRenderer.sprite = Mouth;
            BeardRenderer.sprite = Beard;
            EarsRenderer.sprite = Ears;
            BodyArmLRenderer.sprite = BodyArmL;
            BodyArmRRenderers.ForEach(i => i.sprite = BodyArmR);
            BodyForearmLRenderers.ForEach(i => i.sprite = BodyForearmL);
            BodyForearmRRenderers.ForEach(i => i.sprite = BodyForearmR);
            BodyHandLRenderers.ForEach(i => i.sprite = BodyHandL);
            BodyHandRRenderers.ForEach(i => i.sprite = BodyHandR);
            BodyLegRenderers.ForEach(i => i.sprite = BodyLeg);
            BodyPelvisRenderer.sprite = BodyPelvis;
            BodyShinRenderers.ForEach(i => i.sprite = BodyShin);
            BodyTorsoRenderer.sprite = BodyTorso;
            HelmetRenderer.sprite = Helmet;
            PrimaryMeleeWeaponRenderer.sprite = PrimaryMeleeWeapon;
            SecondaryMeleeWeaponRenderer.sprite = SecondaryMeleeWeapon;
            ArmorArmLRenderer.sprite = ArmorArmL;
            ArmorArmRRenderers.ForEach(i => i.sprite = ArmorArmR);
            ArmorForearmLRenderers.ForEach(i => i.sprite = ArmorForearmL);
            ArmorForearmRRenderers.ForEach(i => i.sprite = ArmorForearmR);
            ArmorHandLRenderers.ForEach(i => i.sprite = ArmorHandL);
            ArmorHandRRenderers.ForEach(i => i.sprite = ArmorHandR);
            ArmorLegRenderers.ForEach(i => i.sprite = ArmorLeg);
            ArmorPelvisRenderer.sprite = ArmorPelvis;
            ArmorShinRenderers.ForEach(i => i.sprite = ArmorShin);
            ArmorTorsoRenderer.sprite = ArmorTorso;
            BackRenderer.sprite = Back;
            ShieldRenderer.sprite = Shield;
            //BowArrowRenderer.sprite = BowArrow;
            BowLimbRenderers.ForEach(i => i.sprite = BowLimb);
            BowRiserRenderers.ForEach(i => i.sprite = BowRiser);

            PrimaryMeleeWeaponRenderer.enabled = WeaponType == WeaponType.Melee1H || WeaponType == WeaponType.MeleeTween || WeaponType == WeaponType.Melee2H;
            SecondaryMeleeWeaponRenderer.enabled = WeaponType == WeaponType.MeleeTween;
            ShieldRenderer.enabled = WeaponType == WeaponType.Melee1H;
            //BowArrowRenderers.ForEach(i => i.enabled = WeaponType == WeaponType.Bow);
            BowRiserRenderers.ForEach(i => i.enabled = WeaponType == WeaponType.Bow);
            BowLimbRenderers.ForEach(i => i.enabled = WeaponType == WeaponType.Bow);
        }

        public void SetBody(Sprite armL, Sprite armR, Sprite forearmL, Sprite forearmR, Sprite handL, Sprite handR, Sprite leg, Sprite pelvis, Sprite shin, Sprite torso)
        {
            BodyArmL = armL;
            BodyArmR = armR;
            BodyForearmL = forearmL;
            BodyForearmR = forearmR;
            BodyHandL = handL;
            BodyHandR = handR;
            BodyLeg = leg;
            BodyPelvis = pelvis;
            BodyShin = shin;
            BodyTorso = torso;
        }

        public void SetArmor(Sprite armL, Sprite armR, Sprite forearmL, Sprite forearmR, Sprite handL, Sprite handR, Sprite leg, Sprite pelvis, Sprite shin, Sprite torso)
        {
            ArmorArmL = armL;
            ArmorArmR = armR;
            ArmorForearmL = forearmL;
            ArmorForearmR = forearmR;
            ArmorHandL = handL;
            ArmorHandR = handR;
            ArmorLeg = leg;
            ArmorPelvis = pelvis;
            ArmorShin = shin;
            ArmorTorso = torso;
        }

        public void SetUpperArmor(Sprite armL, Sprite armR, Sprite forearmL, Sprite forearmR, Sprite torso)
        {
            ArmorArmL = armL;
            ArmorArmR = armR;
            ArmorForearmL = forearmL;
            ArmorForearmR = forearmR;
            ArmorTorso = torso;
        }

        public void SetLowerArmor(Sprite leg, Sprite pelvis)
        {
            ArmorLeg = leg;
            ArmorPelvis = pelvis;
        }

        public void SetGloves(Sprite handL, Sprite handR)
        {
            ArmorHandL = handL;
            ArmorHandR = handR;
        }

        public void SetBow(Sprite arrow, Sprite limb, Sprite riser)
        {
            BowArrow = arrow;
            BowLimb = limb;
            BowRiser = riser;
        }

        public void BuildWeaponTrails()
        {
            PrimaryMeleeWeaponRenderer.GetComponentInChildren<MeleeWeaponTrail>().Build();
            SecondaryMeleeWeaponRenderer.GetComponentInChildren<MeleeWeaponTrail>().Build();
        }
    }
}