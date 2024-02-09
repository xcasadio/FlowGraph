using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class LineBreaksAndWrappingPlaceOnNewLineConfiguration
    {
        [DefaultValue(true)]
        public bool PlaceElseOnNewLine { get; set; }

        [DefaultValue(true)]
        public bool PlaceWhileOnNewLine { get; set; }

        [DefaultValue(true)]
        public bool PlaceCatchOnNewLine { get; set; }

        [DefaultValue(true)]
        public bool PlaceFinallyOnNewLine { get; set; }

        public LineBreaksAndWrappingPlaceOnNewLineConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}
