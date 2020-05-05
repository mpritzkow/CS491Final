using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace CS491Final
{
    public class Program
    {
        static void Main(string[] args)
        {
            IList<Traces> traceChannels;
            Header header;

            string filePath = "C:\\Users\\Matt Pritzkow\\OneDrive - Optim\\Desktop\\sg2 refraction data\\00000062_Hit_1_of_10.sg2";
            header = new Header();
            traceChannels = new List<Traces>();

            Byte[] buffer = File.ReadAllBytes(filePath);

            Debug.WriteLine("Length of buffer: " + buffer.Length);

            header.readHeader(buffer, filePath);
            for (int i = 0; i < header.numOfTraces; i++)
            {
                Traces temp = new Traces();
                temp.storeEntireTraceHeader(buffer, header.traceBlockPointers[i]);
                traceChannels.Add(temp);        //should store all data per trace

                /*foreach (var j in buffer)
                {
                    Debug.WriteLine(j);
                }*/
            }

        }
        
    }
}
