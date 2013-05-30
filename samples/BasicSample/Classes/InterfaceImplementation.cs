using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicSample.Classes
{
    public class InterfaceImplementation : BaseClass, IInterface
    {
        public InterfaceImplementation()
        {
            base.PrintConstructorMessage("Constructing instance of class that implements interface");
        }
    }
}
