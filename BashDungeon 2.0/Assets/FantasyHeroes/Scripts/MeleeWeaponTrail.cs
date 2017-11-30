using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FantasyHeroes.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MeleeWeaponTrail : MonoBehaviour
    {
        public SpriteRenderer WeaponRenderer;
        [Range(0, 1000)]
        public float TrailLength = 100;
        [Range(0, 1)]
        public float TrailBend = 0.25f;
        public AnimationCurve TrailCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
        [Tooltip("Warning: slows down the performance if checked.")]
        public bool RemoveSpaces;
        public bool Disabled;

        public void Build()
        {
            if (Disabled) return;

            var spriteRenderer = GetComponent<SpriteRenderer>();

            if (WeaponRenderer.sprite == null)
            {
                spriteRenderer.sprite = null;
            }
            else
            {
                var texture = CopyNotReadableSprite(WeaponRenderer.sprite);
                var trail = new Texture2D(texture.width, texture.height);

                ClearTexture(trail);

                var pixels = CreateTrailLine(texture, trail);
                var length = pixels.Last().y - pixels.First().y;

                FadeTrailLine(pixels, trail, length);

                if (TrailBend > 0 && RemoveSpaces)
                {
                    FillSpaces(trail);
                }

                trail.Apply();
                spriteRenderer.sprite = Sprite.Create(trail, new Rect(0, 0, trail.width, trail.height), new Vector2(0.5f, 0.5f), 100);
            }
        }

        private static Texture2D CopyNotReadableSprite(Sprite sprite)
        {
            var buffer = new Texture2D(sprite.texture.width, sprite.texture.height);
            var renderTexture = RenderTexture.GetTemporary(sprite.texture.width, sprite.texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

            Graphics.Blit(sprite.texture, renderTexture);
            RenderTexture.active = renderTexture;
            buffer.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            buffer.Apply();

            var texture = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height);
            var pixels = buffer.GetPixels((int) sprite.textureRect.x, (int) sprite.textureRect.y, (int) sprite.textureRect.width, (int) sprite.textureRect.height);

            ClearTexture(texture);
            texture.SetPixels((int) sprite.textureRectOffset.x, (int) sprite.textureRectOffset.y, (int) sprite.textureRect.width, (int) sprite.textureRect.height, pixels);
            texture.Apply();

            return texture;
        }

        private static List<Vector2> CreateTrailLine(Texture2D texture, Texture2D trail)
        {
            var line = new List<Vector2>();
            var color = Color.white;
            var pixels = texture.GetPixels();
            var width = texture.width;
            var height = texture.height;

            for (var y = 0; y < width; y++)
            {
                for (var x = 0; x < height; x++)
                {
                    if (pixels[x + y * width].a > 0.5f)
                    {
                        trail.SetPixel(x, y, color);
                        line.Add(new Vector2(x, y));
                        break;
                    }
                }
            }

            return line;
        }

        private void FadeTrailLine(IList<Vector2> pixels, Texture2D trail, float length)
        {
            foreach (var pixel in pixels)
            {
                var x = (int) pixel.x;
                var y = (int) pixel.y;
                var color = trail.GetPixel(x, y);
                var iterations = TrailLength * TrailCurve.Evaluate((y - pixels[0].y) / length);

                for (var i = 1; i < iterations; i++)
                {
                    if (x >= i)
                    {
                        color.a = 1 - i / iterations;
                        trail.SetPixel(x - i, (int) (y * Mathf.Cos(i * Mathf.Deg2Rad * TrailBend)), color);
                    }
                }
            }
        }

        private static void ClearTexture(Texture2D texture)
        {
            var pixels = new Color[texture.width * texture.height];
            var clear = Color.clear;

            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = clear;
            }

            texture.SetPixels(pixels);
            texture.Apply();
        }

        private static void FillSpaces(Texture2D texture)
        {
            var pixels = texture.GetPixels();
            var width = texture.width;
            var height = texture.height;

            for (var y = 1; y < width - 1; y++)
            {
                for (var x = 1; x < height - 1; x++)
                {
                    if (pixels[x + y * width].a <= 0)
                    {
                        var above = pixels[x + (y + 1) * width];
                        var below = pixels[x + (y - 1) * width];

                        if (above != Color.clear && below != Color.clear)
                        {
                            texture.SetPixel(x, y, (above + below) / 2);
                        }
                    }
                }
            }
        }
    }
}