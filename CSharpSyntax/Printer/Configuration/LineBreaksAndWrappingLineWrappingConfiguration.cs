using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class LineBreaksAndWrappingLineWrappingConfiguration
    {
        [DefaultValue(false)]
        public bool WrapLongLines { get; set; }

        [DefaultValue(120)]
        public int RightMargin { get; set; }

        [DefaultValue(WrapStyle.SimpleWrap)]
        public WrapStyle WrapFormalParameters { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool PreferWrapBeforeParenInDeclaration { get; set; }

        //[DefaultValue(false)]
        //public bool PreferWrapAfterParenInDeclaration { get; set; }

        [DefaultValue(WrapStyle.SimpleWrap)]
        public WrapStyle WrapInvocationArguments { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool PreferWrapBeforeParenInInvocation { get; set; }

        //[DefaultValue(false)]
        //public bool PreferWrapAfterParenInInvocation { get; set; }

        [DefaultValue(false)]
        public bool PreferWrapAfterDotInMethodCalls { get; set; }

        [DefaultValue(WrapStyle.ChopIfLong)]
        public WrapStyle WrapChainedMethodCalls { get; set; }

        [DefaultValue(WrapStyle.SimpleWrap)]
        public WrapStyle WrapExtendsImplementsList { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool PreferWrapBeforeColon { get; set; }

        [DefaultValue(WrapStyle.ChopIfLong)]
        public WrapStyle WrapForStatementHeader { get; set; }

        [DefaultValue(WrapStyle.ChopIfLong)]
        public WrapStyle WrapTernaryExpression { get; set; }

        [DefaultValue(WrapStyle.ChopIfLong)]
        public WrapStyle WrapMultipleDeclarations { get; set; }

        [DefaultValue(false)]
        public bool PreferWrapBeforeOperatorInBinaryExpression { get; set; }

        [DefaultValue(false)]
        public bool ForceChopCompoundConditionInIfStatement { get; set; }

        [DefaultValue(false)]
        public bool ForceChopCompoundConditionInWhileStatement { get; set; }

        [DefaultValue(false)]
        public bool ForceChopCompoundConditionInDoStatement { get; set; }

        [DefaultValue(WrapStyle.ChopIfLong)]
        public WrapStyle WrapMultipleTypeParameterConstraints { get; set; }

        [DefaultValue(WrapStyle.ChopIfLong)]
        public WrapStyle WrapObjectAndCollectionInitializers { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool PreferWrapBeforeFirstConstraint { get; set; }

        //[DefaultValue(false)]
        //public bool PreferWrapBeforeTypeParametersOpeningAngle { get; set; }

        public LineBreaksAndWrappingLineWrappingConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
