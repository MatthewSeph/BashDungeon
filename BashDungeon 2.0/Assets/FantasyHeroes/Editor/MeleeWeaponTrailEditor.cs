using Assets.FantasyHeroes.Scripts;
using UnityEditor;
using UnityEngine;

namespace Assets.FantasyHeroes.Editor
{
    /// <summary>
    /// Add action buttons to LayerManager script
    /// </summary>
    [CustomEditor(typeof(MeleeWeaponTrail))]
    public class MeleeWeaponTrailEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (MeleeWeaponTrail)target;

            if (GUILayout.Button("Build"))
            {
                script.Build();
            }
        }
    }
}