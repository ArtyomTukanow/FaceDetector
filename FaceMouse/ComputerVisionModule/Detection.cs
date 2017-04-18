//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
#if !(__IOS__ || NETFX_CORE)
using Emgu.CV.Cuda;
#endif

namespace FaceMouse.ComputerVisionModule
{
   public partial class Vision
   {
      public static void FaceDetector(
         IInputArray image, String faceFileName, String eyeFileName, String noseFileName,
         List<Rectangle> faces, List<Rectangle> eyes, List<Rectangle> noses,
         out long detectionTime)
      {
         Stopwatch watch;

         using (InputArray iaImage = image.GetInputArray())
         {

#if !(__IOS__ || NETFX_CORE)
            if (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda)
            {
               using (CudaCascadeClassifier face = new CudaCascadeClassifier(faceFileName))
               using (CudaCascadeClassifier eye = new CudaCascadeClassifier(eyeFileName))
               using (CudaCascadeClassifier nose = new CudaCascadeClassifier(noseFileName))
               {
                  face.ScaleFactor = 1.1;
                  face.MinNeighbors = 10;
                  face.MinObjectSize = Size.Empty;
                  eye.ScaleFactor = 1.1;
                  eye.MinNeighbors = 10;
                  eye.MinObjectSize = Size.Empty;
                  nose.ScaleFactor = 1.1;
                  nose.MinNeighbors = 10;
                  nose.MinObjectSize = Size.Empty;
                  watch = Stopwatch.StartNew();
                  using (CudaImage<Bgr, Byte> gpuImage = new CudaImage<Bgr, byte>(image))
                  using (CudaImage<Gray, Byte> gpuGray = gpuImage.Convert<Gray, Byte>())
                  using (GpuMat region = new GpuMat())
                  {
                     face.DetectMultiScale(gpuGray, region);
                     Rectangle[] faceRegion = face.Convert(region);
                     faces.AddRange(faceRegion);
                     foreach (Rectangle f in faceRegion)
                     {
                        using (CudaImage<Gray, Byte> faceImg = gpuGray.GetSubRect(f))
                        {
                           //For some reason a clone is required.
                           //Might be a bug of CudaCascadeClassifier in opencv
                           using (CudaImage<Gray, Byte> clone = faceImg.Clone(null))
                           using (GpuMat eyeRegionMat = new GpuMat())
                           using (GpuMat noseRegionMat = new GpuMat())
                           {
                              eye.DetectMultiScale(clone, eyeRegionMat);
                              Rectangle[] eyeRegion = eye.Convert(eyeRegionMat);
                              foreach (Rectangle e in eyeRegion)
                              {
                                 Rectangle eyeRect = e;
                                 eyeRect.Offset(f.X, f.Y);
                                 eyes.Add(eyeRect);
                              }

                              nose.DetectMultiScale(clone, noseRegionMat);
                              Rectangle[] noseRegion = nose.Convert(noseRegionMat);
                              foreach (Rectangle e in noseRegion)
                              {
                                  Rectangle noseRect = e;
                                  noseRect.Offset(f.X, f.Y);
                                  noses.Add(noseRect);
                              }
                           }
                        }
                     }
                  }
                  watch.Stop();
               }
            }
            else
#endif
            {
               //Read the HaarCascade objects
               using (CascadeClassifier face = new CascadeClassifier(faceFileName))
               using (CascadeClassifier eye = new CascadeClassifier(eyeFileName))
               using (CascadeClassifier nose = new CascadeClassifier(noseFileName))
               {
                  watch = Stopwatch.StartNew();

                  using (UMat ugray = new UMat())
                  {
                     CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                     //normalizes brightness and increases contrast of the eyeImage
                     CvInvoke.EqualizeHist(ugray, ugray);

                     //FaceDetector the faces  from the gray scale eyeImage and store the locations as rectangle
                     //The first dimensional is the channel
                     //The second dimension is the index of the rectangle in the specific channel                     
                     Rectangle[] facesDetected = face.DetectMultiScale(
                        ugray,
                        1.1,
                        10,
                        new Size(20, 20));

                     faces.AddRange(facesDetected);

                     foreach (Rectangle f in facesDetected)
                     {
                        //Get the region of interest on the faces
                        using (UMat faceRegion = new UMat(ugray, f))
                        {
                           Rectangle[] eyesDetected = eye.DetectMultiScale(
                              faceRegion,
                              1.1,
                              10,
                              new Size(20, 20));

                           foreach (Rectangle e in eyesDetected)
                           {
                              Rectangle eyeRect = e;
                              eyeRect.Offset(f.X, f.Y);
                              eyes.Add(eyeRect);
                           }


                           Rectangle[] nosesDetected = nose.DetectMultiScale(
                              faceRegion,
                              1.1,
                              10,
                              new Size(20, 20));

                           foreach (Rectangle e in nosesDetected)
                           {
                               Rectangle noseRect = e;
                               noseRect.Offset(f.X, f.Y);
                               noses.Add(noseRect);
                           }
                        }
                     }
                  }
                  watch.Stop();
               }
            }
            detectionTime = watch.ElapsedMilliseconds;
         }
      }

