using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Collect sprites from specified path
    /// </summary>
    [ExecuteInEditMode]
    public class SpriteCollection : MonoBehaviour
    {
        public string SpritePath;

        [Header("Body Parts")]
        public List<SpriteGroupEntry> Head;
        public List<SpriteGroupEntry> Ears;
        public List<SpriteGroupEntry> Hair;
        public List<SpriteGroupEntry> Eyebrows;
        public List<SpriteGroupEntry> Eyes;
        public List<SpriteGroupEntry> Mouth;
        public List<SpriteGroupEntry> Beard;
        public List<SpriteGroupEntry> BodyArmL;
        public List<SpriteGroupEntry> BodyArmR;
        public List<SpriteGroupEntry> BodyForearmL;
        public List<SpriteGroupEntry> BodyForearmR;
        public List<SpriteGroupEntry> BodyHandL;
        public List<SpriteGroupEntry> BodyHandR;
        public List<SpriteGroupEntry> BodyLeg;
        public List<SpriteGroupEntry> BodyPelvis;
        public List<SpriteGroupEntry> BodyShin;
        public List<SpriteGroupEntry> BodyTorso;

        [Header("Equipment")]
        public List<SpriteGroupEntry> Helmet;
        public List<SpriteGroupEntry> HelmetMask;
        public List<SpriteGroupEntry> Back;
        public List<SpriteGroupEntry> MeleeWeapon1H;
        public List<SpriteGroupEntry> MeleeWeapon2H;
        public List<SpriteGroupEntry> Shield;
        public List<SpriteGroupEntry> BowArrow;
        public List<SpriteGroupEntry> BowLimb;
        public List<SpriteGroupEntry> BowRiser;
        public List<SpriteGroupEntry> ArmorArmL;
        public List<SpriteGroupEntry> ArmorArmR;
        public List<SpriteGroupEntry> ArmorForearmL;
        public List<SpriteGroupEntry> ArmorForearmR;
        public List<SpriteGroupEntry> ArmorHandL;
        public List<SpriteGroupEntry> ArmorHandR;
        public List<SpriteGroupEntry> ArmorLeg;
        public List<SpriteGroupEntry> ArmorPelvis;
        public List<SpriteGroupEntry> ArmorShin;
        public List<SpriteGroupEntry> ArmorTorso;

        #if UNITY_EDITOR

        /// <summary>
        /// Called automatically when something was changed
        /// </summary>
        public void OnValidate()
        {
            Refresh();
        }

        /// <summary>
        /// Read all sprites from specified path again
        /// </summary>
        public void Refresh()
        {
            Head = LoadSprites(SpritePath + "/BodyParts/Head");
            Ears = LoadSprites(SpritePath + "/BodyParts/Ears");
            Hair = LoadSprites(SpritePath + "/BodyParts/Hair");
            Eyebrows = LoadSprites(SpritePath + "/BodyParts/Eyebrows");
            Eyes = LoadSprites(SpritePath + "/BodyParts/Eyes");
            Mouth = LoadSprites(SpritePath + "/BodyParts/Mouth");
            Beard = LoadSprites(SpritePath + "/BodyParts/Beard");
            BodyArmL = LoadSprites(SpritePath + "/BodyParts/Body", "ArmL");
            BodyArmR = LoadSprites(SpritePath + "/BodyParts/Body", "ArmR");
            BodyForearmL = LoadSprites(SpritePath + "/BodyParts/Body", "ForearmL");
            BodyForearmR = LoadSprites(SpritePath + "/BodyParts/Body", "ForearmR");
            BodyHandL = LoadSprites(SpritePath + "/BodyParts/Body", "HandL");
            BodyHandR = LoadSprites(SpritePath + "/BodyParts/Body", "HandR");
            BodyLeg = LoadSprites(SpritePath + "/BodyParts/Body", "Leg");
            BodyPelvis = LoadSprites(SpritePath + "/BodyParts/Body", "Pelvis");
            BodyShin = LoadSprites(SpritePath + "/BodyParts/Body", "Shin");
            BodyTorso = LoadSprites(SpritePath + "/BodyParts/Body", "Torso");
            Helmet = LoadSprites(SpritePath + "/Equipment/Helmet");
            HelmetMask = LoadSprites(SpritePath + "/Equipment/HelmetMask");
            ArmorArmL = LoadSprites(SpritePath + "/Equipment/Armor", "ArmL");
            ArmorArmR = LoadSprites(SpritePath + "/Equipment/Armor", "ArmR");
            ArmorForearmL = LoadSprites(SpritePath + "/Equipment/Armor", "ForearmL");
            ArmorForearmR = LoadSprites(SpritePath + "/Equipment/Armor", "ForearmR");
            ArmorHandL = LoadSprites(SpritePath + "/Equipment/Armor", "HandL");
            ArmorHandR = LoadSprites(SpritePath + "/Equipment/Armor", "HandR");
            ArmorLeg = LoadSprites(SpritePath + "/Equipment/Armor", "Leg");
            ArmorPelvis = LoadSprites(SpritePath + "/Equipment/Armor", "Pelvis");
            ArmorShin = LoadSprites(SpritePath + "/Equipment/Armor", "Shin");
            ArmorTorso = LoadSprites(SpritePath + "/Equipment/Armor", "Torso");
            Back = LoadSprites(SpritePath + "/Equipment/Back");
            MeleeWeapon1H = LoadSprites(SpritePath + "/Equipment/MeleeWeapon1H", 1);
            MeleeWeapon2H = LoadSprites(SpritePath + "/Equipment/MeleeWeapon2H", 1);
            Shield = LoadSprites(SpritePath + "/Equipment/Shield");
            BowArrow = LoadSprites(SpritePath + "/Equipment/Bow", "Arrow");
            BowLimb = LoadSprites(SpritePath + "/Equipment/Bow", "Limb");
            BowRiser = LoadSprites(SpritePath + "/Equipment/Bow", "Riser");
        }

        private static List<SpriteGroupEntry> LoadSprites(string path, int nesting = 0)
        {
            return System.IO.Directory.GetFiles(path, "*.png", System.IO.SearchOption.AllDirectories)
                .Select(i => new SpriteGroupEntry(System.IO.Path.GetFileNameWithoutExtension(i), GetCollectionName(i, nesting), UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(i))).ToList();
        }

        private static List<SpriteGroupEntry> LoadSprites(string path, string spriteName, int nesting = 0)
        {
            var list = new List<SpriteGroupEntry>();

            foreach (var image in System.IO.Directory.GetFiles(path, "*.png", System.IO.SearchOption.AllDirectories))
            {
                var p = image;
                var entries = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(image).Where(i => i is Sprite && i.name == spriteName).Cast<Sprite>()
                    .Select(i => new SpriteGroupEntry(System.IO.Path.GetFileNameWithoutExtension(p), GetCollectionName(p, nesting), i));

                list.AddRange(entries);
            }

            return list;
        }

        private static string GetCollectionName(string path, int nesting)
        {
            var parent = System.IO.Directory.GetParent(path);

            for (var i = 0; i < nesting; i++)
            {
                if (parent != null) parent = parent.Parent;
            }

            if (parent == null) throw new NotSupportedException();

            return parent.Name;
        }

        #endif
    }
}