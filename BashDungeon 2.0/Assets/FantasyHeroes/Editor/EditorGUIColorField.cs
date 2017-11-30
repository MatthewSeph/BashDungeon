using Assets.FantasyHeroes.Scripts;
using UnityEditor;
using UnityEngine;

namespace Assets.FantasyHeroes.Editor
{
    public class EditorGUIColorField : EditorWindow
    {
        public static Color Color;

        private static EditorWindow _window;
        private static bool _initialized;
        private static CharacterEditor _characterEditor;
        private static Color _originalColor;
        private static bool _applyColor;

        public static void Open(Color originalColor)
        {
            _originalColor = Color = originalColor;
            _applyColor = false;
            _characterEditor = _characterEditor ?? FindObjectOfType<CharacterEditor>();
            _window = GetWindow(typeof(EditorGUIColorField), true, "Select color");
            _window.Show();
            _window.position = Rect.zero;
            _initialized = false;
        }

        public void Update()
        {
            if (!Application.isPlaying)
            {
                Close();
            }
        }

        public void OnDestroy()
        {
            if (Application.isPlaying && !_applyColor)
            {
                _characterEditor.PickColor(_originalColor);
            }
        }

        public void OnGUI()
        {
            if (!_initialized)
            {
                var mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

                _window.position = new Rect(mouse.x - 240 - 20, mouse.y, 240, 80);
                _initialized = true;
            }

            Color = EditorGUI.ColorField(new Rect(5, 5, position.width - 6, 15), "Color:", Color);

            _characterEditor.PickColor(Color);

            var buttonWidth = (_window.position.width - 6) / 2;

            if (GUI.Button(new Rect(3, 25, buttonWidth, 20), "Revert"))
            {
                Color = _originalColor;
            }

            if (GUI.Button(new Rect(3 + buttonWidth, 25, buttonWidth, 20), "Apply"))
            {
                _applyColor = true;
                Close();
            }
        }
    }
}