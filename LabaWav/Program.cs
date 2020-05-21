using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace WavFormatCSharp
{
    internal static class Metadata
    {
        
        private static byte[] GetRelevantBytesIntoNewArray(byte[] forwardsWavFileStreamByteArray, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex + 1;
            byte[] relevantBytesArray = new byte[length];
            Array.Copy(forwardsWavFileStreamByteArray, startIndex, relevantBytesArray, 0, length);
            return relevantBytesArray;
        }
    }
    internal static class Constants
    {
        internal const int BitsPerSampleStartIndex = 34;
        internal const int BitsPerSampleEndIndex = 35;

        internal const int Subchunk2sizeStartIndex = 40;
        internal const int Subchunk2sizeEndIndex = 43;

        internal const int StartIndexOfAudioDataChunk = 44;

        internal const int BitsPerByte = 8;
    }

    internal static class ChangeWaver
    {
        public static void Start()
        {
            string forwardsWavFilePath = @"90.wav";
            byte[] forwardsWavFileStreamByteArray = PopulateForwardsWavFileByteArray(forwardsWavFilePath);

             }

        

        private static byte[] PopulateForwardsWavFileByteArray(string forwardsWavFilePath)
        {
            byte[] forwardsWavFileStreamByteArray;
            using (FileStream forwardsWavFileStream = new FileStream(forwardsWavFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                forwardsWavFileStreamByteArray = new byte[forwardsWavFileStream.Length];
                forwardsWavFileStream.Read(forwardsWavFileStreamByteArray, 0, (int)forwardsWavFileStream.Length);
            }

            return forwardsWavFileStreamByteArray;//
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            ChangeWaver.Start();
           
            Console.ReadLine();
        
        }
    }
}