using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;

namespace SBBArkiv
{
    /// <summary>
    /// Uses GZip to compress/decompress byte arrays
    /// </summary>
    public class Compressor
    {
        private string _tempDir;

        /// <summary>
        /// Creates a new compressor using <paramref name="tempDir"/> as temporary directory
        /// </summary>
        /// <param name="tempDir"></param>
        public Compressor(string tempDir)
        {
            _tempDir = tempDir;
        }

        /// <summary>
        /// Compresses the specified byte array using gzip
        /// </summary>
        /// <param name="input">The content to compress</param>
        /// <returns>The path to the compressed/decompressed file</returns>
        private string ProcessZip(byte[] input, CompressionMode mode)
        {
            string fileName = _tempDir + "\\" + Path.GetFileName(Path.GetTempFileName());

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (GZipStream gs = new GZipStream(fs, mode))
                {
                    gs.Write(input, 0, input.Length);
                    gs.Close();
                }

                fs.Close();
            }

            return fileName;
        }

        /// <summary>
        /// Compresses byte array <paramref name="input"/>
        /// </summary>
        /// <param name="input">The byte array to compress</param>
        /// <returns>The result of the compression</returns>
        public byte[] Compress(byte[] input)
        {
            string tempFile = ProcessZip(input, CompressionMode.Compress);
            byte[] result = File.ReadAllBytes(tempFile);
            File.Delete(tempFile);
            return result;
        }

        /// <summary>
        /// Decompresses byte array <paramref name="input"/>
        /// </summary>
        /// <param name="input">The byte array to decompress</param>
        /// <returns>The decompressed bytes</returns>
        public byte[] Decompress(byte[] input)
        {
            string tempFile = ProcessZip(input, CompressionMode.Decompress);
            byte[] result = File.ReadAllBytes(tempFile);
            File.Delete(tempFile);
            return result;
        }

        /// <summary>
        /// Decompresses the specified byte array and returns the file path
        /// </summary>
        /// <param name="input">The byte array to decompress</param>
        /// <returns>The full path and name of file</returns>
        public string DecompressToFile(byte[] input)
        {
            return ProcessZip(input, CompressionMode.Decompress);
        }


    }
}