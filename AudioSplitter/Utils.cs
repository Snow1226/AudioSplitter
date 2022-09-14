using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AudioSplitter
{
    internal static class Utils
    {
        public static string GetFullPath(this Transform transform, bool withSceneName = true)
        {
            string path = transform.name;

            Transform parent = transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            if (withSceneName)
                path = "(" + transform.gameObject.scene.name + ")/" + path;

            return path;
        }
    }
}
