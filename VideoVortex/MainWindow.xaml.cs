using Newtonsoft.Json.Linq;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static VideoVortex.BaseLogRecord;

namespace VideoVortex
{
    public class Parameter
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }
    
    public partial class MainWindow : System.Windows.Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Function
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("請問是否要關閉？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region Parameter and Init
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Display_Window.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        }
        #region Log
        BaseLogRecord Logger = new BaseLogRecord();
        //Logger.WriteLog("儲存參數!", LogLevel.General, richTextBoxGeneral);
        #endregion
        #region Config
        BaseConfig<Parameter> Config = new BaseConfig<Parameter>();
        //Load Config
        //List<Parameter> Parameter_info = Config.Load();
        //Console.WriteLine(Parameter_info[0].ID);
        //Save Config
        //List<Parameter> Parameter_config = new List<Parameter>()
        //{
        //    new Parameter() { ID = 1, Name = "張飛"}
        //};
        //Config.Save(Parameter_config);
        #endregion
        VideoHandler VH = new VideoHandler();
        #endregion

        #region Main Screen
        private void Main_Btn_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case nameof(Start):
                    {
                        Tuple<bool, string> output_video = new Tuple<bool, string>(true, @"E:\DIP Temp\Image Output\1.mp4");
                        List<int> cropRect = new List<int>();
                        cropRect.Add(100);
                        cropRect.Add(50);
                        cropRect.Add(2000);
                        cropRect.Add(2000);
                        VH.OpenVideo<int>(@"E:\DIP Temp\Image Temp\1st Lens(Target).mp4", output_video, Display_Window, ImageOperation.Crop, cropRect);
                        break;
                    }
                case nameof(Save_Image):
                    {
                        VH.save_image = true;
                        break;
                    }
                case nameof(Stop):
                    {
                        VH.video_stop = true;
                        Display_Window.Image = null;
                        break;
                    }
            }
        }
        #endregion





    }
}
