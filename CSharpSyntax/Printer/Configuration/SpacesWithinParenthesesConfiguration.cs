using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class SpacesWithinParenthesesConfiguration
    {
        [DefaultValue(false)]
        public bool Parentheses { get; set; }

        [DefaultValue(false)]
        public bool MethodDeclarationParentheses { get; set; }

        [DefaultValue(false)]
        public bool MethodDeclarationEmptyParentheses { get; set; }

        [DefaultValue(false)]
        public bool MethodCallParentheses { get; set; }

        [DefaultValue(false)]
        public bool MethodCallEmptyParentheses { get; set; }

        [DefaultValue(false)]
        public bool ArrayAccessBrackets { get; set; }

        [DefaultValue(false)]
        public bool TypeCastParentheses { get; set; }

        [DefaultValue(false)]
        public bool IfParentheses { get; set; }

        [DefaultValue(false)]
        public bool WhileParentheses { get; set; }

        [DefaultValue(false)]
        public bool CatchParentheses { get; set; }

        [DefaultValue(false)]
        public bool SwitchParentheses { get; set; }

        [DefaultValue(false)]
        public bool ForParentheses { get; set; }

        [DefaultValue(false)]
        public bool ForEachParentheses { get; set; }

        [DefaultValue(false)]
        public bool UsingParentheses { get; set; }

        [DefaultValue(false)]
        public bool LockParentheses { get; set; }

        [DefaultValue(false)]
        public bool TypeOfParentheses { get; set; }

        [DefaultValue(false)]
        public bool SizeOfParentheses { get; set; }

        [DefaultValue(false)]
        public bool TypeParameterAngles { get; set; }

        [DefaultValue(false)]
        public bool TypeArgumentAngles { get; set; }

        public SpacesWithinParenthesesConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
