// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

public static class HandlerBaseExtension
{
    /// <summary>
    /// The invoke async if possible.
    /// Executes the action asynchronously on the UI thread if possible. Does not block execution on the calling thread.
    /// </summary>
    /// <param name="handler">
    /// The handler.
    /// </param>
    /// <param name="action">
    /// The action.
    /// </param>
    public static void InvokeAsyncIfPossible(this object handler, Action action)
    {
        var task = new Task(action);
        task.ContinueWith(r =>
        {
            switch (task.Status)
            {
                case TaskStatus.Canceled:
                    break;
                case TaskStatus.Faulted:
                    action.Invoke();
                    break;
                case TaskStatus.RanToCompletion:
                    break;
            }
        });

        task.Start();
    }
}