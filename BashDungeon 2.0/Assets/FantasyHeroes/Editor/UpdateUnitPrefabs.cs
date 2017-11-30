using System.IO;
using Assets.FantasyHeroes.Scripts;
using UnityEditor;
using UnityEngine;

namespace Assets.FantasyHeroes.Editor
{
    /// <summary>
    /// Split multiple sprite, move pivots to center and crop if needed
    /// </summary>
    public class UpdateUnitPrefabs : EditorWindow
    {
        public Object UnitPrefabsFolder;

        [MenuItem("Window/Fantasy Heroes/Update Unit Prefabs")]
        public static void Init()
        {
            var window = GetWindow<UpdateUnitPrefabs>(false, "Sprite Import Settings");

            window.minSize = window.maxSize = new Vector2(400, 140);
            window.Show();
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("This tool will update all character prefabs (just preforms load/save).", new GUIStyle(EditorStyles.label) { normal = { textColor = Color.green } });
            UnitPrefabsFolder = EditorGUILayout.ObjectField(new GUIContent("Unit prefabs (folder):", "This should be unit prefabss folder."), UnitPrefabsFolder, typeof(Object), false);

            if (GUILayout.Button("Update"))
            {
                var characterEditor = FindObjectOfType<CharacterEditor>();

                foreach (var path in Directory.GetFiles(AssetDatabase.GetAssetPath(UnitPrefabsFolder), "*.prefab", SearchOption.AllDirectories))
                {
                    var p = path.Replace("\\", "/");

                    characterEditor.Load(p);
                    characterEditor.Save(p);
                }
            }
        }
    }
}