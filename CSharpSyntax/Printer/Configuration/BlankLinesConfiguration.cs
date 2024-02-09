using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System;

namespace CSharpSyntax.Printer.Configuration
{
    public class BlankLinesConfiguration
    {
        [DefaultValue(1)]
        public int AroundNamespace { get; set; }

        [DefaultValue(0)]
        public int InsideNamespace { get; set; }

        [DefaultValue(1)]
        public int AroundType { get; set; }

        [DefaultValue(0)]
        public int InsideType { get; set; }

        [DefaultValue(0)]
        public int AroundField { get; set; }

        [DefaultValue(1)]
        public int AroundMethod { get; set; }

        // Not yet supported.
        //[DefaultValue(1)]
        //public int AroundRegion { get; set; }

        //[DefaultValue(1)]
        //public int InsideRegion { get; set; }

        [DefaultValue(0)]
        public int BetweenDifferentUsingGroups { get; set; }

        [DefaultValue(1)]
        public int AfterUsingList { get; set; }

        [DefaultValue(1)]
        public int AfterFileHeaderComment { get; set; }

        public BlankLinesConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}