using FlowGraph;
using FlowGraph.Nodes;
using FlowGraph.Nodes.StandardVariable;
using FlowGraph.Process;
using NFluent;

namespace FlowGraphTest
{
    public class SequenceBaseTest
    {
        [Test]
        public void Should_change_name()
        {
            var sequence = new Sequence(string.Empty);
            const string newName = "test";
            sequence.Name = newName;
            Check.That(newName).IsEqualTo(sequence.Name);
        }

        [Test]
        public void Should_change_description()
        {
            var sequence = new Sequence(string.Empty);
            const string newDesc = "test";
            sequence.Description = newDesc;
            Check.That(newDesc).IsEqualTo(sequence.Description);
        }

        [Test]
        public void Should_count_1_when_add_one_node()
        {
            var sequence = new Sequence("test");
            sequence.AddNode(new SequenceNodeDummy());
            Check.That(1).IsEqualTo(sequence.NodeCount);
        }

        [Test]
        public void Should_remove_node()
        {
            var sequence = new Sequence("test");
            var sequenceNodeDummy = new SequenceNodeDummy();
            sequence.AddNode(sequenceNodeDummy);
            sequence.RemoveNode(sequenceNodeDummy);
            Check.That(0).IsEqualTo(sequence.NodeCount);
        }

        [Test]
        public void Should_count_0_nodes_when_release_all_nodes()
        {
            var sequence = new Sequence("test");
            sequence.AddNode(new SequenceNodeDummy());
            sequence.RemoveAllNodes();
            Check.That(0).IsEqualTo(sequence.NodeCount);
        }

        [Test]
        public void Should_return_node_by_id()
        {
            var sequence = new Sequence("test");
            SequenceNode sequenceNodeDummy = new SequenceNodeDummy();
            sequence.AddNode(sequenceNodeDummy);
            Check.That(sequenceNodeDummy)
                .IsEqualTo(sequence.GetNodeById(sequenceNodeDummy.Id));
        }

        [Test]
        public void Should_allocate_memory_in_MemoryStack()
        {
            var sequence = new Sequence("test");
            var stringNode = new VariableNodeString
            {
                Value = "Test"
            };
            sequence.AddNode(stringNode);
            var memoryStack = new MemoryStack();
            sequence.AllocateAllVariables(memoryStack);
            Check.That(stringNode.Value)
                .IsEqualTo(memoryStack.GetValueFromId(stringNode.Id).Value);
        }

        [Test]
        public void Should_reset_all_nodes()
        {
            var sequence = new Sequence("test");
            var sequenceNodeDummy = new SequenceNodeDummy();
            sequence.AddNode(sequenceNodeDummy);
            sequenceNodeDummy.CustomText = "test";
            sequenceNodeDummy.IsProcessing = true;
            sequence.ResetNodes();
            Check.That(sequenceNodeDummy.CustomText).IsEqualTo(null);
            Check.That(sequenceNodeDummy.IsProcessing).IsEqualTo(false);
        }
    }

    public class SequenceNodeDummy : SequenceNode
    {
        protected override void InitializeSlots()
        { }

        protected override SequenceNode CopyImpl()
        {
            throw new NotImplementedException();
        }

        public override NodeType NodeType => NodeType.Action;

        public override string Title => "SequenceNodeDummy";
    }
}
