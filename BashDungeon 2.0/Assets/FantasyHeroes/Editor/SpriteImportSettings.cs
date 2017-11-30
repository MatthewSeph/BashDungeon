using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.FantasyHeroes.Editor
{
    /// <summary>
    /// Split multiple sprite, move pivots to center and crop if needed
    /// </summary>
    public class SpriteImportSettings : EditorWindow
    {
        public Object SpritesFolder;
        public bool ForceSingle = true;
        public bool SetPackingTag = true;
        public bool EnableReadWrite;
        public bool SetCompression = true;
        
        [MenuItem("Window/Fantasy Heroes/Sprite Import Settings")]
        public static void Init()
        {
            var window = GetWindow<SpriteImportSettings>(false, "Sprite Import Settings");

            window.minSize = window.maxSize = new Vector2(400, 140);
            window.Show();
        }

        public void OnEnable()
        {
            SpritesFolder = AssetDatabase.LoadAssetAtPath<Object>("Assets/FantasyHeroes/Sprites");
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("This tool is designed for handling sprite collection import settings.", new GUIStyle(EditorStyles.label) { normal = { textColor = Color.green } });
            SpritesFolder = EditorGUILayout.ObjectField(new GUIContent("Sprites (folder):", "This should be sprites root folder."), SpritesFolder, typeof(Object), false);
            ForceSingle = EditorGUILayout.Toggle(new GUIContent("Set sprite mode to Single:", "Check to override sprite mode to Single (this may break a layout)."), ForceSingle);
            SetPackingTag = EditorGUILayout.Toggle(new GUIContent("Set packing tag (auto):", ""), SetPackingTag);
            EnableReadWrite = EditorGUILayout.Toggle(new GUIContent("Enable read/write:", ""), EnableReadWrite);
            SetCompression = EditorGUILayout.Toggle(new GUIContent("Set compression:", "Uncheck for raw images."), SetCompression);

            if (GUILayout.Button("Setup"))
            {
                if (SpritesFolder == null)
                {
                    Debug.LogWarning("SpritesFolder is null");
                }
                else
                {
                    var root = AssetDatabase.GetAssetPath(SpritesFolder);
                    var files = Directory.GetFiles(root, "*.png", SearchOption.AllDirectories).ToList();

                    foreach (var file in files)
                    {
                        SetImportSettings(file, ForceSingle, SetPackingTag, EnableReadWrite, SetCompression);
                    }
                }
            }
        }

        public static void SetImportSettings(string path, bool forceSingle, bool resolvePackingTag, bool enableReadWrite, bool compressed)
        {
            path = path.Replace("\\", "/");

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            var targetImporter = (TextureImporter) AssetImporter.GetAtPath(path);

            targetImporter.spriteImportMode = forceSingle ? SpriteImportMode.Single : targetImporter.spriteImportMode;
            targetImporter.textureType = TextureImporterType.Sprite;
            targetImporter.spritePackingTag = resolvePackingTag ? ResolvePackingTag(path) : null;
            targetImporter.alphaIsTransparency = true;
            targetImporter.isReadable = enableReadWrite;
            targetImporter.mipmapEnabled = false;
            targetImporter.wrapMode = TextureWrapMode.Clamp;
            targetImporter.filterMode = FilterMode.Bilinear;
            targetImporter.maxTextureSize = targetImporter.spritePackingTag == "HelmetMask" ? 128 : GetMaxTextureSize(texture);
            targetImporter.textureCompression = compressed ? TextureImporterCompression.Compressed : TextureImporterCompression.Uncompressed;
            targetImporter.compressionQuality = 50;
            targetImporter.crunchedCompression = true;
            targetImporter.SaveAndReimport();

            Debug.LogFormat("Import settings set for: {0}", path);
        }

        private static string ResolvePackingTag(string path)
        {
            if (path.Contains("/BodyParts/")) return "BodyParts";
            if (path.Contains("/Helmet/")) return "Helmet";
            if (path.Contains("/HelmetMask/")) return "HelmetMask";
            if (path.Contains("/Armor/")) return "Armor";
            if (path.Contains("/Shield/")) return "Shield";
            if (path.Contains("/Back/")) return "Back";
            if (path.Contains("Weapon") || path.Contains("Bow")) return "Weapon";

            Debug.LogWarningFormat("Can't resolve packing tag: {0}", path);

            return "Unknown";
        }

        private static int GetMaxTextureSize(Texture2D texture)
        {
            var maxTextureSize = Math.Max(texture.width, texture.height);

            for (var i = 5; i <= 11; i++)
            {
                var size = (int) Math.Pow(2, i);

                if (size >= maxTextureSize) return size;
            }

            return 2048;
        }
    }
}