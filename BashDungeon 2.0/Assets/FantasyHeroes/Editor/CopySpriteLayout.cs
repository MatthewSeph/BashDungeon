using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.FantasyHeroes.Editor
{
    /// <summary>
    /// Copy rects and pivots for single and multiple sprites
    /// </summary>
    public class CopySpriteLayout : EditorWindow
    {
        public Texture2D Source;
        public Object Destination;

        [MenuItem("Window/Fantasy Heroes/Copy Sprite Layout")]
        public static void Init()
        {
            var window = GetWindow<CopySpriteLayout>(false, "Copy Sprite Layout");

            window.minSize = window.maxSize = new Vector2(400, 130);
            window.Show();
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("This tool can copy sprite sheet and pivot to another sprite.", new GUIStyle(EditorStyles.label) { normal = { textColor = Color.green } });
            Source = (Texture2D) EditorGUILayout.ObjectField(new GUIContent("Source (sprite):", "This must be multiple sprite."), Source, typeof(Texture2D), false);
            Destination = EditorGUILayout.ObjectField(new GUIContent("Destination (sprite or folder):", "This must be a sprite or a folder (including subfolders)."), Destination, typeof(Object), false);

            if (GUILayout.Button("Copy pivots and slices"))
            {
                if (Source == null)
                {
                    Debug.LogWarning("Source is null");
                }
                else if (Destination == null)
                {
                    Debug.LogWarning("Destination is null");
                }
                else if (Destination is Texture2D)
                {
                    CopyPivotsAndSlices(Source, (Texture2D) Destination);
                }
                else
                {
                    var path = AssetDatabase.GetAssetPath(Destination);
                    var textures = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories).Select(AssetDatabase.LoadAssetAtPath<Texture2D>).ToList();

                    foreach (var texture in textures)
                    {
                        CopyPivotsAndSlices(Source, texture);
                    }
                }
            }
        }

        private static void CopyPivotsAndSlices(Texture2D copyFrom, Texture2D copyTo)
        {
            var copyFromPath = AssetDatabase.GetAssetPath(copyFrom);
            var ti1 = (TextureImporter) AssetImporter.GetAtPath(copyFromPath);

            ti1.isReadable = true;

            var copyToPath = AssetDatabase.GetAssetPath(copyTo);
            var ti2 = (TextureImporter) AssetImporter.GetAtPath(copyToPath);
            var ratio = copyFrom.width / copyTo.width;

            ti2.isReadable = false;
            ti2.spriteImportMode = ti1.spriteImportMode;


            if (ti1.spriteImportMode == SpriteImportMode.Single)
            {
                var texSettings = new TextureImporterSettings();

                ti1.ReadTextureSettings(texSettings);
                ti2.SetTextureSettings(texSettings);
            }
            else
            {
                Debug.Log("Fragments found: " + ti1.spritesheet.Length);

                var spritesheet = ti1.spritesheet.ToArray();

                for (var i = 0; i < spritesheet.Length; i++)
                {
                    var meta = spritesheet[i];

                    meta.rect.min /= ratio;
                    meta.rect.max /= ratio;

                    spritesheet[i] = meta;
                }

                ti2.spritesheet = spritesheet;
            }

            AssetDatabase.ImportAsset(copyToPath, ImportAssetOptions.ForceUpdate);
            Debug.LogFormat("Sliced: {0}", copyToPath);
        }
    }
}