using System;
using System.Collections.Generic;
using System.Text;

namespace CS491Final
{
    class Amp32BitFixed : Amplitudes
    {
        public override IList<float> ReadData(Byte[] buffer, int start, int end)
        {
            IList<float> retList = new List<float>();
            for (int i = start; i < end; i += 4)
            {
                float val = (float)BitConverter.ToInt32(buffer, i);
                retList.Add(val);
            }
            return retList;
        }
    }
}
