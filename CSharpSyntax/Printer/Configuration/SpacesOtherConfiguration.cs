using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class SpacesOtherConfiguration
    {
        [DefaultValue(false)]
        public bool AfterTypeCastParentheses { get; set; }

        [DefaultValue(false)]
        public bool BeforeComma { get; set; }

        [DefaultValue(true)]
        public bool AfterComma { get; set; }

        [DefaultValue(false)]
        public bool BeforeForSemicolon { get; set; }

        [DefaultValue(true)]
        public bool AfterForSemicolon { get; set; }

        [DefaultValue(false)]
        public bool BeforeColonInAttribute { get; set; }

        [DefaultValue(true)]
        public bool AfterColonInAttribute { get; set; }

        [DefaultValue(true)]
        public bool BeforeBaseTypesListColon { get; set; }

        [DefaultValue(true)]
        public bool AfterBaseTypesListColon { get; set; }

        [DefaultValue(false)]
        public bool AroundDot { get; set; }

        [DefaultValue(true)]
        public bool AroundLambdaArrow { get; set; }

        // Not yet supported.
        //[DefaultValue(true)]
        //public bool BeforeSingleLineAccessorsBlock { get; set; }

        //[DefaultValue(true)]
        //public bool WithinSingleLineAccessor { get; set; }

        //[DefaultValue(true)]
        //public bool BetweenAccessorsInSingleLinePropertyEvent { get; set; }

        [DefaultValue(false)]
        public bool SpacesBetweenEmptyBraces { get; set; }

        //[DefaultValue(true)]
        //public bool WithinSingleLineMethod { get; set; }

        //[DefaultValue(true)]
        //public bool WithinSingleLineAnonymousMethod { get; set; }

        [DefaultValue(false)]
        public bool WithinAttributeBrackets { get; set; }

        [DefaultValue(false)]
        public bool BeforeArrayRankBrackets { get; set; }

        [DefaultValue(false)]
        public bool WithinArrayRankBrackets { get; set; }

        [DefaultValue(false)]
        public bool WithinArrayRankEmptyBrackets { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool WithinSingleLineInitializerBraces { get; set; }

        [DefaultValue(false)]
        public bool BeforeSemicolon { get; set; }

        [DefaultValue(false)]
        public bool BeforeColonInCaseStatement { get; set; }

        [DefaultValue(false)]
        public bool BeforeNullableMark { get; set; }

        [DefaultValue(true)]
        public bool BeforeTypeParameterConstraintColon { get; set; }

        [DefaultValue(true)]
        public bool AfterTypeParameterConstraintColon { get; set; }

        [DefaultValue(true)]
        public bool AroundEqualsInNamespaceAliasDirective { get; set; }

        // Not yet supported.
        //[DefaultValue(true)]
        //public bool BeforeEndOfLineComment { get; set; }

        public SpacesOtherConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
