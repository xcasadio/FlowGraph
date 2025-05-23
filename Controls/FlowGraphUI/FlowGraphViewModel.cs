﻿using System.Collections.ObjectModel;
using FlowGraph;
using Utils;

namespace FlowGraphUI;

public class FlowGraphViewModel : AbstractModelBase
{
    private readonly FlowGraphManager _flowGraphManager;
    private SequenceViewModel _sequenceViewModel;

    public int Id => _flowGraphManager.Sequence.Id;
    public string Name => _flowGraphManager.Sequence.Name;
    public string Description => _flowGraphManager.Sequence.Description;
    public FlowGraphManager FlowGraphManager => _flowGraphManager;

    public SequenceViewModel SequenceViewModel
    {
        get => _sequenceViewModel;
        private set => SetField(ref _sequenceViewModel, value);
    }

    public ObservableCollection<SequenceViewModel> Functions { get; } = new();

    public FlowGraphViewModel(FlowGraphManager flowGraphManager)
    {
        _flowGraphManager = flowGraphManager;
        _sequenceViewModel = new SequenceViewModel(flowGraphManager.Sequence);
    }

    public bool IsValidSequenceName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        return !string.Equals(Name, name);
    }

    public bool IsValidFunctionName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        foreach (var seq in FlowGraphManager.Functions)
        {
            if (string.Equals(seq.Name, name))
            {
                return false;
            }
        }

        return true;
    }

    public void AddFunction(SequenceViewModel sequenceViewModel)
    {
        FlowGraphManager.Functions.Add(sequenceViewModel.SequenceBase as SequenceFunction);
        Functions.Add(sequenceViewModel);
    }

    public void RemoveFunction(SequenceViewModel sequenceViewModel)
    {
        FlowGraphManager.Functions.Remove(sequenceViewModel.SequenceBase as SequenceFunction);
        Functions.Remove(sequenceViewModel);
    }
    
    public void ClearFunctions()
    {
        FlowGraphManager.Functions.Clear();
        Functions.Clear();
    }

    public void Initialize()
    {
        ClearFunctions();

        SequenceViewModel = new SequenceViewModel(_flowGraphManager.Sequence);

        foreach (var sequenceFunction in _flowGraphManager.Functions)
        {
            AddFunction(new SequenceViewModel(sequenceFunction));
        }
    }
}