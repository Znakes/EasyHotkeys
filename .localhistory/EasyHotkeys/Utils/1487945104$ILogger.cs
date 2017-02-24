using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyHotkeys.Utils
{
    public class ILogger
    {
        void LogAsync(string message);
    }

    public class DebugLogger : ILogger
    {
        
    }

    public class NoneLogger : ILogger
    {
        
    }
}
