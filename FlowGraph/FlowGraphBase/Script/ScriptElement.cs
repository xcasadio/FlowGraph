using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using FlowGraphBase.Logger;
using FlowGraphBase.Node.StandardActionNode;
using Microsoft.CSharp;

namespace FlowGraphBase.Script
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptElement : INotifyPropertyChanged
    {
		#region Fields

        public delegate bool ScriptEntryDelegate(ScriptSlotDataCollection params_, ScriptSlotDataCollection ret_);
        public ScriptEntryDelegate _ScriptDelegate;

        private static int _FreeID;

        private string _SourceCode = string.Empty;
        private string _Name;

        public event EventHandler<FunctionSlotChangedEventArg> FunctionSlotChanged;

        private readonly ObservableCollection<SequenceFunctionSlot> _Slots = new ObservableCollection<SequenceFunctionSlot>();
        private int _NextSlotId;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public string ScriptSourceCode
        {
            get => _SourceCode;
            set
            { 
                _SourceCode = value;
                OnPropertyChanged("ScriptSourceCode");
            }
        }

        /// <summary>
        /// Gets/Sets
        /// </summary>
        private string ScriptSourceCodeBackup
        {
            get => _SourceCode;
            set
            {
                _SourceCode = value;
                OnPropertyChanged("ScriptSourceCode");
            }
        }

        /// <summary>
        /// Gets/Sets
        /// </summary>
        private string LastScriptSourceCodeCompiled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public bool IsBuildUpToDate => string.Equals(LastScriptSourceCodeCompiled, ScriptSourceCode);

        /// <summary>
        /// Gets
        /// </summary>
        public CompilerErrorCollection CompilationErrors
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get => _Name;
            set 
            { 
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int InputCount
        {
            get
            {
                int count = 0;

                foreach (SequenceFunctionSlot s in _Slots)
                {
                    if (s.SlotType == FunctionSlotType.Input)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OutputCount
        {
            get
            {
                int count = 0;

                foreach (SequenceFunctionSlot s in _Slots)
                {
                    if (s.SlotType == FunctionSlotType.Output)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SequenceFunctionSlot> Inputs
        {
            get
            {
                foreach (SequenceFunctionSlot s in _Slots)
                {
                    if (s.SlotType == FunctionSlotType.Input)
                    {
                        yield return s;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SequenceFunctionSlot> Outputs
        {
            get
            {
                foreach (SequenceFunctionSlot s in _Slots)
                {
                    if (s.SlotType == FunctionSlotType.Output)
                    {
                        yield return s;
                    }
                }
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ScriptElement()
        {
            ID = _FreeID++;

            ScriptSourceCode = "";
            ScriptSourceCodeBackup = "";
            LastScriptSourceCodeCompiled = "";

            _Slots.CollectionChanged += OnSlotCollectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ScriptElement(XmlNode node_)
        {
            Load(node_);
            _Slots.CollectionChanged += OnSlotCollectionChanged;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            ScriptSourceCode =
                ScriptSourceCodeBackup =
                LastScriptSourceCodeCompiled = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsChanges()
        {
            return !(ScriptSourceCode.Equals(ScriptSourceCodeBackup));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public void AddInput(string name_)
        {
            AddSlot(new SequenceFunctionSlot(++_NextSlotId, FunctionSlotType.Input) { Name = name_ });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public void AddOutput(string name_)
        {
            AddSlot(new SequenceFunctionSlot(++_NextSlotId, FunctionSlotType.Output) { Name = name_ });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot_"></param>
        private void AddSlot(SequenceFunctionSlot slot_)
        {
            slot_.IsArray = false;
            slot_.VariableType = typeof(int);

            _Slots.Add(slot_);

            if (FunctionSlotChanged != null)
            {
                FunctionSlotChanged(this, new FunctionSlotChangedEventArg(FunctionSlotChangedType.Added, slot_));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void RemoveSlotById(int id_)
        {
            foreach (var slot in _Slots)
            {
                if (slot.ID == id_)
                {
                    _Slots.Remove(slot);

                    if (FunctionSlotChanged != null)
                    {
                        FunctionSlotChanged(this, new FunctionSlotChangedEventArg(FunctionSlotChangedType.Removed, slot));
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSlotCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Inputs");
            OnPropertyChanged("Outputs");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">All value from the input nodes.</param>
        /// <param name="returnVals">All value to set into the output nodes.</param>
        /// <returns>True is the script is executed successfully</returns>
        public bool Run(ScriptSlotDataCollection parameters, ScriptSlotDataCollection returnVals)
        {
            if (_ScriptDelegate == null)
            {
                return false;
            }

            return _ScriptDelegate(parameters, returnVals);
        }

        #region Compilation

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CompilScript()
        {
            LastScriptSourceCodeCompiled = ScriptSourceCode;
            return CompilScript(ScriptSourceCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcText_"></param>
        /// <returns></returns>
        public bool CompilScript(string srcText_)
        {
            try
            {
                string finalSrcCode = _SrcFileTemplate.Replace("##_SCRIPT_CODE_##", srcText_);
                finalSrcCode = finalSrcCode.Replace("\r\n", "\n").Replace("\n", "\r\n");

                finalSrcCode = finalSrcCode.Replace("##_CUSTOM_USING_##", ""); //TODO
                finalSrcCode = finalSrcCode.Replace("##_SCRIPT_ELEMENT_ID_##", ID.ToString());

                CompilerParameters CompilerParams = new CompilerParameters();
                string outputDirectory = Directory.GetCurrentDirectory();

                CompilerParams.GenerateInMemory = true;
                CompilerParams.TreatWarningsAsErrors = false;
                CompilerParams.GenerateExecutable = false;
                CompilerParams.CompilerOptions = "/optimize";

                string currentPocessName = AppDomain.CurrentDomain.FriendlyName;
                //if launch by visual studio
                currentPocessName = currentPocessName.Replace(".vshost", "");

                string[] references = { "System.dll", "System.Core.dll",
                                      "System.Data.dll", "System.Xml.dll",
                                      "System.Xml.Linq.dll", "System.Windows.Forms.dll",
                                      "System.Windows.dll", "FlowGraphBase.dll",
                                      currentPocessName};
                CompilerParams.ReferencedAssemblies.AddRange(references);

                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerResults compile = provider.CompileAssemblyFromSource(CompilerParams, finalSrcCode);

                if (compile.Errors.HasErrors)
                {
                    CompilationErrors = compile.Errors;

                    StringBuilder text = new StringBuilder(5000);
                    text.AppendFormat("Script({0}) <{1}> : Compile error(s): \n", ID, Name);

                    foreach (CompilerError ce in compile.Errors)
                    {
                        text.Append("\t");
                        text.AppendLine(ce.ToString());
                    }

                    text = text.Replace("{", "{{").Replace("}", "}}");

                    LogManager.Instance.WriteLine(LogVerbosity.Error, text.ToString());

                    return false;
                }

                LogManager.Instance.WriteLine(LogVerbosity.Info, "Script({0}) <{1}> Build succeeded.", ID, Name);
                //ExploreAssembly(compile.CompiledAssembly);
                FindDelegates(compile.CompiledAssembly.GetModules()[0]);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// For debugging purpose
        /// </summary>
        /// <param name="assembly"></param>
        void ExploreAssembly(Assembly assembly)
        {
            LogManager.Instance.WriteLine(LogVerbosity.Trace, "Modules in the assembly:");
            foreach (Module m in assembly.GetModules())
            {
                LogManager.Instance.WriteLine(LogVerbosity.Trace, "{0}", m);

                foreach (Type t in m.GetTypes())
                {
                    LogManager.Instance.WriteLine(LogVerbosity.Trace, "\t{0}", t.Name);

                    foreach (MethodInfo mi in t.GetMethods())
                    {
                        LogManager.Instance.WriteLine(LogVerbosity.Trace, "\t\t{0}", mi.Name);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module_"></param>
        private void FindDelegates(Module module_)
        {
            if (module_ == null)
            {
                LogManager.Instance.WriteLine(LogVerbosity.Error, "ScriptManager.FindDelegates() : Module is null");
                return;
            }

            try
            {
                Type mt = null;

                mt = module_.GetType("FlowSimulatorScriptManagerNamespace._MyInternalScript_" + ID);


                Type scriptDataColl = typeof(ScriptSlotDataCollection);
                Type boolType = typeof(bool);


                foreach (MethodInfo methInfo in mt.GetMethods())
                {
                    if (methInfo.ReturnParameter.ParameterType.Equals(boolType)
                        && methInfo.GetParameters().Length == 2)
                    {
                        if (methInfo.GetParameters()[0].ParameterType.Equals(scriptDataColl)
                            && methInfo.GetParameters()[1].ParameterType.FullName.Equals(scriptDataColl.FullName))
                        {
                            try
                            {
                                _ScriptDelegate = (ScriptEntryDelegate)Delegate.CreateDelegate(
                                        typeof(ScriptEntryDelegate), methInfo);
                            }
                            catch (Exception ex)
                            {
                                Exception newEx = new Exception("Try to create a OnMessageCreateDelegate with function " + methInfo.Name, ex);
                                LogManager.Instance.WriteException(newEx);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        #endregion // Compilation

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // INotifyPropertyChanged Members

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Save(XmlNode node_)
        {
            const int version = 1; //v1.0.0.0

            XmlNode scriptNode = node_.OwnerDocument.CreateElement("Script");
            node_.AppendChild(scriptNode);

            scriptNode.AddAttribute("version", version.ToString());
            scriptNode.AddAttribute("id", ID.ToString());
            scriptNode.AddAttribute("name", Name);

            XmlNode slotListNode = node_.OwnerDocument.CreateElement("SlotList");
            scriptNode.AppendChild(slotListNode);

            //save slots
            foreach (SequenceFunctionSlot s in _Slots)
            {
                XmlNode slotNode = node_.OwnerDocument.CreateElement("Slot");
                slotListNode.AppendChild(slotNode);

                string typeName = s.VariableType.AssemblyQualifiedName;
                int index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
                typeName = typeName.Substring(0, index);

                slotNode.AddAttribute("type", Enum.GetName(typeof(FunctionSlotType), s.SlotType));
                slotNode.AddAttribute("varType", typeName);
                slotNode.AddAttribute("isArray", s.IsArray.ToString());
                slotNode.AddAttribute("name", s.Name);
                slotNode.AddAttribute("id", s.ID.ToString());
            }

            XmlNode codeNode = node_.OwnerDocument.CreateElement("Code");
            scriptNode.AppendChild(codeNode);
            XmlText txtXML = scriptNode.OwnerDocument.CreateTextNode(ScriptSourceCode);
            codeNode.AppendChild(txtXML);

            ScriptSourceCodeBackup = ScriptSourceCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptElementNode"></param>
        public void Load(XmlNode scriptElementNode)
        {
            try
            {
                Clear();

                int version = int.Parse(scriptElementNode.Attributes["version"].Value);
                Name = scriptElementNode.Attributes["name"].Value;
                ScriptSourceCode = scriptElementNode.SelectSingleNode("Code").InnerText;
                ScriptSourceCodeBackup = ScriptSourceCode;
                LastScriptSourceCodeCompiled = "";

                ID = int.Parse(scriptElementNode.Attributes["id"].Value);
                if (_FreeID <= ID)
                {
                    _FreeID = ID + 1;
                }

                foreach (XmlNode node in scriptElementNode.SelectNodes("SlotList/Slot"))
                {
                    int id = int.Parse(node.Attributes["id"].Value);
                    FunctionSlotType type = (FunctionSlotType)Enum.Parse(typeof(FunctionSlotType), node.Attributes["type"].Value);

                    if (_NextSlotId <= id) _NextSlotId = id + 1;

                    SequenceFunctionSlot slot = new SequenceFunctionSlot(id, type)
                    {
                        Name = node.Attributes["name"].Value,
                        IsArray = bool.Parse(node.Attributes["isArray"].Value),
                        VariableType = Type.GetType(node.Attributes["varType"].Value)
                    };

                    _Slots.Add(slot);
                }

                CompilScript();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        #endregion //Persistence

		#endregion //Methods

        /// <summary>
        /// 
        /// </summary>
        private readonly string _SrcFileTemplate =
            @"using System;
            using System.Collections.Generic;
            using System.ComponentModel;
            using System.Data;
            using System.Globalization;
            using System.IO;
            using System.Linq;
            using System.Text;
            using System.Text.RegularExpressions;        
            using System.Threading;
            using System.Xml;
            using FlowSimulator;
            using FlowGraphBase.Logger;
            using FlowSimulator.Messages;
            using FlowGraphBase.Node;
            using FlowGraphBase.Node.StandardActionNode;
            using FlowGraphBase.Script;
            using FlowSimulator.Sessions;
            
            ##_CUSTOM_USING_##

            namespace FlowSimulatorScriptManagerNamespace
            {
	            static public class _MyInternalScript_##_SCRIPT_ELEMENT_ID_##
	            {
		            ##_SCRIPT_CODE_##
	            }
            }";
    }
}
