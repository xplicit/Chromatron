// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron;

/// <summary>
/// The action task.
/// </summary>
internal sealed class ActionTask : CefTask
{
    /// <summary>
    /// Run a function on specified thread.
    /// </summary>
    /// <param name="threadId">The thread identifier.</param>
    /// <param name="action">The method to run.</param>
    internal static void PostTask(CefThreadId threadId, Action action)
    {
        CefRuntime.PostTask(threadId, new ActionTask(action));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionTask"/> class.
    /// </summary>
    /// <param name="action">
    /// The action.
    /// </param>
    public ActionTask(Action action)
    {
        Action = action;
    }

    /// <summary>
    /// The execute.
    /// </summary>
    protected override void Execute()
    {
        if (Action is not null)
        {
            Action();
        }
    }

    /// <summary>
    /// Gets or sets the action.
    /// </summary>
    private Action? Action { get; set; }
}