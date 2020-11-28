using Fuse.Tasks;
using UnityEngine;

namespace Fuse.Hotfix
{
    public static class Extension
    {
        public static CTaskHandle Run(this CTask task)
        {
            return GameEntry.CTask.Manager.Run(task);
        }

        public static Color ToColor(this string colorStr)
        {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString(colorStr, out color);
            return color;
        }
    }
}
