using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace WavFormatCSharp
{
    internal static class Metadata
    {
        internal static ushort GetBitsPerSample(byte[] forwardsWavFileStreamByteArray)
        {
            byte[] bitsPerSampleByteArray = GetRelevantBytesIntoNewArray(forwardsWavFileStreamByteArray, Constants.BitsPerSampleStartIndex, Constants.BitsPerSampleEndIndex);
            ushort bitsPerSample = BitConverter.ToUInt16(bitsPerSampleByteArray, 0);
            byte[] SubchunksizeByteArray = GetRelevantBytesIntoNewArray(forwardsWavFileStreamByteArray, 4, 7);
            UInt32 Subchunksize = BitConverter.ToUInt32(SubchunksizeByteArray, 0);
            
            Console.WriteLine("Subchunk2size = {0}", Subchunksize);

            Console.WriteLine("Bits Per Sample = {0}", bitsPerSample);
            return bitsPerSample;
        }
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

            byte[] forwardsArrayWithOnlyHeaders = CreateForwardsArrayWithOnlyHeaders(forwardsWavFileStreamByteArray, Constants.StartIndexOfAudioDataChunk);
            byte[] forwardsArrayWithOnlyAudioData = CreateForwardsArrayWithOnlyAudioData(forwardsWavFileStreamByteArray, Constants.StartIndexOfAudioDataChunk);

            int bytesPerSample = Metadata.GetBitsPerSample(forwardsWavFileStreamByteArray) / Constants.BitsPerByte;
            byte[] ArrayWithOnlyAudioData = TheForwardsArrayWithOnlyAudioData(bytesPerSample, forwardsArrayWithOnlyAudioData); // изменять дата тут добавить число на которое изменить и там разобраться
            byte[] WavFileStreamByteArray = CombineArrays(forwardsArrayWithOnlyHeaders, ArrayWithOnlyAudioData);

            string WavFilePath = @"File.wav";

            WriteReversedWavFileByteArrayToFile(WavFileStreamByteArray, WavFilePath);
        }

        private static void WriteReversedWavFileByteArrayToFile(byte[] WavFileStreamByteArray, string WavFilePath)
        {
            using (FileStream FileStream = new FileStream(WavFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                FileStream.Write(WavFileStreamByteArray, 0, WavFileStreamByteArray.Length);
            }
        }

        private static byte[] CombineArrays(byte[] forwardsArrayWithOnlyHeaders, byte[] ArrayWithOnlyAudioData)
        {
            byte[] WavFileStreamByteArray = new byte[forwardsArrayWithOnlyHeaders.Length + ArrayWithOnlyAudioData.Length];
            Array.Copy(forwardsArrayWithOnlyHeaders, WavFileStreamByteArray, forwardsArrayWithOnlyHeaders.Length);
            Array.Copy(ArrayWithOnlyAudioData, 0, WavFileStreamByteArray, forwardsArrayWithOnlyHeaders.Length, ArrayWithOnlyAudioData.Length);
            return WavFileStreamByteArray;
        }

            private static byte[] TheForwardsArrayWithOnlyAudioData(int bytesPerSample, byte[] forwardsArrayWithOnlyAudioData)
        {
            int length = forwardsArrayWithOnlyAudioData.Length;
            byte[] ArrayWithOnlyAudioData = new byte[length*2];
            
            forwardsArrayWithOnlyAudioData.CopyTo(ArrayWithOnlyAudioData, 0);
            int x = forwardsArrayWithOnlyAudioData.Length-1;
            //forwardsArrayWithOnlyAudioData.CopyTo(ArrayWithOnlyAudioData, x);

            return ArrayWithOnlyAudioData;
        }

        private static byte[] CreateForwardsArrayWithOnlyAudioData(byte[] forwardsWavFileStreamByteArray, int startIndexOfDataChunk)
        {
            byte[] forwardsArrayWithOnlyAudioData = new byte[forwardsWavFileStreamByteArray.Length - startIndexOfDataChunk];
            Array.Copy(forwardsWavFileStreamByteArray, startIndexOfDataChunk, forwardsArrayWithOnlyAudioData, 0, forwardsWavFileStreamByteArray.Length - startIndexOfDataChunk);
            return forwardsArrayWithOnlyAudioData;
        }

        private static byte[] CreateForwardsArrayWithOnlyHeaders(byte[] forwardsWavFileStreamByteArray, int startIndexOfDataChunk)
        {
            byte[] forwardsArrayWithOnlyHeaders = new byte[startIndexOfDataChunk];
          
           
            Array.Copy(forwardsWavFileStreamByteArray, 0, forwardsArrayWithOnlyHeaders, 0, startIndexOfDataChunk);
           
            return forwardsArrayWithOnlyHeaders;
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