using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class SpacesTernaryOperatorConfiguration
    {
        [DefaultValue(true)]
        public bool BeforeQuestionMark { get; set; }

        [DefaultValue(true)]
        public bool AfterQuestionMark { get; set; }

        [DefaultValue(true)]
        public bool BeforeColon { get; set; }

        [DefaultValue(true)]
        public bool AfterColon { get; set; }

        public SpacesTernaryOperatorConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
