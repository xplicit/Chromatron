// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromatronDataTransferOptions"/>.
/// </summary>
public partial class DataTransferOptions : IChromatronDataTransferOptions
{
    /// <summary>
    /// Initializes a new instance of <see cref="DataTransferOptions"/>.
    /// </summary>
    /// <param name="serializerOptions">The serializer type of <see cref="JsonSerializerOptions"/>.</param>
    public DataTransferOptions(JsonSerializerOptions? serializerOptions = null)
    {
        Encoding = Encoding.UTF8;
        SerializerOptions = (serializerOptions is not null) 
                            ? serializerOptions.ToSerializerOptions()
                            : new JsonSerializerOptions();
    }

    /// <inheritdoc />
    public int MaxBufferSize { get; }

    /// <inheritdoc />
    public Encoding Encoding { get; }

    /// <inheritdoc />
    public object SerializerOptions { get; }

    /// <inheritdoc />
    protected virtual JsonSerializerOptions Options
    {
        get
        {
            return SerializerOptions.ToSerializerOptions();
        }
    }
}