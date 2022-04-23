using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CancellableTask 
{
    private CancellationTokenSource _token;
    private Task _task;
    private bool _locked; 
    private Action _cancel; 
    public void SetTask(Action taskAction, Action cancel = null)
    {
        _cancel = cancel; 
        if(_locked) return; 
        _locked = true;
        if(_task != null)
        {
            if(_task.IsCompleted == false)
            {
                _token.Cancel();
                _cancel?.Invoke();
            }
        }
        _token = new CancellationTokenSource();
        _task = new Task(taskAction, _token.Token);
        _task.Start();
        _locked = false;
    }
    public void Cancel()
    {
        _token?.Cancel();
        _task = null; 
        _cancel?.Invoke();
    }
}
