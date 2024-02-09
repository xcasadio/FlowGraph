using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class LineBreaksAndWrappingOtherConfiguration
    {
        [DefaultValue(true)]
        public bool PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceSimplePropertyIndexerEventDeclarationOnSingleLine { get; set; }

        [DefaultValue(true)]
        public bool PlaceSimpleAccessorOnSingleLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceSimpleMethodOnSingleLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceSimpleAnonymousMethodOnSingleLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceLinqExpressionOnSingleLine { get; set; }

        // Not yet supported.
        //[DefaultValue(false)]
        //public bool PlaceSimpleArrayObjectCollectionOnSingleLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceTypeAttributeOnSingleLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceMethodAttributeOnSameLine { get; set; }

        [DefaultValue(false)]
        public bool PlacePropertyIndexerEventAttributeOnSameLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceSingleLineAccessorAttributeOnSameLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceMultiLineAccessorAttributeOnSameLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceFieldAttributeOnSameLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceConstructorInitializerOnSameLine { get; set; }

        [DefaultValue(false)]
        public bool PlaceTypeConstraintsOnSameLine { get; set; }

        public LineBreaksAndWrappingOtherConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
