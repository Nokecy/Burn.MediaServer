using System;
using Xabe.FFmpeg;

namespace Burn.RtmpPush
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = "http://223.110.243.173/ott.js.chinamobile.com/PLTV/3/224/3221227215/index.m3u8";

            FFmpeg.ExecutablesPath = @"C:\Program Files\ffmpeg\bin";

            var queue = new ConversionQueue(parallel: false);
            queue.Add(Conver(inputFile, "rtmp://127.0.0.1/wenli/live"));
            queue.Add(Conver(inputFile, "rtmp://127.0.0.1/wenli/live1"));
            queue.Start();
            queue.Add(Conver(inputFile, "rtmp://127.0.0.1/wenli/live2"));
            Console.ReadLine();
        }

        static IConversion Conver(string inputfile, string outputPath)
        {
            string arguments = $"-i \"{inputfile}\"  -vcodec copy -acodec aac -f flv \"{outputPath}\"";
            return new Conversion()
                                                        .AddParameter($"-i \"{inputfile}\"")
                                                        .AddParameter($"-vcodec copy -acodec aac -f flv")
                                                        .SetOutput(outputPath);
        }
    }
}
