using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicSample.Classes
{
    public abstract class BaseClass
    {
        protected void PrintConstructorMessage(string message) 
        {
            
            ConsoleHelpers.WriteMessage(String.Format
                ("{0}. Instance HashCode:{1}",
                message,
                this.GetHashCode()), MessageKind.Action);
        }
    }
}
