using Qaeq.Exceptions;
using Qaeq.Mapping;

namespace Qaeq.Tests.Unit.Mapping;

[Collection("Registry")]
public class QaeqRegistryTests : IDisposable
{
    public void Dispose()
    {
        QaeqRegistry.Reset();
    }

    [Fact]
    public void IsConfigured_BeforeConfigure_ReturnsFalse()
    {
        Assert.False(QaeqRegistry.IsConfigured);
    }

    [Fact]
    public void IsConfigured_AfterConfigure_ReturnsTrue()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id);
        });

        Assert.True(QaeqRegistry.IsConfigured);
    }

    [Fact]
    public void Configure_CalledTwice_ThrowsConfigurationException()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id);
        });

        Assert.Throws<ConfigurationException>(() =>
        {
            QaeqRegistry.Configure(registry =>
            {
                registry.Entity<Models.Customer>()
                    .HasKey(c => c.Id);
            });
        });
    }

    [Fact]
    public void GetDescriptor_BeforeConfigure_ThrowsConfigurationException()
    {
        Assert.Throws<ConfigurationException>(() => QaeqRegistry.GetDescriptor<Models.Customer>());
    }

    [Fact]
    public void GetDescriptor_UnregisteredType_ThrowsMappingException()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id);
        });

        Assert.Throws<MappingException>(() => QaeqRegistry.GetDescriptor<Models.Order>());
    }

    [Fact]
    public void GetDescriptor_RegisteredType_ReturnsDescriptor()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id)
                .HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        Assert.NotNull(descriptor);
        Assert.Equal(typeof(Models.Customer), descriptor.ClrType);
    }

    [Fact]
    public void GetDescriptor_CalledTwice_ReturnsCachedInstance()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id)
                .HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);
        });

        var descriptor1 = QaeqRegistry.GetDescriptor<Models.Customer>();
        var descriptor2 = QaeqRegistry.GetDescriptor<Models.Customer>();

        Assert.Same(descriptor1, descriptor2);
    }
}
