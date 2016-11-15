using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.DirectX.DirectSound;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace zhibo
{
    class ffemg
    {
        #region 控制台api

        [DllImport("kernel32.dll")]
        static extern bool GenerateConsoleCtrlEvent(int dwCtrlEvent, int dwProcessGroupId);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(IntPtr handlerRoutine, bool add);

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();

        #endregion

        
        // 访问ffmpeg进程
        static Process p = new Process();

        // ffmpeg.exe路径
        static string ffmpegPath()
        {
            string ffmpegpath;
            string zbbinPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] temp = zbbinPath.Split("\\".ToCharArray());
            string upf = "";
            for (int i = 0; i < temp.Length - 3; i++)
            {
                upf += temp[i];
                upf += "\\";
            }
            ffmpegpath = upf + @"ffmpeg-20161012-7cf0ed3-win64-static\ffmpeg-20161012-7cf0ed3-win64-static\bin\ffmpeg.exe";
            return ffmpegpath;
        }
        int a;
        /// <summary>
        /// 功能: 录制
        /// </summary>
        public void Start(string audio,string outFilePath,out bool err)//输出路径
        {
            err = true;
            string ffmpegpath = ffmpegPath();
            if (File.Exists(outFilePath))
            {
                File.Delete(outFilePath);
            }
            ProcessStartInfo startInfo = new ProcessStartInfo(ffmpegpath);
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = "-f dshow -i audio=\"" + audio 
                                + "\"" + " -draw_mouse 1 -offset_x 0 -offset_y 0 -f GDIgrab -i desktop -vcodec libx264 -hls_time 4 -hls_list_size 0 " 
                                + outFilePath;//相当于在CMD输入的东西，ffmpeg参数
            p.StartInfo = startInfo;
            //隐藏cmd
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            //程序开始
            try
            {
                p.Start();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("找不到"))
                {
                    MessageBox.Show("屏幕录制库已丢失");
                    err = false;
                }
                
            }
        }

        /// <summary>
        /// 功能: 停止录制 模拟Ctrl+c
        /// </summary>
        public void Stop()
        {

            AttachConsole(p.Id);
            SetConsoleCtrlHandler(IntPtr.Zero, true);
            GenerateConsoleCtrlEvent(0, 0);
            FreeConsole();

            
        }

    }
}
