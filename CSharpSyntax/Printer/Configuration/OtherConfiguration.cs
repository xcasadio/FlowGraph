using System.Text;
using System.Collections.Generic;
using System;

namespace CSharpSyntax.Printer.Configuration
{
    public class OtherConfiguration
    {
        public OtherIndentationConfiguration Indentation { get; set; }

        public OtherModifiersConfiguration Modifiers { get; set; }

        public OtherAlignMultiLineConstructsConfiguration AlignMultiLineConstructs { get; set; }

        public OtherOtherConfiguration Other { get; set; }

        public OtherConfiguration()
        {
            Indentation = new OtherIndentationConfiguration();
            Modifiers = new OtherModifiersConfiguration();
            AlignMultiLineConstructs = new OtherAlignMultiLineConstructsConfiguration();
            Other = new OtherOtherConfiguration();
        }
    }
}
