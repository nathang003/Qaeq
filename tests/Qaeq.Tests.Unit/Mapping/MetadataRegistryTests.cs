using Qaeq.Exceptions;
using Qaeq.Mapping.Internal;

namespace Qaeq.Tests.Unit.Mapping;

public class MetadataRegistryTests
{
    [Fact]
    public void Entity_ReturnsEntityBuilder()
    {
        var registry = new MetadataRegistry();

        var builder = registry.Entity<Models.Customer>();

        Assert.NotNull(builder);
    }

    [Fact]
    public void Entity_CalledTwiceForSameType_ReturnsSameBuilder()
    {
        var registry = new MetadataRegistry();

        var builder1 = registry.Entity<Models.Customer>();
        var builder2 = registry.Entity<Models.Customer>();

        Assert.Same(builder1, builder2);
    }

    [Fact]
    public void Entity_AfterFreeze_ThrowsConfigurationException()
    {
        var registry = new MetadataRegistry();
        registry.Freeze();

        Assert.Throws<ConfigurationException>(() => registry.Entity<Models.Customer>());
    }

    [Fact]
    public void Freeze_CalledTwice_ThrowsConfigurationException()
    {
        var registry = new MetadataRegistry();
        registry.Freeze();

        Assert.Throws<ConfigurationException>(() => registry.Freeze());
    }

    [Fact]
    public void TryGetBuilder_RegisteredType_ReturnsBuilder()
    {
        var registry = new MetadataRegistry();
        registry.Entity<Models.Customer>();

        var builder = registry.TryGetBuilder<Models.Customer>();

        Assert.NotNull(builder);
    }

    [Fact]
    public void TryGetBuilder_UnregisteredType_ReturnsNull()
    {
        var registry = new MetadataRegistry();

        var builder = registry.TryGetBuilder<Models.Customer>();

        Assert.Null(builder);
    }
}
