using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System;

namespace CSharpSyntax.Printer.Configuration
{
    public class LineBreaksAndWrappingConfiguration
    {
        public LineBreaksAndWrappingPlaceOnNewLineConfiguration PlaceOnNewLine { get; set; }

        public LineBreaksAndWrappingLineWrappingConfiguration LineWrapping { get; set; }

        public LineBreaksAndWrappingOtherConfiguration Other { get; set; }

        public LineBreaksAndWrappingConfiguration()
        {
            PlaceOnNewLine = new LineBreaksAndWrappingPlaceOnNewLineConfiguration();
            LineWrapping = new LineBreaksAndWrappingLineWrappingConfiguration();
            Other = new LineBreaksAndWrappingOtherConfiguration();
        }
    }
}