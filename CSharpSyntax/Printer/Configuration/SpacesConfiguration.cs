using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System;

namespace CSharpSyntax.Printer.Configuration
{
    public class SpacesConfiguration
    {
        public SpacesBeforeParenthesesConfiguration BeforeParentheses { get; set; }

        public SpacesAroundOperatorsConfiguration AroundOperators { get; set; }

        public SpacesWithinParenthesesConfiguration WithinParentheses { get; set; }

        public SpacesTernaryOperatorConfiguration TernaryOperator { get; set; }

        public SpacesOtherConfiguration Other { get; set; }

        public SpacesConfiguration()
        {
            BeforeParentheses = new SpacesBeforeParenthesesConfiguration();
            AroundOperators = new SpacesAroundOperatorsConfiguration();
            WithinParentheses = new SpacesWithinParenthesesConfiguration();
            TernaryOperator = new SpacesTernaryOperatorConfiguration();
            Other = new SpacesOtherConfiguration();
        }
    }
}