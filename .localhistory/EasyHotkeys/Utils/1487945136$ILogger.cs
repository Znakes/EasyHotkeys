using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyHotkeys.Utils
{
    public interface ILogger
    {
        void LogAsync(string message);
    }

    public class DebugLogger : ILogger
    {
        public void LogAsync(string message)
        {
            Debug.Print(message);
        }
    }

    public class NoneLogger : ILogger
    {
        public void LogAsync(string message)
        {
            // Do nothing
        }
    }
}