       public static void EyeDetector2(
           IInputArray eyeImage,
           String eyeFileName,
           List<Rectangle> eyes,
           out long detectionTime)
       {
           Stopwatch watch;

           using (InputArray iaImage = eyeImage.GetInputArray())
           {

#if !(__IOS__ || NETFX_CORE)
               if (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda)
               {
                   using (CudaCascadeClassifier eye = new CudaCascadeClassifier(eyeFileName))
                   {
                       eye.ScaleFactor = 1.1;
                       eye.MinNeighbors = 10;
                       eye.MinObjectSize = Size.Empty;
                       watch = Stopwatch.StartNew();
                       using (CudaImage<Bgr, Byte> gpuImage = new CudaImage<Bgr, byte>(eyeImage))
                       using (CudaImage<Gray, Byte> gpuGray = gpuImage.Convert<Gray, Byte>())
                       using (GpuMat region = new GpuMat())
                       {
                           eye.DetectMultiScale(gpuGray, region);
                           Rectangle[] eyesDetected = eye.Convert(region);
                           eyes.AddRange(eyesDetected);
                       }
                       watch.Stop();
                   }
               }
               else
#endif
               {
                   using (CascadeClassifier eye = new CascadeClassifier(eyeFileName))
                   {
                       watch = Stopwatch.StartNew();

                       using (UMat ugray = new UMat())
                       {
                           CvInvoke.CvtColor(eyeImage, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                           CvInvoke.EqualizeHist(ugray, ugray);
                           Rectangle[] eyesDetected = eye.DetectMultiScale(
                               ugray,
                               1.1,
                               10,
                               new Size(20, 20));
                           eyes.AddRange(eyesDetected);
                       }
                       watch.Stop();
                   }
               }
               detectionTime = watch.ElapsedMilliseconds;
           }
       }



       public static void EyeDetector(
           IInputArray image, String eyeFileName,
           Rectangle eyeRectangle, List<Rectangle> eyes,
           out long detectionTime)
       {
           Stopwatch watch;

           using (InputArray iaImage = image.GetInputArray())
           {

#if !(__IOS__ || NETFX_CORE)
               if (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda)
               {
                   using (CudaCascadeClassifier eye = new CudaCascadeClassifier(eyeFileName))
                   {
                       eye.ScaleFactor = 1.1;
                       eye.MinNeighbors = 10;
                       eye.MinObjectSize = Size.Empty;
                       watch = Stopwatch.StartNew();
                       using (CudaImage<Bgr, Byte> gpuImage = new CudaImage<Bgr, byte>(image))
                       using (CudaImage<Gray, Byte> gpuGray = gpuImage.Convert<Gray, Byte>())
                       using (GpuMat region = new GpuMat())
                       {
                           using (CudaImage<Gray, Byte> faceImg = gpuGray.GetSubRect(eyeRectangle))
                           {
                               using (CudaImage<Gray, Byte> clone = faceImg.Clone(null))
                               using (GpuMat eyeRegionMat = new GpuMat())
                               {
                                   eye.DetectMultiScale(clone, eyeRegionMat);
                                   Rectangle[] eyeRegion = eye.Convert(eyeRegionMat);
                                   foreach (Rectangle e in eyeRegion)
                                   {
                                       Rectangle eyeRect = e;
                                       eyeRect.Offset(eyeRectangle.X, eyeRectangle.Y);
                                       eyes.Add(eyeRect);
                                   }
                               }
                           }
                       }
                       watch.Stop();
                   }
               }
               else
#endif
               {
                   using (CascadeClassifier eye = new CascadeClassifier(eyeFileName))
                   {
                       watch = Stopwatch.StartNew();

                       using (UMat ugray = new UMat())
                       {
                           CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                           CvInvoke.EqualizeHist(ugray, ugray);
                           using (UMat faceRegion = new UMat(ugray, eyeRectangle))
                           {
                               Rectangle[] eyesDetected = eye.DetectMultiScale(faceRegion);

                               foreach (Rectangle e in eyesDetected)
                               {
                                   Rectangle eyeRect = e;
                                   eyeRect.Offset(eyeRectangle.X, eyeRectangle.Y);
                                   eyes.Add(eyeRect);
                               }
                           }
                       }
                       watch.Stop();
                   }
               }
               detectionTime = watch.ElapsedMilliseconds;
           }
       }
   }
}
