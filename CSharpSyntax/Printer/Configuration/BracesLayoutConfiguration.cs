using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System;

namespace CSharpSyntax.Printer.Configuration
{
    public class BracesLayoutConfiguration
    {
        [DefaultValue(BraceFormatting.NextLine)]
        public BraceFormatting TypeAndNamespaceDeclaration { get; set; }

        [DefaultValue(BraceFormatting.NextLine)]
        public BraceFormatting MethodDeclaration { get; set; }

        [DefaultValue(BraceFormatting.NextLine)]
        public BraceFormatting AnonymousMethodDeclaration { get; set; }

        [DefaultValue(BraceFormatting.NextLine)]
        public BraceFormatting BlockUnderCaseLabel { get; set; }

        [DefaultValue(BraceFormatting.NextLine)]
        public BraceFormatting ArrayAndObjectInitializer { get; set; }

        [DefaultValue(BraceFormatting.NextLine)]
        public BraceFormatting Other { get; set; }

        [DefaultValue(EmptyBraceFormatting.OnDifferentLines)]
        public EmptyBraceFormatting EmptyBraceFormatting { get; set; }

        public BracesLayoutConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);
        }
    }
}