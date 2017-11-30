using System;
using UnityEngine;

namespace Assets.FantasyHeroes.Scripts
{
    [Serializable]
    public class SpriteGroupEntry
    {
        public string Name;
        public string Collection;
        public int Id;
        public Sprite Sprite;

        public SpriteGroupEntry(string name, string collection, Sprite sprite)
        {
            if (sprite == null)
            {
                Debug.LogWarningFormat("Sprite is null, please check import settings for: {0}/{1}", collection, name);
            }

            Name = name;
            Collection = collection;
            Id = sprite.GetHashCode();
            Sprite = sprite;
        }
    }
}