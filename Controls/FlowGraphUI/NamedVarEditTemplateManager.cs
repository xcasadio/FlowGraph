using System.Windows;

namespace FlowGraphUI
{
    public static class NamedVarEditTemplateManager
    {
        private static readonly Dictionary<Type, DataTemplate> TemplatesByTypes = new Dictionary<Type, DataTemplate>(15);

        public static void Initialize()
        {
            ResourceDictionary res = new ResourceDictionary
            {
                Source = new Uri("/FlowGraphUI;component/SharedVisualTemplates.xaml", UriKind.RelativeOrAbsolute)
            };

            DataTemplate numericTemplate = (DataTemplate)res["numericTemplate"];
            DataTemplate selectableTemplate = (DataTemplate)res["selectableTemplate"];
            DataTemplate checkableTemplate = (DataTemplate)res["checkableTemplate"];
            DataTemplate textTemplate = (DataTemplate)res["textTemplate"];
            DataTemplate readOnlyTemplate = (DataTemplate)res["readOnlyTemplate"];
            DataTemplate customWindowTemplate = (DataTemplate)res["customWindowTemplate"];
            DataTemplate sessionTemplate = (DataTemplate)res["sessionTemplate"];
            DataTemplate messageDatasTemplate = (DataTemplate)res["messageDatasTemplate"];

            Add(typeof(bool), checkableTemplate);

            Add(typeof(sbyte), numericTemplate);
            Add(typeof(char), numericTemplate);
            Add(typeof(short), numericTemplate);
            Add(typeof(int), numericTemplate);
            Add(typeof(long), numericTemplate);
            Add(typeof(byte), numericTemplate);
            Add(typeof(ushort), numericTemplate);
            Add(typeof(uint), numericTemplate);
            Add(typeof(ulong), numericTemplate);
            Add(typeof(float), numericTemplate);
            Add(typeof(double), numericTemplate);

            Add(typeof(string), textTemplate);

            Add(typeof(object), readOnlyTemplate);
        }

        public static void Add(Type type, DataTemplate template)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            TemplatesByTypes.Add(type, template);
        }

        public static DataTemplate GetTemplateByType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (TemplatesByTypes.ContainsKey(type))
            {
                return TemplatesByTypes[type];
            }

            return null;
        }

        public static bool ContainsType(Type type)
        {
            return type != null && TemplatesByTypes.ContainsKey(type);
        }
    }
}
