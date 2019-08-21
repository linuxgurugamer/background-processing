using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackgroundProcessing
{
    public class ModuleBackgroundProcessing : PartModule
    {
        [KSPField(isPersistant = true)]
        public string module = "";
    }
}
