using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoVortex
{
    class VideoHandler
    {
        public bool save_image = false;
        public bool video_stop = false;

        public void OpenVideo(string input_video, Tuple<bool, string> output_video, Rect cropRect, PictureBox Display_Window)
        {
            VideoWriter writer = null;
            using (VideoCapture capture = new VideoCapture(input_video))
            {
                if (!capture.IsOpened())
                {
                    Console.WriteLine("Unable to open the video!");
                    return;
                }
                if (output_video.Item1)
                {
                    double fps = capture.Fps;
                    //int width = (int)capture.FrameWidth;
                    //int height = (int)capture.FrameHeight;
                    writer = new VideoWriter(output_video.Item2, FourCC.XVID, fps, new OpenCvSharp.Size(cropRect.Width, cropRect.Height));
                }
                using (Mat frame = new Mat())
                {
                    while (capture.Read(frame))
                    {
                        Mat croppedFrame = new Mat(frame, cropRect);
                        if (output_video.Item1)
                        {
                            writer.Write(croppedFrame);
                        }
                        if (save_image)
                        {
                            Cv2.ImWrite(@"E:\DIP Temp\Image Output\test.bmp", croppedFrame);
                            save_image = false;

                        }
                        if (video_stop)
                        {
                            break;
                        }
                        Display_Window.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(croppedFrame);
                        Cv2.WaitKey(30);
                    }
                }
                if (output_video.Item1)
                {
                    writer.Release();
                }
                
            }
        }
    }





}
