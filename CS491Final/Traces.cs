using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CS491Final
{
    public class Traces
    {
        public ushort traceId { get; set; }         //Bytes 0-1
        public ushort sizeOfBlock { get; set; }    //Bytes 2-3, also MUST BE DIVISBLE BY 4 (all blocks must start on double word)

        public uint sizeOfCorrespondingBlock { get; set; }   //Bytes 4-7
        public uint numOfSampsInBlock { get; set; }      //Bytes 8-11

        public ushort DataFormat { get; set; }               //Byte 12
        public IList<float> amplitudes { get; set; }
        public bool isEmpty { get; set; }

        //-----------------------Simplifier public variables (probably won't be included in final library)-----------------------\\
        public int counter { get; set; }

        //-------------BYTES 13 - 31 ARE RESERVED-------------\\

        public Traces()
        {
            counter = 1;
            numOfSampsInBlock = 0;
            DataFormat = 0;
            isEmpty = true;
            this.amplitudes = new List<float>();
        }

        public void storeEntireTraceHeader(Byte[] buffer, uint startIndex)
        {
            if (buffer.Count() <= 0)
            {
                return;
            }
            else
            {
                isEmpty = false;
            }
            Debug.WriteLine("START INDEX: " + startIndex);
            this.traceId = BitConverter.ToUInt16(buffer, (int)startIndex);
            this.sizeOfBlock = BitConverter.ToUInt16(buffer, (int)startIndex + 2);

            this.sizeOfCorrespondingBlock = BitConverter.ToUInt32(buffer, (int)startIndex + 4);
            this.numOfSampsInBlock = BitConverter.ToUInt32(buffer, (int)startIndex + 8);
            this.DataFormat = BitConverter.ToUInt16(buffer, (int)startIndex + 12);

            //-----------------------------------For Debugging Only-----------------------------------\\
            uint nextStartIndex = (sizeOfBlock) + startIndex;
            Debug.WriteLine("Start Index: " + startIndex + " BYTES" + "\n" + "End Index: " + nextStartIndex + " BYTES");
            //-----------------------------------For Debugging Only-----------------------------------\\

            //-----------------------------------Checking if all data is correctly formatted-----------------------------------//

            if (this.traceId == 0x4422)     //makes sure that Trace FileID is correct (standard == 0x4422)
            {
                Debug.WriteLine(counter + ". Trace FileID: " + traceId);
            }
            else { Debug.WriteLine("************Need to check on traceID."); }

            if (this.sizeOfBlock % 4 == 0)           //makes sure that the sizeOfBlock is divisible by 4 (standard for double word boundaries)
            {
                Debug.WriteLine("Size of Block: " + sizeOfBlock);
            }
            else { Debug.WriteLine("******************Need to check on sizeOfBlock."); }

            if (this.sizeOfCorrespondingBlock % 4 == 0)          //makes sure that sizeOfCorrespondingBlock is divisible by 4 
            {
                Debug.WriteLine("Size of Corresponding Block: " + sizeOfCorrespondingBlock);
            }
            else { Debug.WriteLine("******************Need to check on sizeOfCorrespondingBlock."); }

            Debug.WriteLine("Number of Samples in Block: " + numOfSampsInBlock);
            Debug.WriteLine("Data Format: " + DataFormat + "\n" + "\n");

            //---------------Switch statement that takes care of the 5 different types of DataFormats---------------\\
            Amplitudes amplitudeReader = null;
            switch (this.DataFormat)
            {
                /*case 1:
                    amplitudeReader = new Amp16BitFixed();
                    break;*/
                case 2:
                    amplitudeReader = new Amp32BitFixed();
                    break;
                /*case 3:
                    Debug.WriteLine("WE HAVE A 20 BIT FLOATING POINT LOL.");
                    break;
                case 4:
                    amplitudeReader = new Amp32BitFixed();
                    break;
                case 5:
                    amplitudeReader = new Amp64BitFloating();
                    break;*/
                default:
                    amplitudeReader = null;
                    break;
            }
            int start = (int)startIndex + this.sizeOfBlock;
            int end = (int)startIndex + this.sizeOfBlock + (int)this.sizeOfCorrespondingBlock;
            this.amplitudes = amplitudeReader.ReadData(buffer, start, end);
            counter++;
        }
        
    }
}
