using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLibrary
{
    [Serializable]
    public class EventPacket
    {
        public int eventId;
        public string eventValue;
    }
}
