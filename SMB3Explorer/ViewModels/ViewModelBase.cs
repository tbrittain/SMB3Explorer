using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SMB3Explorer.ViewModels;

public abstract class ViewModelBase : ObservableObject, IDisposable
{
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}