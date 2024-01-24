// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Defaults;

public partial class DataTransferOptions
{
    /// <inheritdoc />
    public virtual string? ConvertResponseToJson(object? response)
    {
        return ConvertObjectToJson(response);
    }
}