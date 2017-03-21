using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goertzel.Event
{
    public class DecodingEvents :  EventArgs
    {
        public List<double> FreqPowerResult { get; set; }
    }
}
