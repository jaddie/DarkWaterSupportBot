using System.Collections.Generic;
using System.IO;

namespace DarkwaterSupportBot
{
    static class WriteErrorSystem
    {
        public static void WriteError(IEnumerable<string> error)
        {
            try
            {
                using (var writer = new StreamWriter("ErrorLog.txt", true))
                {
                    writer.AutoFlush = true;
                    foreach (var line in error)
                    {
                        writer.WriteLine(line);
                    }
                    return;
                }
            }
            catch
            {
                return;
            }
        }
    }
}
