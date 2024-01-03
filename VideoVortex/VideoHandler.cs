using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace VideoVortex
{
    enum ImageOperation
    {
        Origin, Resize, Crop
    }
    class VideoHandler
    {
        public bool save_image = false;
        public bool video_stop = false;

        public void OpenVideo<T>(string input_video, Tuple<bool, string> output_video, PictureBox display_window, Slider slider, ImageOperation type, List<T> parameter)
        {
            VideoWriter writer = null;
            using (VideoCapture capture = new VideoCapture(input_video))
            {
                if (!capture.IsOpened())
                {
                    Console.WriteLine("Unable to open the video!");
                    return;
                }
                int frameCount = capture.FrameCount;
                double fps = capture.Fps;
                slider.Maximum = frameCount / fps;
                if (output_video.Item1)
                {
                    switch (type)
                    {
                        case ImageOperation.Crop:
                            {
                                dynamic crop_width = parameter[2];
                                dynamic crop_height = parameter[3];
                                writer = new VideoWriter(output_video.Item2, FourCC.XVID, fps, new OpenCvSharp.Size(crop_width, crop_height));
                                break;
                            }
                        case ImageOperation.Resize:
                            {
                                dynamic resize_width = parameter[0];
                                dynamic resize_height = parameter[1];
                                writer = new VideoWriter(output_video.Item2, FourCC.XVID, fps, new OpenCvSharp.Size(resize_width, resize_height));
                                break;
                            }
                        default:
                            {
                                int width = (int)capture.FrameWidth;
                                int height = (int)capture.FrameHeight;
                                writer = new VideoWriter(output_video.Item2, FourCC.XVID, fps, new OpenCvSharp.Size(width, height));
                                break;
                            }
                    }
                }
                using (Mat frame = new Mat())
                {
                    Mat result = null;
                    int frame_num = 0;
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
                                    result = new Mat(frame, cropRect);
                                    break;
                                }
                            case ImageOperation.Resize:
                                {
                                    dynamic resize_width = parameter[0];
                                    dynamic resize_height = parameter[1];
                                    Size targetSize = new Size(resize_width, resize_height);
                                    result = new Mat();
                                    Cv2.Resize(frame, result, targetSize);
                                    break;
                                }
                            default:
                                {
                                    result = frame;
                                    break;
                                }
                        }
                        JudgeVideoWrite(output_video, writer, result);
                        JudgeSaveImage(save_image, result);
                        if (video_stop)
                        {
                            video_stop = false;
                            break;
                        }
                        slider.Value = frame_num++ / fps;
                        display_window.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(result);
                        Cv2.WaitKey(30);
                    }
                }
                JudgeVideoWriteRelease(output_video, writer);

            }
        }

        private void JudgeVideoWrite(Tuple<bool, string> output_video, VideoWriter writer, Mat result)
        {
            if (output_video.Item1)
            {
                writer.Write(result);
            }
        }

        private void JudgeSaveImage(bool save_image_state, Mat result)
        {
            if (save_image_state)
            {
                Cv2.ImWrite(Path.Combine(@"E:\DIP Temp\Image Output", "VideoVertex" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bmp"), result);
                save_image = false;

            }
        }

        private void JudgeVideoWriteRelease(Tuple<bool, string> output_video, VideoWriter writer)
        {
            if (output_video.Item1)
            {
                writer.Release();
            }
        }



    }





}
