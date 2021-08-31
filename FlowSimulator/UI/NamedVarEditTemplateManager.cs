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
        #region Fields

        static private Dictionary<Type, DataTemplate> m_TemplatesByTypes = new Dictionary<Type, DataTemplate>(15);

        #endregion //Fields
        
        #region Properties
        
        #endregion //Properties
        
        #region Constructors

        #endregion //Constructors
        
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        static public void Initialize()
        {
            ResourceDictionary res = new ResourceDictionary();
            res.Source = new Uri("/FlowSimulator;component/UI/SharedVisualTemplates.xaml",
                        UriKind.RelativeOrAbsolute);

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
        static public void Add(Type type_, DataTemplate template_)
        {
            if (type_ == null)
            {
                throw new ArgumentNullException("Type cannot be null");
            }

            if (template_ == null)
            {
                throw new ArgumentNullException("DataTemplate cannot be null");
            }

            m_TemplatesByTypes.Add(type_, template_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        static public DataTemplate GetTemplateByType(Type type_)
        {
            if (type_ == null)
            {
                return null;
            }

            if (m_TemplatesByTypes.ContainsKey(type_))
            {
                return m_TemplatesByTypes[type_];
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        static public bool ContainsType(Type type_)
        {
            return type_ == null ? false : m_TemplatesByTypes.ContainsKey(type_);
        }
        
        #endregion //Methods
    }
}
