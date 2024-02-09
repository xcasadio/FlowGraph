using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class OtherOtherConfiguration
    {
        [DefaultValue(true)]
        public bool SpecialElseIfTreatment { get; set; }

        [DefaultValue(true)]
        public bool IndentCaseFromSwitch { get; set; }

        [DefaultValue(false)]
        public bool IndentNestedUsingStatements { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool DoNotIndentCommentsStartedAtFirstColumn { get; set; }

        public OtherOtherConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
