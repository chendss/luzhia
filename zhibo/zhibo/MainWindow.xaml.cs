using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System.IO;
namespace zhibo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ffemg ffs = new ffemg();
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 开始按钮事件：按下则灰色，执行录制函数。若列表是空则跳出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string recodequipmentText = RecodEquipment.Text.ToString();
            if (recodequipmentText == "")
            {
                MessageBox.Show("请选择一个设备！");
                return;
            }
            bool err;
            ffs.Start(recodequipmentText,@"D:\wss.m3u8",out err);
            if (err)
            {
                EndButton.IsEnabled = true;
                StartButton.IsEnabled = false;
            }
            

        }
        /// <summary>
        /// 结束按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            ffs.Stop();
            EndButton.IsEnabled = false;
            StartButton.IsEnabled = true;

        }


        /// <summary>
        /// 程序一载入执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecodEquipment_Initialized(object sender, EventArgs e)
        {
            /// <summary>
            /// 
            /// 获得音频设备名，并且赋值给下拉框
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            CaptureDevicesCollection captureCollection = new CaptureDevicesCollection();//获取录音设备
            string str = "";
            for (int i = 0; i < captureCollection.Count; i++)//获取每个设备名的值
            {
                if (i != 0)
                {
                    str = captureCollection[i].Description;
                    RecodEquipment.Items.Add(str);
                }
            }
        }
    }
}
