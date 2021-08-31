using System;
using System.Collections.Generic;
using System.Windows;

namespace FlowSimulator.UI
{
    /// <summary>
    /// 
    /// </summary>
    static class NamedVarEditTemplateManager
    {
        private static readonly Dictionary<Type, DataTemplate> TemplatesByTypes = new Dictionary<Type, DataTemplate>(15);

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {
            ResourceDictionary res = new ResourceDictionary
            {
                Source = new Uri("/FlowSimulator;component/UI/SharedVisualTemplates.xaml",
                    UriKind.RelativeOrAbsolute)
            };

            DataTemplate numericTemplate = (DataTemplate) res["numericTemplate"];
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        /// <param name="template_"></param>
        public static void Add(Type type, DataTemplate template)
        {
            if (type == null)
            {
                throw new ArgumentNullException("Type cannot be null");
            }

            if (template == null)
            {
                throw new ArgumentNullException("DataTemplate cannot be null");
            }

            TemplatesByTypes.Add(type, template);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        public static bool ContainsType(Type type)
        {
            return type == null ? false : TemplatesByTypes.ContainsKey(type);
        }
    }
}
