// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

/// <summary>
/// The ready state.
/// </summary>
public enum ReadyState
{
    /// <summary>
    /// The not initialized - request not initialized 
    /// </summary>
    NotInitialized = 0,

    /// <summary>
    /// The server connection established.
    /// </summary>
    ServerConnectionEstablished = 1,

    /// <summary>
    /// The request received.
    /// </summary>
    RequestReceived = 2,

    /// <summary>
    /// The processing request.
    /// </summary>
    ProcessingRequest = 3,

    /// <summary>
    /// The response is ready - Request finished and response is ready
    /// </summary>
    ResponseIsReady = 4
}