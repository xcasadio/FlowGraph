using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utils;

/// <summary>
/// Abstract base for view-model classes that need to implement INotifyPropertyChanged.
/// </summary>
public abstract class AbstractModelBase : INotifyPropertyChanged
{
#if DEBUG
    private static int _nextObjectId;

    public int ObjectDebugId { get; } = _nextObjectId++;
#endif

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Event raised to indicate that a property value has changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}