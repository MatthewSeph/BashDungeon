using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Defines editor's behaviour
    /// </summary>
    public class CharacterEditor : MonoBehaviour
    {
        public SpriteCollection SpriteCollection;
        public AnimationManager AnimationManager;
        public Character Dummy;

        [Header("UI")]
        public GameObject Editor;
        public GameObject CommonPalette;
        public GameObject SkinPalette;
        public Dropdown HeadDropdown;
        public Dropdown EarsDropdown;
        public Dropdown HairDropdown;
        public Dropdown EyebrowsDropdown;
        public Dropdown EyesDropdown;
        public Dropdown MouthDropdown;
        public Dropdown BeardDropdown;
        public Dropdown BodyDropdown;
        public Dropdown HelmetDropdown;
        public Dropdown ArmorDropdown;
        public Dropdown ArmorArmLDropdown;
        public Dropdown ArmorArmRDropdown;
        public Dropdown ArmorForearmLDropdown;
        public Dropdown ArmorForearmRDropdown;
        public Dropdown ArmorHandLDropdown;
        public Dropdown ArmorHandRDropdown;
        public Dropdown ArmorLegDropdown;
        public Dropdown ArmorPelvisDropdown;
        public Dropdown ArmorShinDropdown;
        public Dropdown ArmorTorsoDropdown;
        public Dropdown UpperArmorDropdown;
        public Dropdown LowerArmorDropdown;
        public Dropdown GlovesDropdown;
        public Dropdown BootsDropdown;
        public Dropdown BackDropdown;
        public Dropdown MeleeWeapon1HDropdown;
        public Dropdown MeleeWeaponPairedDropdown;
        public Dropdown MeleeWeapon2HDropdown;
        public Dropdown ShieldDropdown;
        public Dropdown BowDropdown;
        public List<Button> EditorOnlyButtons;
        public GameObject Grid;
        public GameObject GridArmorParts;
        public List<SpriteRenderer> HandEquipment;

        /// <summary>
        /// Called automatically on app start
        /// </summary>
        public void Start()
        {
            Dummy.Initialize();
            InitializeDropdowns();
            EditorOnlyButtons.ForEach(i => i.interactable = Application.isEditor);
        }

        private List<SpriteRenderer> _palleteTargets;

        /// <summary>
        /// Open palette to change sprite color
        /// </summary>
        /// <param name="target">Pass one of the following values: Head, Ears, Body, Hair, Eyes, Mouth</param>
        public void OpenPalette(string target)
        {
            var palette = CommonPalette;

            switch (target)
            {
                case "Head": _palleteTargets = new List<SpriteRenderer> { Dummy.HeadRenderer }; palette = SkinPalette; break;
                case "Ears": _palleteTargets = new List<SpriteRenderer> { Dummy.EarsRenderer }; palette = SkinPalette; break;
                case "Body":
                {
                    _palleteTargets = new List<SpriteRenderer> { Dummy.BodyArmLRenderer, Dummy.BodyPelvisRenderer, Dummy.BodyTorsoRenderer };
                    _palleteTargets.AddRange(Dummy.BodyArmRRenderers);
                    _palleteTargets.AddRange(Dummy.BodyArmRRenderers);
                    _palleteTargets.AddRange(Dummy.BodyForearmLRenderers);
                    _palleteTargets.AddRange(Dummy.BodyForearmRRenderers);
                    _palleteTargets.AddRange(Dummy.BodyHandLRenderers);
                    _palleteTargets.AddRange(Dummy.BodyHandRRenderers);
                    _palleteTargets.AddRange(Dummy.BodyLegRenderers);
                    _palleteTargets.AddRange(Dummy.BodyShinRenderers);
                    palette = SkinPalette;
                    break;
                }
                case "Hair": _palleteTargets = new List<SpriteRenderer> { Dummy.HairRenderer }; break;
                case "Eyebrows": _palleteTargets = new List<SpriteRenderer> { Dummy.EyebrowsRenderer }; break;
                case "Eyes": _palleteTargets = new List<SpriteRenderer> { Dummy.EyesRenderer }; break;
                case "Mouth": _palleteTargets = new List<SpriteRenderer> { Dummy.MouthRenderer }; break;
                case "Beard": _palleteTargets = new List<SpriteRenderer> { Dummy.BeardRenderer }; break;
            }

            #if UNITY_EDITOR

            EditorGUIColorField.Open(_palleteTargets[0].color);
            palette.SetActive(false);

            #else

            Editor.SetActive(false);
            palette.SetActive(true);

            #endif
        }

        /// <summary>
        /// Close palette
        /// </summary>
        public void ClosePalette()
        {
            CommonPalette.SetActive(false);
            SkinPalette.SetActive(false);
            Editor.SetActive(true);
        }

        /// <summary>
        /// Remove all equipment
        /// </summary>
        public void Reset()
        {
            Dummy.ArmorArmL = Dummy.ArmorArmR = Dummy.ArmorForearmL = Dummy.ArmorForearmR = Dummy.ArmorHandL = Dummy.ArmorHandR = Dummy.ArmorLeg = Dummy.ArmorPelvis = Dummy.ArmorShin = Dummy.ArmorTorso = null;
            Dummy.Helmet = Dummy.HelmetMask = Dummy.Back = Dummy.PrimaryMeleeWeapon = Dummy.SecondaryMeleeWeapon = Dummy.Shield = Dummy.BowArrow = Dummy.BowLimb = Dummy.BowRiser = null;
            Dummy.Initialize();
            Dummy.BuildWeaponTrails();
            InitializeDropdowns();
        }

        /// <summary>
        /// Flip character by X-axis
        /// </summary>
        public void Flip()
        {
            var scale = Dummy.transform.localScale;

            scale.x *= -1;
            Dummy.transform.localScale = scale;
        }

        #if UNITY_EDITOR

        /// <summary>
        /// Save character to prefab
        /// </summary>
        public void Save()
        {
            var path = UnityEditor.EditorUtility.SaveFilePanel("Save character prefab", "Assets/FantasyHeroes/Prefabs", "New character", "prefab");

            if (path.Length > 0)
            {
                path = "Assets" + path.Replace(Application.dataPath, null);
                Save(path);
            }
        }

        public void Save(string path)
        {
            UnityEditor.PrefabUtility.CreatePrefab(path, Dummy.gameObject);
            Debug.LogFormat("Prefab saved as {0}", path);
        }

        public void Load(string path)
        {
            Debug.LogWarning("This feature is available only in full asset version: http://u3d.as/QCQ");
        }

        #endif

        /// <summary>
        /// Navigate to URL
        /// </summary>
        public void Navigate(string url)
        {
            Application.OpenURL(url);
        }

        /// <summary>
        /// Pick color and apply to sprite
        /// </summary>
        public void PickColor(Color color)
        {
            _palleteTargets.ForEach(i => i.color = color);
        }

        /// <summary>
        /// Switch armor parts mode
        /// </summary>
        public void SwitchToArmorParts()
        {
            Grid.SetActive(!Grid.activeSelf);
            GridArmorParts.SetActive(!GridArmorParts.activeSelf);
            HandEquipment.ForEach(i => i.color = Grid.activeSelf ? Color.white : new Color(1, 1, 1, 0.25f));
            InitializeDropdowns();
        }

        private void InitializeDropdowns()
        {
            if (Grid.activeSelf)
            {
                InitializeDropdown(HeadDropdown, SpriteCollection.Head, Dummy.Head, entry => Dummy.Head = GetSprite(entry));
                InitializeDropdown(EarsDropdown, SpriteCollection.Ears, Dummy.Ears, entry => Dummy.Ears = GetSprite(entry));
                InitializeDropdown(HairDropdown, SpriteCollection.Hair, Dummy.Hair, entry => Dummy.Hair = GetSprite(entry));
                InitializeDropdown(EyebrowsDropdown, SpriteCollection.Eyebrows, Dummy.Eyebrows, entry => Dummy.Eyebrows = GetSprite(entry));
                InitializeDropdown(EyesDropdown, SpriteCollection.Eyes, Dummy.Eyes, entry => Dummy.Eyes = GetSprite(entry));
                InitializeDropdown(MouthDropdown, SpriteCollection.Mouth, Dummy.Mouth, entry => Dummy.Mouth = GetSprite(entry));
                InitializeDropdown(BeardDropdown, SpriteCollection.Beard, Dummy.Beard, entry => Dummy.Beard = GetSprite(entry));
                InitializeDropdown(BodyDropdown, SpriteCollection.BodyTorso, Dummy.BodyTorso, SetBody);
                InitializeDropdown(HelmetDropdown, SpriteCollection.Helmet, Dummy.Helmet, SetHelmet);
                InitializeDropdown(ArmorDropdown, SpriteCollection.ArmorTorso, Dummy.ArmorTorso, SetArmor);
                InitializeDropdown(BackDropdown, SpriteCollection.Back, Dummy.Back, entry => Dummy.Back = GetSprite(entry));
                InitializeDropdown(MeleeWeapon1HDropdown, SpriteCollection.MeleeWeapon1H, Dummy.PrimaryMeleeWeapon, SetMeleeWeapon1H);
                InitializeDropdown(MeleeWeaponPairedDropdown, SpriteCollection.MeleeWeapon1H, Dummy.SecondaryMeleeWeapon, SetMeleeWeaponPaired);
                InitializeDropdown(MeleeWeapon2HDropdown, SpriteCollection.MeleeWeapon2H, Dummy.PrimaryMeleeWeapon, SetMeleeWeapon2H);
                InitializeDropdown(ShieldDropdown, SpriteCollection.Shield, Dummy.Shield, SetShield);
                InitializeDropdown(BowDropdown, SpriteCollection.BowRiser, Dummy.BowRiser, SetBow);
            }

            if (GridArmorParts.activeSelf)
            {
                InitializeDropdown(UpperArmorDropdown, new[] { SpriteCollection.ArmorArmL, SpriteCollection.ArmorArmR, SpriteCollection.ArmorForearmL, SpriteCollection.ArmorForearmR, SpriteCollection.ArmorTorso }, new List<Sprite> { Dummy.ArmorArmL, Dummy.ArmorArmR, Dummy.ArmorForearmL, Dummy.ArmorForearmR, Dummy.ArmorTorso }, SetUpperArmor);
                InitializeDropdown(LowerArmorDropdown, new[] { SpriteCollection.ArmorPelvis, SpriteCollection.ArmorLeg }, new List<Sprite> { Dummy.ArmorPelvis, Dummy.ArmorLeg }, SetLowerArmor);
                InitializeDropdown(GlovesDropdown, new[] { SpriteCollection.ArmorHandL, SpriteCollection.ArmorHandR }, new List<Sprite> { Dummy.ArmorHandL, Dummy.ArmorHandR }, SetGloves);
                InitializeDropdown(BootsDropdown, SpriteCollection.ArmorShin, Dummy.ArmorShin, entry => Dummy.ArmorShin = GetSprite(entry));
                InitializeDropdown(ArmorArmLDropdown, SpriteCollection.ArmorArmL, Dummy.ArmorArmL, entry => Dummy.ArmorArmL = GetSprite(entry));
                InitializeDropdown(ArmorArmRDropdown, SpriteCollection.ArmorArmR, Dummy.ArmorArmR, entry => Dummy.ArmorArmR = GetSprite(entry));
                InitializeDropdown(ArmorForearmLDropdown, SpriteCollection.ArmorForearmL, Dummy.ArmorForearmL, entry => Dummy.ArmorForearmL = GetSprite(entry));
                InitializeDropdown(ArmorForearmRDropdown, SpriteCollection.ArmorForearmR, Dummy.ArmorForearmR, entry => Dummy.ArmorForearmR = GetSprite(entry));
                InitializeDropdown(ArmorHandLDropdown, SpriteCollection.ArmorHandL, Dummy.ArmorHandL, entry => Dummy.ArmorHandL = GetSprite(entry));
                InitializeDropdown(ArmorHandRDropdown, SpriteCollection.ArmorHandR, Dummy.ArmorHandR, entry => Dummy.ArmorHandR = GetSprite(entry));
                InitializeDropdown(ArmorLegDropdown, SpriteCollection.ArmorLeg, Dummy.ArmorLeg, entry => Dummy.ArmorLeg = GetSprite(entry));
                InitializeDropdown(ArmorPelvisDropdown, SpriteCollection.ArmorPelvis, Dummy.ArmorPelvis, entry => Dummy.ArmorPelvis = GetSprite(entry));
                InitializeDropdown(ArmorShinDropdown, SpriteCollection.ArmorShin, Dummy.ArmorShin, entry => Dummy.ArmorShin = GetSprite(entry));
                InitializeDropdown(ArmorTorsoDropdown, SpriteCollection.ArmorTorso, Dummy.ArmorTorso, entry => Dummy.ArmorTorso = GetSprite(entry));
            }
        }

        private void SetBody(SpriteGroupEntry entry)
        {
            Dummy.SetBody(FindSprite(SpriteCollection.BodyArmL, entry),
                FindSprite(SpriteCollection.BodyArmR, entry),
                FindSprite(SpriteCollection.BodyForearmL, entry),
                FindSprite(SpriteCollection.BodyForearmR, entry),
                FindSprite(SpriteCollection.BodyHandL, entry),
                FindSprite(SpriteCollection.BodyHandR, entry),
                FindSprite(SpriteCollection.BodyLeg, entry),
                FindSprite(SpriteCollection.BodyPelvis, entry),
                FindSprite(SpriteCollection.BodyShin, entry),
                FindSprite(SpriteCollection.BodyTorso, entry));
        }

        private void SetHelmet(SpriteGroupEntry entry)
        {
            Dummy.Helmet = GetSprite(entry);
            Dummy.HelmetMask = FindSprite(SpriteCollection.HelmetMask, entry);
        }

        private void SetArmor(SpriteGroupEntry entry)
        {
            Dummy.SetArmor(FindSprite(SpriteCollection.ArmorArmL, entry),
                FindSprite(SpriteCollection.ArmorArmR, entry),
                FindSprite(SpriteCollection.ArmorForearmL, entry),
                FindSprite(SpriteCollection.ArmorForearmR, entry),
                FindSprite(SpriteCollection.ArmorHandL, entry),
                FindSprite(SpriteCollection.ArmorHandR, entry),
                FindSprite(SpriteCollection.ArmorLeg, entry),
                FindSprite(SpriteCollection.ArmorPelvis, entry),
                FindSprite(SpriteCollection.ArmorShin, entry),
                FindSprite(SpriteCollection.ArmorTorso, entry));
        }

        private void SetUpperArmor(SpriteGroupEntry entry)
        {
            Dummy.SetUpperArmor(FindSprite(SpriteCollection.ArmorArmL, entry),
                FindSprite(SpriteCollection.ArmorArmR, entry),
                FindSprite(SpriteCollection.ArmorForearmL, entry),
                FindSprite(SpriteCollection.ArmorForearmR, entry),
                FindSprite(SpriteCollection.ArmorTorso, entry));
        }

        private void SetLowerArmor(SpriteGroupEntry entry)
        {
            Dummy.SetLowerArmor(FindSprite(SpriteCollection.ArmorLeg, entry), FindSprite(SpriteCollection.ArmorPelvis, entry));
        }

        private void SetGloves(SpriteGroupEntry entry)
        {
            Dummy.SetGloves(FindSprite(SpriteCollection.ArmorHandL, entry), FindSprite(SpriteCollection.ArmorHandR, entry));
        }

        private void SetMeleeWeapon1H(SpriteGroupEntry entry)
        {
            MeleeWeapon2HDropdown.value = 0;
            Dummy.PrimaryMeleeWeapon = GetSprite(entry);
            Dummy.WeaponType = Dummy.WeaponType == WeaponType.MeleeTween ? WeaponType.MeleeTween : WeaponType.Melee1H;
            Dummy.BuildWeaponTrails();
            AnimationManager.Reset();
        }

        private void SetMeleeWeaponPaired(SpriteGroupEntry entry)
        {
            MeleeWeapon2HDropdown.value = ShieldDropdown.value = BowDropdown.value = 0;
            Dummy.SecondaryMeleeWeapon = GetSprite(entry);
            Dummy.WeaponType = WeaponType.MeleeTween;
            Dummy.BuildWeaponTrails();
            AnimationManager.Reset();
        }

        private void SetMeleeWeapon2H(SpriteGroupEntry entry)
        {
            MeleeWeapon1HDropdown.value = MeleeWeaponPairedDropdown.value = ShieldDropdown.value = BowDropdown.value = 0;
            Dummy.PrimaryMeleeWeapon = GetSprite(entry);
            Dummy.WeaponType = WeaponType.Melee2H;
            Dummy.BuildWeaponTrails();
            AnimationManager.Reset();
        }

        private void SetShield(SpriteGroupEntry entry)
        {
            MeleeWeapon2HDropdown.value = MeleeWeaponPairedDropdown.value = BowDropdown.value = 0;
            Dummy.Shield = GetSprite(entry);
            Dummy.WeaponType = WeaponType.Melee1H;
            AnimationManager.Reset();
        }

        private void SetBow(SpriteGroupEntry entry)
        {
            MeleeWeapon1HDropdown.value = MeleeWeapon2HDropdown.value = MeleeWeaponPairedDropdown.value = ShieldDropdown.value = 0;
            Dummy.SetBow(FindSprite(SpriteCollection.BowArrow, entry), FindSprite(SpriteCollection.BowLimb, entry), FindSprite(SpriteCollection.BowRiser, entry));
            Dummy.WeaponType = WeaponType.Bow;
            AnimationManager.Reset();
        }

        private static Sprite FindSprite(IEnumerable<SpriteGroupEntry> collection, SpriteGroupEntry entry)
        {
            return entry == null ? null : GetSprite(collection.SingleOrDefault(i => i.Collection == entry.Collection && i.Name == entry.Name));
        }

        private static Sprite GetSprite(SpriteGroupEntry entry)
        {
            return entry == null ? null : entry.Sprite;
        }

        private void InitializeDropdown(Dropdown dropdown, List<SpriteGroupEntry> entries, Sprite equipped, Action<SpriteGroupEntry> onEntrySelected)
        {
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.options = new List<Dropdown.OptionData> { new Dropdown.OptionData("Empty") };
            dropdown.value = 0;

            var list = new List<SpriteGroupEntry> { null };
            var ordered = entries.OrderBy(i => i.Collection).ThenBy(i => i.Name).ToList();

            foreach (var group in ordered.GroupBy(i => i.Collection))
            {
                dropdown.options.Add(new Dropdown.OptionData(string.Format("<b>{0}</b>", group.Key)));
                list.Add(null);

                foreach (var entry in group)
                {
                    dropdown.options.Add(new Dropdown.OptionData(GetDisplayName(entry.Name)));
                    list.Add(entry);

                    if (entry.Sprite == equipped)
                    {
                        dropdown.value = dropdown.options.Count - 1;
                    }
                }
            }

            UnityAction<int> onValueChanged = index => { onEntrySelected(list[index]); Dummy.Initialize(); };

            dropdown.RefreshShownValue();
            dropdown.onValueChanged.AddListener(onValueChanged);
            
            var onPointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            var onPointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };

            onPointerUp.callback.AddListener(eventData => StartCoroutine(OnDropdownExpanded(dropdown, list, onValueChanged)));
            onPointerExit.callback.AddListener(eventData => { if (dropdown.GetComponentInChildren<ScrollRect>() != null) onValueChanged(dropdown.value); });
            dropdown.GetComponent<EventTrigger>().triggers = new List<EventTrigger.Entry> { onPointerUp, onPointerExit };
        }

        private void InitializeDropdown(Dropdown dropdown, List<SpriteGroupEntry>[] groups, List<Sprite> equipped, Action<SpriteGroupEntry> onEntrySelected)
        {
            var entries = new List<SpriteGroupEntry>();

            foreach (var group in groups)
            {
                foreach (var entry in group)
                {
                    if (entries.Any(i => i.Collection == entry.Collection && i.Name == entry.Name)) continue;

                    entries.Add(entry);
                }
            }

            InitializeDropdown(dropdown, entries, equipped.FirstOrDefault(i => i != null), onEntrySelected);
        }

        private IEnumerator OnDropdownExpanded(Dropdown dropdown, List<SpriteGroupEntry> list, UnityAction<int> callback)
        {
            yield return null;

            var toggles = dropdown.GetComponentsInChildren<Toggle>().ToList();

            for (var i = 0; i < toggles.Count; i++)
            {
                if (i > 0 && list[i] == null)
                {
                    toggles[i].interactable = false;
                    toggles[i].colors = new ColorBlock { disabledColor = new Color(0, 0, 0, 20 / 255f), colorMultiplier = 1 };
                }
                else
                {
                    var index = i;
                    var pointerEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
                    var pointerClick = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
                    var scroll = new EventTrigger.Entry { eventID = EventTriggerType.Scroll };
                    var scrollRect = dropdown.GetComponentInChildren<ScrollRect>();

                    pointerEnter.callback.AddListener(eventData => callback(index));
                    pointerClick.callback.AddListener(eventData => toggles.ForEach(j => Destroy(j.GetComponent<EventTrigger>())));
                    scroll.callback.AddListener(eventData => scrollRect.OnScroll(eventData as PointerEventData));
                    toggles[i].gameObject.AddComponent<EventTrigger>().triggers = new List<EventTrigger.Entry> { pointerEnter, pointerClick, scroll };
                }
            }

            dropdown.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1 - (float) dropdown.value / (dropdown.options.Count - 1);
        }
      
        private static string GetDisplayName(string name)
        {
            if (name.All(c => char.IsUpper(c))) return name;

            return Regex.Replace(Regex.Replace(name, "[A-Z]", " $0"), "([a-z])([1-9])", "$1 $2").Trim();
        }
    }
}