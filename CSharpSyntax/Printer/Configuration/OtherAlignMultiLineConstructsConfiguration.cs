using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class OtherAlignMultiLineConstructsConfiguration
    {
        [DefaultValue(false)]
        public bool MethodParameters { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool FirstCallArgumentsByParen { get; set; }

        [DefaultValue(false)]
        public bool CallArguments { get; set; }

        [DefaultValue(false)]
        public bool ListOfBaseClassesAndInterfaces { get; set; }

        [DefaultValue(false)]
        public bool Expression { get; set; }

        [DefaultValue(false)]
        public bool ChainedBinaryExpressions { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool ChainedMethodCalls { get; set; }

        [DefaultValue(false)]
        public bool ArrayObjectCollectionInitializer { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool AnonymousMethodBody { get; set; }

        [DefaultValue(false)]
        public bool ForStatementHeader { get; set; }

        [DefaultValue(false)]
        public bool MultipleDeclarations { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool TypeParametersList { get; set; }

        [DefaultValue(false)]
        public bool TypeParameterConstraints { get; set; }

        [DefaultValue(false)]
        public bool LinqQuery { get; set; }

        public OtherAlignMultiLineConstructsConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
