using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class SpacesBeforeParenthesesConfiguration
    {
        [DefaultValue(false)]
        public bool MethodCallParentheses { get; set; }

        [DefaultValue(false)]
        public bool MethodCallEmptyParentheses { get; set; }

        [DefaultValue(false)]
        public bool ArrayAccessBrackets { get; set; }

        [DefaultValue(false)]
        public bool MethodDeclarationParentheses { get; set; }

        [DefaultValue(false)]
        public bool MethodDeclarationEmptyParentheses { get; set; }

        [DefaultValue(true)]
        public bool IfParentheses { get; set; }

        [DefaultValue(true)]
        public bool WhileParentheses { get; set; }

        [DefaultValue(true)]
        public bool CatchParentheses { get; set; }

        [DefaultValue(true)]
        public bool SwitchParentheses { get; set; }

        [DefaultValue(true)]
        public bool ForParentheses { get; set; }

        [DefaultValue(true)]
        public bool ForEachParentheses { get; set; }

        [DefaultValue(true)]
        public bool UsingParentheses { get; set; }

        [DefaultValue(true)]
        public bool LockParentheses { get; set; }

        [DefaultValue(false)]
        public bool TypeOfParentheses { get; set; }

        [DefaultValue(false)]
        public bool SizeOfParentheses { get; set; }

        [DefaultValue(false)]
        public bool BeforeTypeParameterListAngle { get; set; }

        [DefaultValue(false)]
        public bool BeforeTypeArgumentListAngle { get; set; }

        public SpacesBeforeParenthesesConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
