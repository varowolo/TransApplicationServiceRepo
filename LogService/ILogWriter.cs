using System;
using System.Collections.Generic;
using System.Text;

namespace TransApplicationService.LogService
{
   public interface ILogWriter
    {
        string LogWrite(string message, string type);

        string LogWarn(string message, string type);
    }
}
