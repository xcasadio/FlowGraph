using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    public class OtherModifiersConfiguration
    {
        [DefaultValue(false)]
        public bool UseExplicitPrivateModifier { get; set; }

        [DefaultValue(false)]
        public bool UseExplicitInternalModifier { get; set; }

        public ModifiersCollection ModifiersOrder { get; set; }

        public bool ShouldSerializeModifiersOrder()
        {
            var defaultOrder = Enum.GetValues(typeof(Modifier)).Cast<Modifier>().ToArray();

            if (defaultOrder.Length != ModifiersOrder.Count)
                return true;

            for (int i = 0; i < defaultOrder.Length; i++)
            {
                if (defaultOrder[i] != ModifiersOrder[i])
                    return true;
            }

            return false;
        }

        public OtherModifiersConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);

            ModifiersOrder = new ModifiersCollection();
            ModifiersOrder.AddRange(Enum.GetValues(typeof(Modifier)).Cast<Modifier>());
        }
    }
}
