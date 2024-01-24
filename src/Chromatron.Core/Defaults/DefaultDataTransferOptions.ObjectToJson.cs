// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Defaults;

public partial class DataTransferOptions
{
    /// <inheritdoc />
    public virtual string? ConvertObjectToJson(object? value)
    {
        try
        {
            if (value is null)
            {
                return null;
            }

            var stream = value as Stream;
            if (stream is not null)
            {
                using var reader = new StreamReader(stream, Encoding);
                value = reader.ReadToEnd();
            }

            if (value.IsValidJson())
            {
                return value.ToString();
            }

            return JsonSerializer.Serialize(value, Options);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        return value.ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}