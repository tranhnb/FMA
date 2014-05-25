using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Utils
{

    /// <summary>
    /// Find subImage position by Color pixel
    /// </summary>
    public class ImageUtils
    {
        #region Variable and Properties
        
        private Process process = null;
        
        #endregion

        #region Constructor
        
        public ImageUtils()
        {
            Init();
        }

        #endregion Constructor

        /// <summary>
        /// Find a color is existed at an specific point
        /// </summary>
        /// <param name="img"></param>
        /// <param name="color"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public Point? FindAllPixelLocation(Bitmap img, Color color, Point startPoint, Point endPoint)
        {
            int c = color.ToArgb();
            for (int x = startPoint.X; x <= endPoint.X; x++)
            {
                for (int y = startPoint.Y; y < endPoint.Y; y++)
                {
                    if (c.Equals(img.GetPixel(x, y).ToArgb()))
                    {
                        return new Point(x, y);
                    }
                        
                }
            }
            return null;
        }

        /// <summary>
        /// Hash image file to byte array
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public byte[] Sha256HashFile(string file)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (Stream input = File.OpenRead(file))
                {
                    return sha256.ComputeHash(input);
                }
            }
        }

        /// <summary>
        /// Check an image is sub image.
        /// </summary>
        /// <param name="parentImagePath"></param>
        /// <param name="childImagePath"></param>
        /// <returns></returns>
        public bool IsSubImage(string parentImagePath, string childImagePath)
        {
            var isSub = true;
            var guid = Guid.NewGuid().ToString();
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var batchFilePath = root + guid + ".bat";


            string outputImagePath = Path.Combine(root, guid + ".png");
            string ioOutPutPath = Path.Combine(root, guid + ".txt");

            var cmd = string.Format("compare -metric RMSE -subimage-search \"{0}\" \"{1}\" \"{2}\" 2>\"{3}\"", parentImagePath, childImagePath, outputImagePath, ioOutPutPath);

            CreateBatchFile(cmd, batchFilePath);
            RunBatFile(batchFilePath);
            //Read output in output file
            string[] lines = File.ReadLines(ioOutPutPath).ToArray();

            if (lines.Length != 1)
            {
                isSub = false;
            }
            else
            {
                string pattern = "@ .*,.*";
                string result = lines[0];//7056.16 (0.10767) @ 38,13
                //Check result length
                if (string.IsNullOrEmpty(result) || result.Length > 100)
                {
                    isSub = false;
                }
                else
                {
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
                    MatchCollection collection = regex.Matches(result, 0);
                    if (collection.Count == 1 && collection[0].Success)
                    {
                        Match match = collection[0];
                        string[] coordinates = match.ToString().Replace("@", "").Trim().Split(new char[1]{','},StringSplitOptions.RemoveEmptyEntries);
                        int tempResult;
                        isSub = coordinates.Length == 2 && int.TryParse(coordinates[0], out tempResult) && int.TryParse(coordinates[1], out tempResult);
                    }
                    else
                    {
                        isSub = false;
                    }
                }
            }

            //Delete files
            DeleteFiles(new string[2] { 
                outputImagePath,
                ioOutPutPath
            });

            return isSub;
        }

        private void Init()
        {
            process= new Process();
            process.StartInfo.UseShellExecute = false;
            
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.LoadUserProfile = true;
           
        }

        private void RunBatFile(string batchFilePath)
        {
            process.StartInfo.FileName = batchFilePath;
            process.Start();
            process.WaitForExit();
        }
        private void DeleteFiles(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
            
        }

        private void CreateBatchFile(string input, string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine(input);
            writer.Close();
            fs.Close();
        }
    }



    
}

