using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNamesGenerator : MonoBehaviour
{


    public List<string> listaNomi = new List<string>();

    public string GenerateName()
    {

        string nomeScelto = "";
        int tempRandom;

        tempRandom = (int)Random.Range(0, adjectives.Length);

        nomeScelto += adjectives[tempRandom];

        tempRandom = (int)Random.Range(0, nouns.Length);

        nomeScelto += nouns[tempRandom];

        if (listaNomi.Contains(nomeScelto))
        {
            return GenerateName();
        }

        listaNomi.Add(nomeScelto);

        return nomeScelto;
    }

    public string[] adjectives =
        {
            "aged", "ancient", "autumn", "billowing", "bitter", "black", "blue", "bold",
            "broad", "broken", "calm", "cold", "cool", "crimson", "curly", "damp",
            "dark", "dawn", "delicate", "divine", "dry", "empty", "falling", "fancy",
            "flat", "floral", "fragrant", "frosty", "gentle", "green", "hidden", "holy",
            "icy", "jolly", "late", "lingering", "little", "lively", "long", "lucky",
            "misty", "muddy", "mute", "nameless", "noisy", "odd", "old",
            "orange", "patient", "plain", "polished", "proud", "purple", "quiet", "rapid",
            "raspy", "red", "restless", "rough", "round", "royal", "shiny", "shrill",
            "shy", "silent", "small", "snowy", "soft", "solitary", "sparkling", "spring",
            "square", "steep", "still", "summer", "super", "sweet", "throbbing", "tight",
            "tiny", "twilight", "wandering", "weathered", "white", "wild", "winter", "wispy",
            "withered", "yellow", "young"
        };

    public string[] nouns =
    {
            "Art",     "Band",   "Bar",       "Base",      "Bird",      "Block", "Boat", "Bonus",
            "Bread",   "Breeze", "Brook",     "Bush",      "Butterfly", "Cake", "Cell", "Cherry",
            "Cloud",   "Credit", "Darkness",  "Dew",       "Disk", "Dream", "Dust",
            "Feather", "Field",  "Fire",      "Firefly",   "Flower",    "Fog", "Forest", "Frog",
            "Frost",   "Glade",  "Glitter",   "Grass",     "Hall",      "Hat", "Haze", "Heart",
            "Hill",    "King",   "Lab",       "Lake",      "Leaf",      "Limit", "Math", "Meadow",
            "Mode",    "Moon",   "Morning",   "Mountain",  "Mouse",     "Mud", "Night", "Paper",
            "Pine",    "Poetry", "Pond",      "Queen",     "Rain",      "Recipe", "Resonance", "Rice",
            "River",   "Salad",  "Scene",     "Sea",       "Shadow", "Shape", "Silence", "Sky",
            "Smoke",   "Snow",   "Snowflake", "Sound",     "Star", "Sun", "Sunset",
            "Surf",    "Term",   "Thunder",   "Tooth",     "Tree", "Truth", "Union", "Unit",
            "Violet",  "Voice",  "Water",     "Waterfall", "Wave", "Wildflower", "Wind",
            "Wood"
        };
}

