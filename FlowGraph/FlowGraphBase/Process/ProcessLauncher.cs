using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase.Node;
using System.ComponentModel;
using FlowGraphBase.Logger;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessLauncher : INotifyPropertyChanged
    {
        #region Instance

        private static ProcessLauncher m_Singleton = null;

        /// <summary>
        /// Gets
        /// </summary>
        public static ProcessLauncher Instance
        {
            get
            {
                if (m_Singleton == null)
                {
                    m_Singleton = new ProcessLauncher();
                }

                return m_Singleton;
            }
        }

        #endregion //Instance

		#region Fields

        private List<ProcessingContext> m_CallStacks = new List<ProcessingContext>();
        private int m_CurrentCallStackIndex = 0;

        private volatile bool m_UpdateOnlyOneStep = false;
        private volatile bool m_IsOnPause = false;
        private volatile bool m_MustStop = false;

        private ProcessingContextStep m_LastExecution = null;
        private SequenceState m_State = SequenceState.Stop;

        private BackgroundWorker m_BGWorker;
        private volatile bool m_IsClosing;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// Gets
        /// </summary>
        public SequenceState State
        {
            get { return m_State; }
            private set
            {
                if (m_State != value)
                {
                    m_State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        /// <summary>
        /// Gets
        /// </summary>
        public IEnumerable<ProcessingContext> CallStack
        {
            get { return m_CallStacks; }
        }

		#endregion //Properties
	
		#region Constructors
		
        /// <summary>
        /// 
        /// </summary>
        private ProcessLauncher()
        {

        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void StartLoop()
        {
            if (m_BGWorker == null)
            {
                m_BGWorker = new BackgroundWorker();
                m_BGWorker.DoWork += (sender1, e1) =>
                {
                    try
                    {
                        while (m_IsClosing == false)
                        {
                            ProcessLoop();
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogManager.Instance.WriteException(ex);
                    }
                };

                m_BGWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopLoop()
        {
            m_IsClosing = true;
            m_MustStop = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Resume()
        {
            State = SequenceState.Running;
            m_UpdateOnlyOneStep = false;
            m_IsOnPause = false;
            m_MustStop = false;

            if (m_LastExecution != null)
            {
                m_LastExecution.Slot.Node.IsProcessing = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NextStep()
        {
            State = SequenceState.Pause;
            m_UpdateOnlyOneStep = true;
            m_IsOnPause = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Pause()
        {
            State = SequenceState.Pause;
            m_IsOnPause = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            State = SequenceState.Stop;
            m_MustStop = true;
            m_IsOnPause = false;
            m_UpdateOnlyOneStep = false;
        }

        /// <summary>
        /// Trigger the event node with the type eventType_ only in running Sequence
        /// </summary>
        /// <param name="eventType_"></param>
        /// <param name="index_"></param>
        /// <param name="param_"></param>
        public void OnGlobalEvent(Type eventType_, int index_, object param_)
        {
            List<Sequence> seqList = new List<Sequence>();

            foreach (ProcessingContext c in m_CallStacks)
            {
                if (c.SequenceBase is Sequence)
                {
                    Sequence seq = c.SequenceBase as Sequence;

                    if (seq.ContainsEventNodeWithType(eventType_) == true
                        && seqList.Contains(seq) == false)
                    {
                        seqList.Add(seq);
                        seq.OnEvent(c, eventType_, index_, param_);
                    }
                }
            }

            //Resume();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq_"></param>
        /// <param name="eventType_"></param>
        /// <param name="index_"></param>
        /// <param name="param_"></param>
        public void LaunchSequence(SequenceBase seq_, Type eventType_, int index_, object param_)
        {
            MemoryStackFrameManager stackFrames = new MemoryStackFrameManager();
            //stackFrames.Clear();
            stackFrames.AddStackFrame(); // 1st frame
            seq_.AllocateAllVariables(stackFrames.CurrentFrame);
            ProcessingContext processContext = new ProcessingContext(seq_, stackFrames);
            seq_.OnEvent(processContext, eventType_, index_, param_);
            m_CallStacks.Add(processContext);

            Resume();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessLoop(bool sleep_ = true)
        {
            bool processing = true;

            while (processing == true
                && m_MustStop == false)
            {
                if (m_IsOnPause == false)
                {
                    processing = DoOneStep();

                    if (processing == true)
                    {
                        State = SequenceState.Running;
                    }
                }
                else if (m_UpdateOnlyOneStep == true)
                {
                    m_UpdateOnlyOneStep = false;
                    processing = DoOneStep();

                    if (processing == true)
                    {
                        State = SequenceState.Pause;
                    }
                }

                if (sleep_ == true)
                {
                    // Do sleep else all WPF bindings will block the UI thread
                    // 5ms else the UI is not enough responsive
                    System.Threading.Thread.Sleep(5);
                }
            };

            State = SequenceState.Stop;
            m_LastExecution = null;

            foreach (ProcessingContext c in m_CallStacks)
            {
                c.SequenceBase.ResetNodes();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if there is at least one node to be activated else returns false</returns>
        public bool DoOneStep()
        {
            if (GetNumberOfActiveProcess() > 0)
            {
                if (State == SequenceState.Pause)
                {
                    if (m_LastExecution != null)
                    {
                        m_LastExecution.Slot.Node.IsProcessing = false;
                    }
                }

                ProcessingContext context = m_CallStacks[m_CurrentCallStackIndex];
                ProcessingContextStep contextStep = context.PopStep();

                if (contextStep == null)
                {
                    m_CallStacks.Remove(context);
                    context.SequenceBase.ResetNodes();
                }
                else
                {
                    ActionNode node = contextStep.Slot.Node as ActionNode;
                    m_LastExecution = contextStep;

                    ActionNode.ProcessingInfo info = node.Activate(context, contextStep.Slot);
                    node.ErrorMessage = info.ErrorMessage;

                    if (info.State == ActionNode.LogicState.Error)
                    {
                        context.IsOnError = true;
                    }

                    if (State == SequenceState.Pause)
                    {
                        m_LastExecution.Slot.Node.IsProcessing = true;
                    }
                }
            }

            return SetNextProcess();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfActiveProcess()
        {
            int count = 0;

            foreach (var c in m_CallStacks)
            {
                if (c.IsOnError == false)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool SetNextProcess()
        {
            for (int i = m_CurrentCallStackIndex + 1; i < m_CallStacks.Count; i++)
            {
                if (m_CallStacks[i].IsOnError == false)
                {
                    m_CurrentCallStackIndex = i;
                    return true;
                }
            }

            for (int i = 0; i <= m_CurrentCallStackIndex && i < m_CallStacks.Count; i++)
            {
                if (m_CallStacks[i].IsOnError == false)
                {
                    m_CurrentCallStackIndex = i;
                    return true;
                }
            }

            return false;
        }

        #region IPropertyNotify

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // IPropertyNotify

		#endregion //Methods
    }
}
