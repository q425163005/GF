﻿
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Fuse.Editor
{
    public class ToolsHelper
    {
        public static bool ShowDialog(string msg, string title = "提示", string button = "确定")
        {
            return EditorUtility.DisplayDialog(title, msg, button);
        }

        public static void ShowDialogSelection(string msg, Action yesCallback)
        {
            if (EditorUtility.DisplayDialog("确定吗", msg, "是!", "不！"))
            {
                yesCallback();
            }
        }
        
        /// <summary>
        /// 打开资源管理器
        /// </summary>
        /// <param name="path"></param>
        public static void ShowExplorer(string path)
        {
            System.Diagnostics.Process.Start(path);
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteDir(string dir)
        {
            FileUtil.DeleteFileOrDirectory(dir);
        }

        /// <summary>
        /// 打开场景
        /// </summary>
        /// <param name="scene"></param>
        public static void OpenScene(string scene)
        {
#if UNITY_5 || UNITY_2017_1_OR_NEWER
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scene);
#else
            if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
                EditorApplication.OpenScene(mainScene);
#endif
        }
        /// <summary>
        /// 显示进度
        /// </summary>
        /// <param name="val"></param>
        /// <param name="total"></param>
        /// <param name="cur"></param>
        public static void ShowProgress(string title ,int total, int cur)
        {
            EditorUtility.DisplayProgressBar(title, string.Format("请稍等({0}/{1}) ", cur, total), cur/ (float)total);
            if(total==cur)
                EditorUtility.ClearProgressBar();
        }
        /// <summary>
        /// 清理控制台日志
        /// </summary>
        public static void ClearConsole()
        {
#if UNITY_EDITOR
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
            clearConsoleMethod.Invoke(new object(), null);
#endif
        }
        public static bool IsPlaying()
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("提示", "请先停止运行","知道了...");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行批处理命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="workingDirectory"></param>
        public static void ExecuteCommand(string command, string workingDirectory = null)
        {
            var fProgress = .1f;
            EditorUtility.DisplayProgressBar("ExecuteCommand", command, fProgress);

            try
            {
                string cmd;
                string preArg;
                var os = Environment.OSVersion;

                //Debug.Log(String.Format("[ExecuteCommand]Command on OS: {0}", os.ToString()));
                if (os.ToString().Contains("Windows"))
                {
                    cmd = "cmd.exe";
                    preArg = "/C ";
                }
                else
                {
                    cmd = "sh";
                    preArg = "-c ";
                }
                Debug.Log("[ExecuteCommand]" + command);
                var allOutput = new StringBuilder();
                using (var process = new Process())
                {
                    if (workingDirectory != null)
                        process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.FileName = cmd;
                    process.StartInfo.Arguments = preArg + "\"" + command + "\"";
                    //process.StartInfo.StandardOutputEncoding = Encoding.Default;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    Console.InputEncoding = Encoding.UTF8;
                    process.Start();
                    while (true)
                    {
                        var line = process.StandardOutput.ReadLine();
                        if (line == null)
                            break;
                        allOutput.AppendLine(line);
                        EditorUtility.DisplayProgressBar("[ExecuteCommand] " + command, line, fProgress);
                        fProgress += .001f;
                    }

                    var err = process.StandardError.ReadToEnd();
                    if (!String.IsNullOrEmpty(err))
                    {
                        Debug.LogError(String.Format("[ExecuteCommand] {0}", err));
                    }
                    process.WaitForExit();
                }
                if(allOutput.Length>0)
                    Debug.Log("[ExecuteResult]" + allOutput);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }


        /// <summary>
        /// 打开外部程序
        /// </summary>
        /// <param name="_exePathName">EXE所在绝对路径及名称带.exe</param>
        /// <param name="_exeArgus">启动参数</param>
        public static void OpenEXE(string filePath, string _exeArgus = null)
        {
            try
            {
                FileInfo file = new FileInfo(filePath);
                if (!file.Exists)
                {
                    Debug.LogError("文件不存在:"+ file.FullName);
                    return;
                }
                Process myprocess = new Process();
                //ProcessStartInfo startInfo = new ProcessStartInfo(file.FullName, _exeArgus);
                myprocess.StartInfo.FileName = file.FullName;
                myprocess.StartInfo.WorkingDirectory = file.DirectoryName;
                myprocess.StartInfo.Arguments = _exeArgus;
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.StartInfo.CreateNoWindow = true;
                myprocess.Start();
            }
            catch (Exception ex)
            {
                Debug.Log("出错原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="iscover">存在是否进行覆盖,默认true</param>
        public static void SaveFile(string path, string content, bool iscover = true, bool isLog = true)
        {
            FileInfo info = new FileInfo(path);            
            if (!iscover && info.Exists) //不覆盖
            {
                if (isLog)
                    Debug.LogWarning($"文件已存在，不进行覆盖操作!! {path}");
                return;
            }
            CheckCreateDirectory(info.DirectoryName);
            FileStream fs = new FileStream(path, FileMode.Create);            
            StreamWriter sWriter = new StreamWriter(fs, Encoding.GetEncoding("UTF-8"));
            sWriter.WriteLine(content);
            sWriter.Flush();
            sWriter.Close();
            fs.Close();
            Debug.Log($"成功生成文件 {path}");
        }
        /// <summary>
        /// 判断文件夹是否存在，不存在则创建一个
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void CheckCreateDirectory(string path)
        {
            if (!Directory.Exists(path))//如果不存在就创建文件夹
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 复制到剪切板
        /// </summary>
        /// <param name="str"></param>
        public static void CopyString(string str)
        {
            TextEditor te = new TextEditor();
            te.text = str;
            te.SelectAll();
            te.Copy();
        }
    }
}
