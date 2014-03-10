using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace wndr.me.SoundWaveser
{
    /// <summary>
    /// Encapsulate logic to generate a waves Bitmap files (currently only PNG is supported) that represents the given 
    /// WAV file (currently only 16 bit WAV is supported) file path.
    /// </summary>
    public class Waveser
    {
        /// <summary>
        /// Generate Waves in Bitmap representation from WAV URI
        /// </summary>
        /// <param name="wavURI">URL or local machine path</param>
        /// <param name="width">Bitmap width</param>
        /// <param name="height">Bitmap height</param>
        /// <param name="pen">Custom wave Bitmap colors</param>
        /// <returns></returns>
        public Bitmap GenerateWavesBitmap(string wavURI, int width, int height, Pen pen)
        {
            WaveFile wave = LoadWAV(wavURI);

            Bitmap bm = GenerateBitmap(width, height);
            Graphics grp = GenerateGraphic(bm);

            Paint(wave, width, height, grp, pen);

            return bm;
        }

        /// <summary>
        ///  Generate Waves in Bitmap representation from WAV URI
        /// </summary>
        /// <param name="wavURL">URL or local machine path</param>
        /// <param name="width">Bitmap width - Default 800</param>
        /// <param name="totalHeight">Bitmap height - Default 50</param>
        /// <returns></returns>
        public Bitmap GenerateWavesBitmap(string wavURL, int width = 800, int totalHeight = 50)
        {
            Pen pen = GenerateDefaultPen(totalHeight);
            return GenerateWavesBitmap(wavURL, width, totalHeight, pen);
        }

        #region Private Helpers
        /// <summary>
        /// Creates a waves represenation Bitmap of the WAV file
        /// </summary>
        /// <param name="Wavefile">Internal Wavefile object to 'Paint'</param>
        /// <param name="width">Bitmap Width</param>
        /// <param name="height">Bitmap Height</param>
        /// <param name="grfx">Graphic object</param>
        /// <param name="pen">Pen object</param>
        private void Paint(WaveFile Wavefile, int width, int height, Graphics grfx, Pen pen)
        {
            int prevX = 0;
            int prevY = 0;
            int index = 0;
            int jumper = 0;

            int samplesPerPixel = Wavefile.Data.NumSamples / width;
            int maxSample = (int)samplesPerPixel * width;

            maxSample = Math.Min(maxSample, Wavefile.Data.NumSamples);

            while (jumper < maxSample)
            {
                short maxVal = -Int16.MaxValue;
                short minVal = Int16.MaxValue;

                //* finds the max & min spots for each pixel 
                for (int x = 0; x < samplesPerPixel; x++)
                {
                    maxVal = Math.Max(maxVal, Wavefile.Data[x + jumper]);
                    minVal = Math.Min(minVal, Wavefile.Data[x + jumper]);
                }

                //* next jumper calculation
                jumper = (int)(index * samplesPerPixel);

                //* Adjust Waves height based on max Bitmap height
                int scaledMinVal = (int)(((minVal + Int16.MaxValue) * height) / UInt16.MaxValue);
                int scaledMaxVal = (int)(((maxVal + Int16.MaxValue) * height) / UInt16.MaxValue);

                //* In case max & min are equal, draw a line from the previous position
                if (scaledMinVal == scaledMaxVal)
                {
                    if (prevY != 0)
                        grfx.DrawLine(pen, prevX, prevY, index, scaledMaxVal);
                }
                else
                {
                    grfx.DrawLine(pen, index, scaledMinVal, index, scaledMaxVal);
                }

                //* Save points in case needed in next iteration
                prevX = index;
                prevY = scaledMaxVal;

                index++;
            }
        }

        /// <summary>
        /// Default System.Drawing.Pen. Used when not provied explicitly from client 
        /// </summary>
        /// <param name="height">Bitmap height</param>
        /// <returns></returns>
        private Pen GenerateDefaultPen(int height)
        {
            Color gradiantFromColor = Color.FromArgb(141, 141, 141);
            Color gradiantToColor = Color.FromArgb(65, 65, 65);
            Brush brush = new LinearGradientBrush(new Point(0, height / 2), new Point(0, height), gradiantFromColor, gradiantToColor);
            Pen pen = new Pen(brush);
            return pen;
        }

        /// <summary>
        /// Generate Graphic object
        /// </summary>
        /// <param name="bm">Bitmap object</param>
        /// <returns></returns>
        private static Graphics GenerateGraphic(Bitmap bm)
        {
            Graphics grp = Graphics.FromImage(bm);
            grp.Clear(Color.Transparent);

            /* NOTE: 
             * Still don't know why but after compering generated image with 'Audacity' generated image 
             * I have noticed that the Graphic must by inverted in order to represent the audio correctly. */
            grp.TranslateTransform(0, bm.Height);
            grp.ScaleTransform(1, -1);

            return grp;
        }

        /// <summary>
        /// Generate Bitmap object
        /// </summary>
        /// <param name="width">Bitmap Width</param>
        /// <param name="height">Bitmap Height</param>
        /// <returns>Bitmap object</returns>
        private Bitmap GenerateBitmap(int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            Graphics grp = Graphics.FromImage(bm);
            grp.Clear(Color.Transparent);
            return bm;
        }

        /// <summary>
        /// Load Wave file remotely or locally depending on the given URI
        /// </summary>
        /// <param name="wavURI">URI of WAVE file</param>
        /// <returns>WaveFile Object</returns>
        private WaveFile LoadWAV(string wavURI)
        {
            WaveFile wave;
            if (wavURI.ToLower().Contains("http"))
            {
                WebClient client = new WebClient();
                byte[] rawData = client.DownloadData(wavURI);
                wave = new WaveFile(new MemoryStream(rawData));
            }
            else
            {
                FileInfo file = new FileInfo(wavURI);
                Stream strm = file.OpenRead();
                wave = new WaveFile(strm);
            }

            wave.Read();
            return wave;
        } 
        #endregion

    }
}
