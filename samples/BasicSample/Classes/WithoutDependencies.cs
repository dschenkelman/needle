using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicSample.Classes
{
    public class WithoutDependencies : BaseClass
    {
        public WithoutDependencies()
        {
               base.PrintConstructorMessage("Constructing instance of class without dependencies");
        }
    }
}
