# Qaeq - Quick and Easy Queries
Qaeq (pronounced "cake") is a lightweight C# NuGet package that simplifies database mapping and querying logic. It abstracts away the complexity of data retrieval, relationship loading, and change tracking while maintaining high performance and data integrity.

## Core Features

### Data Retrieval

#### Navigation Loading
Efficiently load related data using intelligent bucketing strategies:
•	1-to-1 relationships via JOIN
•	1-to-many relationships via parallel batches
•	Override default behavior with .Include() and/or .JoinQuery

#### Depth Limit
Relationships are limited to 2 layers (including root) by default. This prevents unbounded data loading and maintains predictable query performance. Additional depth overrides may be considered in future releases.

#### Pagination
Efficient pagination with .Skip().Take() that avoids unnecessary TOP(N) clauses.

#### Record Cap
A configurable default cap (N = 1000) limits result sets, preventing accidental large data transfers. Override with .NoCap() when needed.

#### Async Streaming
Full support for asynchronous streaming via IAsyncEnumerable for memory-efficient data processing.

#### Query Timeout
30-second timeout per query segment (configurable) ensures long-running queries are caught early.

### Mapping & Modeling

#### Table & Column Mapping
Declarative mapping via [Table] and [Column] attributes on your models.

#### Relationship Definitions
Centralized Fluent Metadata Registry for defining keys, relationships, and additional configuration details.

#### Partial Models
Database columns not present in your model are silently ignored, allowing flexible model design.

#### Type Safety
Type mismatches between database and model throw IntegrityException to prevent silent data corruption.

#### SQL Discovery
Automatic validation via INFORMATION_SCHEMA queries at startup ensures schema consistency.

### Change Tracking

#### Scoped Context
Change tracking is bound to the Context instance, not database-wide, for isolation and predictability.

#### Multiple Modes
•	Full: Track all changes
•	None: Disable tracking
•	Scheduled: Time-based tracking windows

#### Integrity Checking
Detect data-to-model drift without unnecessary overhead. Focus on changes that could interfere with your application.

### Production & Performance

#### Application Intent
Query-level optimization with .WithIntent(ReadOnly) for read-heavy scenarios.

#### Lightweight Output
•	Tuples as the default return type for minimal overhead
•	Dynamic types available for complex joins or partial projections
•	Ideal when manually constructing queries with subset properties

#### Error Handling
Categorized exceptions with wrapped SQL error details for easier debugging and error recovery.

## Getting Started

Coming soon. For more information, visit the #.

---

Qaeq — Where database queries are simple, safe, and swift.