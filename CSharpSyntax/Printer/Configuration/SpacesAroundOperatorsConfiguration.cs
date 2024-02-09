using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class SpacesAroundOperatorsConfiguration
    {
        [DefaultValue(true)]
        public bool AssignmentOperators { get; set; }

        [DefaultValue(true)]
        public bool LogicalOperators { get; set; }

        [DefaultValue(true)]
        public bool EqualityOperators { get; set; }

        [DefaultValue(true)]
        public bool RelationalOperators { get; set; }

        [DefaultValue(true)]
        public bool BitwiseOperators { get; set; }

        [DefaultValue(true)]
        public bool AdditiveOperators { get; set; }

        [DefaultValue(true)]
        public bool MultiplicativeOperators { get; set; }

        [DefaultValue(true)]
        public bool ShiftOperators { get; set; }

        [DefaultValue(true)]
        public bool NullCoalescingOperator { get; set; }

        public SpacesAroundOperatorsConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
