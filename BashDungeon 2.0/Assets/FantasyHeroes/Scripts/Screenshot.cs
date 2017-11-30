using System;
using UnityEngine;

namespace Assets.FantasyHeroes.Scripts
{
    /// <summary>
    /// Take a screnshoot in play mode [S]
    /// </summary>
    public class Screenshot : MonoBehaviour
    {
        #if UNITY_EDITOR

        public int SuperSize = 1;
        public string Directory = "Screenshots";

        public  string GetPath()
        {
            return string.Format("{0}/Screenshot_{1}.png", Directory, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                var fileName = GetPath();

                System.IO.Directory.CreateDirectory(Directory);
                ScreenCapture.CaptureScreenshot(fileName, SuperSize);
                Debug.Log(string.Format("Screenshot saved: {0}", fileName));
            }
        }

        #endif
    }
}