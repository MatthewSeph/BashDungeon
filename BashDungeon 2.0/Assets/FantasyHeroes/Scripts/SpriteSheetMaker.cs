using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Used for creating sprite sheets for frame-by-frame animation
    /// </summary>
    public class SpriteSheetMaker : MonoBehaviour
    {
        public Character Dummy;
        public GameObject Canvas;
        public ScreenshotTransparent ScreenshotTransparent;
        public string AnimationFolder;
        public List<string> ClipNames;
        public Dropdown AnimationDropdown;
        public Dropdown FrameSizeDropdown;
        public Dropdown FrameRatioDropdown;
        public Dropdown ScreenshotIntervalDropdown;
        public Dropdown WeaponTrailsDropdown;
        public Dropdown ShadowDropdown;
        
        #if UNITY_EDITOR

        /// <summary>
        /// Called only in Editor
        /// </summary>
        public void OnValidate()
        {
            ClipNames = System.IO.Directory.GetFiles(AnimationFolder, "*.anim", System.IO.SearchOption.AllDirectories).Select(System.IO.Path.GetFileNameWithoutExtension).ToList();
            AnimationDropdown.options = new List<Dropdown.OptionData> { new Dropdown.OptionData("All") };

            foreach (var clipName in ClipNames)
            {
                AnimationDropdown.options.Add(new Dropdown.OptionData(clipName));
            }
        }

        /// <summary>
        /// Called on start
        /// </summary>
        public void Start()
        {
            foreach (var dropdown in new[] { AnimationDropdown, FrameSizeDropdown, FrameRatioDropdown, ScreenshotIntervalDropdown, WeaponTrailsDropdown, ShadowDropdown })
            {
                dropdown.RefreshShownValue();
            }
        }

        /// <summary>
        /// Load character from prefab
        /// </summary>
        public void Load()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Open character prefab", "Assets/FantasyHeroes/Prefabs", "prefab");

            if (path.Length > 0)
            {
                path = "Assets" + path.Replace(Application.dataPath, null);
                Load(path);
            }
        }

        /// <summary>
        /// Load character from prefab by given path
        /// </summary>
        public void Load(string path)
        {
            var character = UnityEditor.AssetDatabase.LoadAssetAtPath<Character>(path);

            if (character == null) throw new Exception("Error loading character, please make sure you are loading correct prefab!");

            var instance = Instantiate(character);

            instance.name = Dummy.name;
            instance.transform.position = Dummy.transform.position;
            instance.transform.localScale = Dummy.transform.localScale;
            Destroy(Dummy.gameObject);
            Dummy = instance;
        }

        /// <summary>
        /// Create sprite sheet
        /// </summary>
        public void Capture()
        {
            var frameSize = new[] { 256, 512, 1024 }[FrameSizeDropdown.value];
            var frameRatio = FrameRatioDropdown.value + 4;
            var screenshotInterval = new[] { 0.1f, 0.25f, 0.5f, 1f }[ScreenshotIntervalDropdown.value];
            var weaponTrails = WeaponTrailsDropdown.value == 1;
            var shadow = ShadowDropdown.value == 1;

            foreach (var trail in Dummy.GetComponentsInChildren<MeleeWeaponTrail>(true))
            {
                trail.Disabled = !weaponTrails;

                if (weaponTrails)
                {
                    trail.RemoveSpaces = true;
                    trail.Build();
                }
            }

            Dummy.GetComponentsInChildren<SpriteRenderer>().Single(i => i.name == "Shadow").enabled = shadow;

            if (AnimationDropdown.value == 0)
            {
                StartCoroutine(CaptureFrames(ClipNames, frameSize, frameRatio, screenshotInterval));
            }
            else
            {
                var clipName = ClipNames[AnimationDropdown.value - 1];

                StartCoroutine(CaptureFrames(new List<string> { clipName }, frameSize, frameRatio, screenshotInterval));
            }
        }

        /// <summary>
        /// Navigate to URL
        /// </summary>
        public void Navigate(string url)
        {
            Application.OpenURL(url);
        }

        private void ShowFrame(string clipName, float normalizedTime)
        {
            Dummy.Animator.Play(clipName, 0, normalizedTime);
            Dummy.Animator.speed = 0;
        }

        private IEnumerator CaptureFrames(IEnumerable<string> clipNames, int frameSize, int frameRatio, float screenshotInterval)
        {
            Canvas.SetActive(false);

            foreach (var clipName in clipNames)
            {
                for (var i = 0; i < frameRatio; i++)
                {
                    ShowFrame(clipName, (float)i / (frameRatio - 1));

                    yield return new WaitForSeconds(screenshotInterval);

                    ScreenshotTransparent.Width = ScreenshotTransparent.Height = frameSize;
                    ScreenshotTransparent.Capture(string.Format("SpriteSheets/{0}/{1}.png", clipName, i));
                }
            }

            Canvas.SetActive(true);
        }

        #endif
    }
}