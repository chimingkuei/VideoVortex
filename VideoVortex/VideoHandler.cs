using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoVortex
{
    enum ImageOperation
    {
        Resize, Convert_format, Crop
    }
    class VideoHandler
    {
        public bool save_image = false;
        public bool video_stop = false;

        public void OpenVideo<T>(string input_video, Tuple<bool, string> output_video, PictureBox display_window, ImageOperation type, List<T> parameter)
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
                    if (type == ImageOperation.Crop)
                    {
                        dynamic crop_width = parameter[2];
                        dynamic crop_height = parameter[3];
                        writer = new VideoWriter(output_video.Item2, FourCC.XVID, fps, new OpenCvSharp.Size(crop_width, crop_height));
                    }
                    else
                    {
                        int width = (int)capture.FrameWidth;
                        int height = (int)capture.FrameHeight;
                        writer = new VideoWriter(output_video.Item2, FourCC.XVID, fps, new OpenCvSharp.Size(width, height));
                    }
                    
                }
                using (Mat frame = new Mat())
                {
                    Mat Result = null;
                    while (capture.Read(frame))
                    {
                        switch (type)
                        {
                            case ImageOperation.Crop:
                                {
                                    dynamic crop_x = parameter[0];
                                    dynamic crop_y = parameter[1];
                                    dynamic crop_width = parameter[2];
                                    dynamic crop_height = parameter[3];
                                    OpenCvSharp.Rect cropRect = new OpenCvSharp.Rect(crop_x, crop_y, crop_width, crop_height);
                                    Result = new Mat(frame, cropRect);
                                    break;
                                }

                        }
                        if (output_video.Item1)
                        {
                            writer.Write(Result);
                        }
                        if (save_image)
                        {
                            Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmss"));
                            Cv2.ImWrite(Path.Combine(@"E:\DIP Temp\Image Output", "VideoVertex" + DateTime.Now.ToString("yyyyMMddHHmmss") +".bmp"), Result);
                            save_image = false;

                        }
                        if (video_stop)
                        {
                            break;
                        }
                        display_window.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(Result);
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
