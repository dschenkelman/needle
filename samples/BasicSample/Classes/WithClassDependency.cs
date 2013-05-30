using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Needle.Container;
using Needle.Attributes;

namespace BasicSample.Classes
{
    public class WithClassDependency : BaseClass
    {
        [Constructor]
        public WithClassDependency(WithoutDependencies dep)
        {
            base.PrintConstructorMessage("Constructing instance of class with class dependency");
        }
    }
}
