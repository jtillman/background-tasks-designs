using System.Collections.Generic;

namespace DefinedCaller
{
    public class BackgroundMessage {
        public BackgroundMessageType Type { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}
