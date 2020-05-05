using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CS491Final
{
    public class Header
    {
        public ushort fileId;                 //Bytes 0-1
        public ushort revNumber;              //Bytes 2-3

        public ushort sizeOfTracePointer;     //Bytes 4-5
        public ushort numOfTraces;          //Bytes 6-7

        public sbyte sizeOfStringTerm;       //Byte 8
        public sbyte firstStringTerm;        //Byte 9
        public sbyte secondStringTerm;       //Byte 10
        public sbyte sizeOfLineTerm;         //Byte 11

        public sbyte firstLineTerm;          //Byte 12
        public sbyte secondLineTerm;         //Byte 13

        public uint[] traceBlockPointers { get; set; }       //runs in function in main, stores each trace data [12 or numOfTraces]

        public int traceCounter;

        public bool isEmpty;

        public Header()
        {
            this.traceBlockPointers = null;
            traceCounter = 1;
            isEmpty = true;
        }

        public void readHeader(Byte[] buffer, string filePath)
        {
            FileStream filestream = new FileStream(filePath, FileMode.Open);
            if (buffer.Count() <= 0)
            {
                return;
            }
            else
            {
                isEmpty = false;
            }
            this.fileId = BitConverter.ToUInt16(buffer, 0);

            this.revNumber = BitConverter.ToUInt16(buffer, 2);

            this.sizeOfTracePointer = BitConverter.ToUInt16(buffer, 4);
            this.numOfTraces = BitConverter.ToUInt16(buffer, 6);

            this.sizeOfStringTerm = (sbyte)filestream.ReadByte();
            this.firstStringTerm = (sbyte)filestream.ReadByte();
            this.secondStringTerm = (sbyte)filestream.ReadByte();
            this.sizeOfLineTerm = (sbyte)filestream.ReadByte();

            this.firstLineTerm = (sbyte)filestream.ReadByte();
            this.secondLineTerm = (sbyte)filestream.ReadByte();

            filestream.Close();

            traceBlockPointers = new uint[numOfTraces];
            for (int i = 0; i < numOfTraces; i++)
            {
                this.traceBlockPointers[i] = BitConverter.ToUInt32(buffer, 32 + ((i) * 4));         //equation from the document
                Debug.WriteLine(traceCounter + ". " + this.traceBlockPointers[i]);
                traceCounter++;                                                                 //simply keeps track of the amount of traces
            }
            Debug.WriteLine("Trace Counter: " + traceCounter);

            if (fileId.ToString().Contains("14933"))        //14933 = 3a55, which is what every fileID should be at the beginning of a SEG2 file
            {
                Debug.WriteLine("\n" + "File Descriptor Block: " + fileId.ToString());
            }
            else { Debug.WriteLine("************Need to fix fileID."); }

            Debug.WriteLine("Revision Number: " + revNumber);

            if (sizeOfTracePointer % 4 == 0)        //makes sure that the sizeOfBlock is divisible by 4 (standard for double word boundaries)
            {
                Debug.WriteLine("Size of Trace Pointer: " + sizeOfTracePointer);
            }
            else { Debug.WriteLine("*****************Need to fix tracePointer."); }

            if (numOfTraces <= (sizeOfTracePointer / 4))             //as stated in document, numOfTraces must be <= sizeOfTracePoint / 4            --> (each trace takes up 4 bytes of mem?)
            {
                Debug.WriteLine("Number of Traces: " + numOfTraces);
            }
            else { Debug.WriteLine("*******************Need to fix numberOfTraces."); }

            Debug.WriteLine("Size of String Term: " + sizeOfStringTerm.ToString());
            Debug.WriteLine("First String Term: " + firstStringTerm);
            Debug.WriteLine("Second String Term: " + secondStringTerm);
            Debug.WriteLine("Size of Line Term: " + sizeOfLineTerm);
            Debug.WriteLine("First Line Term: " + firstLineTerm);
            Debug.WriteLine("Second Line Term: " + secondLineTerm + "\n" + "\n");
        }
    }
}
