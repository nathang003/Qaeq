using Qaeq.Tracking;

namespace Qaeq.Core;

/// <summary>
/// Configuration options for a <see cref="QaeqContext"/> instance.
/// </summary>
public class QaeqOptions
{
    /// <summary>
    /// The SQL Server connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// The maximum number of records returned by a query when no explicit
    /// pagination is specified. Set to 0 for no default cap.
    /// Default: 1000.
    /// </summary>
    public int DefaultRecordCap { get; set; } = 1000;

    /// <summary>
    /// The default timeout applied to each query segment.
    /// Default: 30 seconds.
    /// </summary>
    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The change tracking mode for contexts created with these options.
    /// Default: <see cref="ChangeTrackingMode.Full"/>.
    /// </summary>
    public ChangeTrackingMode TrackingMode { get; set; } = ChangeTrackingMode.Full;

    /// <summary>
    /// Whether to query INFORMATION_SCHEMA at context creation for model validation.
    /// Default: true.
    /// </summary>
    public bool EnableSchemaDiscovery { get; set; } = true;

    /// <summary>
    /// The database schemas to inspect during schema discovery.
    /// Default: ["dbo"].
    /// </summary>
    public string[] Schemas { get; set; } = ["dbo"];
}
