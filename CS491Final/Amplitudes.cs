using System;
using System.Collections.Generic;
using System.Text;

namespace CS491Final
{
    public abstract class Amplitudes
    {
        public abstract IList<float> ReadData(Byte[] buffer, int start, int numSamp);       //will stream in all data points into IList<float>
    }
}
