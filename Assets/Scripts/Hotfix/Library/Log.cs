using System.Text;
using Runtime=UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class Log
    {
        private static bool isShowLog = true;
        public static void SetIsShowLog(bool isShow)
        {
            isShowLog = isShow;
            
        }

        /// <summary>
        /// 普通打印输出
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(params object[] msg)
        {
            if (isShowLog)
                Runtime.Log.Info(ObjectsToString(null, msg));
        }

        /// <summary>
        /// 输出警告信息
        /// </summary>
        public static void Warning(params object[] msg)
        {
            if (isShowLog)
                Runtime.Log.Warning(ObjectsToString(null, msg));
        }
        /// <summary>
        /// 输出错误信息
        /// </summary>
        public static void Error(params object[] msg)
        {
            Runtime.Log.Error(ObjectsToString(null, msg));
        }

        public static string ObjectsToString(string sTitle, params object[] logs)
        {
            StringBuilder sb = new StringBuilder();
            if (sTitle == null)
                sb.Append("<color=#FF00FF>[HotFix:]</color>");
            else
                sb.Append(sTitle);
            for (int i = 0; i < logs.Length; ++i)
            {
                if (logs[i] == null)
                    sb.Append("null");
                else
                    sb.Append(logs[i].ToString());
                if (i < logs.Length - 1)
                    sb.Append(", ");
            }
            return sb.ToString();
        }
    }
}