using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class OtherIndentationConfiguration
    {
        [DefaultValue(1)]
        public int ContinuousLineIndentMultiplier { get; set; }

        public OtherIndentationConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
