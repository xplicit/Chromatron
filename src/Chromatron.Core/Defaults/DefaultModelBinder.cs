// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromatronModelBinder"/>.
/// </summary>
public class DefaultModelBinder : IChromatronModelBinder
{
    protected readonly IChromatronDataTransferOptions _dataTransfers;

    /// <summary>
    /// Initializes a new instance of <see cref="IChromatronModelBinder"/>.
    /// </summary>
    /// <param name="dataTransfers">The <see cref="IChromatronDataTransferOptions"/> instance.</param>
    public DefaultModelBinder(IChromatronDataTransferOptions dataTransfers)
    {
        _dataTransfers = dataTransfers;
    }

    /// <inheritdoc />
#nullable disable
    public virtual object Bind(string parameterName, Type type, JsonElement value)
    {
        try
        {
            TypeCode typeCode = Type.GetTypeCode(type);

            switch (typeCode)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return type.DefaultValue();

                case TypeCode.Boolean:
                    return value.GetBoolean();

                case TypeCode.Char:
                    {
                        var tempData = value.GetString();
                        if (tempData is not null)
                        {
                            return tempData[0];
                        }

                        return char.MinValue;
                    }

                case TypeCode.SByte:
                    return value.GetSByte();

                case TypeCode.Byte:
                    return value.GetByte();

                case TypeCode.Int16:
                    return value.GetInt16();

                case TypeCode.UInt16:
                    return value.GetUInt16();

                case TypeCode.Int32:
                    return value.GetInt32();

                case TypeCode.UInt32:
                    return value.GetUInt32();

                case TypeCode.Int64:
                    return value.GetInt64();

                case TypeCode.UInt64:
                    return value.GetUInt64();

                case TypeCode.Single:
                    return value.GetSingle();

                case TypeCode.Double:
                    return value.GetDouble();

                case TypeCode.Decimal:
                    return value.GetDecimal();

                case TypeCode.DateTime:
                    return value.GetDateTime();

                case TypeCode.String:
                    return value.GetString();

                case TypeCode.Object:
                    {
                        if (type.IsGuidtype())
                        {
                            return value.GetGuid();
                        }

                        if (type.IsDictionaryType())
                        {
                            return _dataTransfers.ConvertJsonToDictionary(value.GetRawText(), type);
                        }

                        return _dataTransfers.ConvertJsonToObject(value.GetRawText(), type);
                    }

                default:
                    return _dataTransfers.ConvertJsonToObject(value.GetRawText(), type);
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return type.DefaultValue();
    }
#nullable restore
}