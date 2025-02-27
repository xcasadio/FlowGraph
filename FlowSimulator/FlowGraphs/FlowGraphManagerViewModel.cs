using System.Collections.ObjectModel;
using System.Xml;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraphUI;
using Newtonsoft.Json.Linq;

namespace FlowSimulator.FlowGraphs
{
    public class FlowGraphManagerViewModel
    {
        private readonly FlowGraphManager _flowGraphManager;

        public ObservableCollection<SequenceViewModel> FlowGraphs { get; } = new ObservableCollection<SequenceViewModel>();

        public FlowGraphManagerViewModel()
        {
            _flowGraphManager = new FlowGraphManager();
        }

        public SequenceViewModel GetViewModelById(int id)
        {
            foreach (SequenceViewModel vm in FlowGraphs)
            {
                if (vm.Id == id)
                {
                    return vm;
                }
            }

            return null;
        }

        internal void Clear()
        {
            FlowGraphs.Clear();
        }

        internal void Add(SequenceViewModel viewModel)
        {
            FlowGraphs.Add(viewModel);
        }

        internal void Remove(SequenceViewModel viewModel)
        {
            FlowGraphs.Remove(viewModel);
        }

        public bool IsChanges()
        {
            return false;
        }

        public void Load(JObject node)
        {
            try
            {
                _flowGraphManager.Load(node);

                throw new NotImplementedException();
                //int version = int.Parse(node.SelectSingleNode("GraphList").Attributes["version"].Value);
                //
                //foreach (XmlNode graphNode in node.SelectNodes("GraphList/Graph"))
                //{
                //    //create sequence
                //    //load
                //    //FlowGraphs.Add(new SequenceViewModel(graphNode));
                //}
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        public void Save(JObject node)
        {
            // Create all sequence to reflect all changes make (because it is not done in real time)
            foreach (SequenceViewModel wm in FlowGraphs)
            {
                wm.CreateSequence();
            }

            _flowGraphManager.Save(node);

            const int version = 1;

            throw new NotImplementedException();

            var graphArrayNode = new JArray();

            foreach (SequenceViewModel wm in FlowGraphs)
            {
                var newObject = new JObject();
                //wm.Save(newObject);
                graphArrayNode.Add(newObject);
            }
        }
    }
}
