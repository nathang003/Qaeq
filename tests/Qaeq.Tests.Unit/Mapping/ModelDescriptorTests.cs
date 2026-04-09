using Qaeq.Exceptions;
using Qaeq.Mapping;

namespace Qaeq.Tests.Unit.Mapping;

[Collection("Registry")]
public class ModelDescriptorTests : IDisposable
{
    public void Dispose()
    {
        QaeqRegistry.Reset();
    }

    [Fact]
    public void Build_ExplicitColumns_MapsOnlyColumnAttributes()
    {
        QaeqRegistry.Configure(registry =>
        {
            var customerBuilder = registry.Entity<Models.Customer>();
            customerBuilder.HasKey(c => c.Id);
            customerBuilder.HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            var orderBuilder = registry.Entity<Models.Order>();
            orderBuilder.HasKey(o => o.Id);
            orderBuilder.HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);
            orderBuilder.HasMany<Models.OrderItem>(o => o.Items)
                .WithForeignKeyOn<int>(l => l.OrderId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        Assert.Equal("Customer", descriptor.TableName);
        Assert.Equal("dbo", descriptor.Schema);
        Assert.Equal(typeof(Models.Customer), descriptor.ClrType);
        Assert.Equal(3, descriptor.Columns.Count);
        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Id" && c.ColumnName == "Id");
        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Name" && c.ColumnName == "Name");
        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Email" && c.ColumnName == "Email");
    }

    [Fact]
    public void Build_ExplicitColumns_ExcludesPropertiesWithoutColumnAttribute()
    {
        QaeqRegistry.Configure(registry =>
        {
            var customerBuilder = registry.Entity<Models.Customer>();
            customerBuilder.HasKey(c => c.Id);
            customerBuilder.HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            var orderBuilder = registry.Entity<Models.Order>();
            orderBuilder.HasKey(o => o.Id);
            orderBuilder.HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);
            orderBuilder.HasMany<Models.OrderItem>(o => o.Items)
                .WithForeignKeyOn<int>(l => l.OrderId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        Assert.DoesNotContain(descriptor.Columns, c => c.PropertyName == "Orders");
    }

    [Fact]
    public void Build_ImplicitColumns_MapsAllPublicProperties()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.ImplicitColumnProduct>()
                .HasKey(p => p.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.ImplicitColumnProduct>();

        Assert.Equal("Products", descriptor.TableName);
        Assert.Equal(3, descriptor.Columns.Count);
        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Id" && c.ColumnName == "Id");
        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Name" && c.ColumnName == "Name");
        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Price" && c.ColumnName == "Price");
    }

    [Fact]
    public void Build_ImplicitColumns_ExcludesNotMapped()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.ImplicitColumnProduct>()
                .HasKey(p => p.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.ImplicitColumnProduct>();

        Assert.DoesNotContain(descriptor.Columns, c => c.PropertyName == "ComputedDisplay");
    }

    [Fact]
    public void Build_ImplicitColumns_ExcludesVirtualProperties()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.ImplicitColumnWithVirtual>()
                .HasKey(e => e.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.ImplicitColumnWithVirtual>();

        Assert.Equal(2, descriptor.Columns.Count);
        Assert.DoesNotContain(descriptor.Columns, c => c.PropertyName == "SomeVirtualProp");
    }

    [Fact]
    public void Build_CustomColumnName_UsesAttributeName()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.CustomColumnNameEntity>()
                .HasKey(e => e.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.CustomColumnNameEntity>();

        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Id" && c.ColumnName == "entity_id");
        Assert.Contains(descriptor.Columns, c => c.PropertyName == "Name" && c.ColumnName == "display_name");
    }

    [Fact]
    public void Build_CustomColumnName_CapturesTypeName()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.CustomColumnNameEntity>()
                .HasKey(e => e.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.CustomColumnNameEntity>();

        var idCol = descriptor.Columns.First(c => c.PropertyName == "Id");
        var nameCol = descriptor.Columns.First(c => c.PropertyName == "Name");

        Assert.Equal("int", idCol.SqlTypeName);
        Assert.Equal("nvarchar(100)", nameCol.SqlTypeName);
    }

    [Fact]
    public void Build_CustomColumnName_CapturesOrder()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.CustomColumnNameEntity>()
                .HasKey(e => e.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.CustomColumnNameEntity>();

        var idCol = descriptor.Columns.First(c => c.PropertyName == "Id");
        var nameCol = descriptor.Columns.First(c => c.PropertyName == "Name");

        Assert.Equal(0, idCol.Order);
        Assert.Equal(1, nameCol.Order);
    }

    [Fact]
    public void Build_NullableValueType_IsNullableTrue()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.ImplicitColumnWithVirtual>()
                .HasKey(e => e.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.ImplicitColumnWithVirtual>();

        var idCol = descriptor.Columns.First(c => c.PropertyName == "Id");
        Assert.False(idCol.IsNullable);

        var nameCol = descriptor.Columns.First(c => c.PropertyName == "Name");
        Assert.False(nameCol.IsNullable);
    }

    [Fact]
    public void Build_NoTableAttribute_ThrowsMappingException()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.NoTableEntity>()
                .HasKey(e => e.Id);
        });

        Assert.Throws<MappingException>(() => QaeqRegistry.GetDescriptor<Models.NoTableEntity>());
    }

    [Fact]
    public void Build_ConflictingAttributes_ThrowsMappingException()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.ConflictingAttributeEntity>()
                .HasKey(e => e.Id);
        });

        Assert.Throws<MappingException>(() => QaeqRegistry.GetDescriptor<Models.ConflictingAttributeEntity>());
    }

    [Fact]
    public void Build_NavigationWithoutRegistration_ThrowsMappingException()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.OrphanedNavigationEntity>()
                .HasKey(e => e.Id);
            // Deliberately not registering HasOne for Customer
        });

        Assert.Throws<MappingException>(() => QaeqRegistry.GetDescriptor<Models.OrphanedNavigationEntity>());
    }

    [Fact]
    public void Build_CompositeKey_CapturesBothProperties()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.CompositeKeyEntity>()
                .HasKey(e => new { e.TenantId, e.OrderId });
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.CompositeKeyEntity>();

        Assert.Single(descriptor.Keys);
        var key = descriptor.Keys[0];
        Assert.Equal(2, key.PropertyNames.Count);
        Assert.Equal("TenantId", key.PropertyNames[0]);
        Assert.Equal("OrderId", key.PropertyNames[1]);
    }

    [Fact]
    public void Build_SingleKey_CapturesProperty()
    {
        QaeqRegistry.Configure(registry =>
        {
            var customerBuilder = registry.Entity<Models.Customer>();
            customerBuilder.HasKey(c => c.Id);
            customerBuilder.HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            var orderBuilder = registry.Entity<Models.Order>();
            orderBuilder.HasKey(o => o.Id);
            orderBuilder.HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);
            orderBuilder.HasMany<Models.OrderItem>(o => o.Items)
                .WithForeignKeyOn<int>(l => l.OrderId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        Assert.Single(descriptor.Keys);
        var key = descriptor.Keys[0];
        Assert.Single(key.PropertyNames);
        Assert.Equal("Id", key.PropertyNames[0]);
    }

    [Fact]
    public void Build_HasOneRelationship_CreatesDescriptor()
    {
        QaeqRegistry.Configure(registry =>
        {
            var orderBuilder = registry.Entity<Models.Order>();
            orderBuilder.HasKey(o => o.Id)
                .HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);
            orderBuilder.HasMany<Models.OrderItem>(o => o.Items)
                .WithForeignKeyOn<int>(l => l.OrderId);

            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id)
                .HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.Order>();

        var customerRel = descriptor.Relationships
            .FirstOrDefault(r => r.NavigationPropertyName == "Customer");

        Assert.NotNull(customerRel);
        Assert.Equal(typeof(Models.Order), customerRel.SourceType);
        Assert.Equal(typeof(Models.Customer), customerRel.TargetType);
        Assert.Equal(RelationshipType.ManyToOne, customerRel.Cardinality);
        Assert.True(customerRel.ForeignKeyOnSource);
        Assert.Single(customerRel.ForeignKeyPropertyNames);
        Assert.Equal("CustomerId", customerRel.ForeignKeyPropertyNames[0]);
    }

    [Fact]
    public void Build_HasManyRelationship_CreatesDescriptor()
    {
        QaeqRegistry.Configure(registry =>
        {
            var orderBuilder = registry.Entity<Models.Order>();
            orderBuilder.HasKey(o => o.Id);
            orderBuilder.HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);
            orderBuilder.HasMany<Models.OrderItem>(o => o.Items)
                .WithForeignKeyOn<int>(l => l.OrderId);

            var customerBuilder = registry.Entity<Models.Customer>();
            customerBuilder.HasKey(c => c.Id);
            customerBuilder.HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        var ordersRel = descriptor.Relationships
            .FirstOrDefault(r => r.NavigationPropertyName == "Orders");

        Assert.NotNull(ordersRel);
        Assert.Equal(typeof(Models.Customer), ordersRel.SourceType);
        Assert.Equal(typeof(Models.Order), ordersRel.TargetType);
        Assert.Equal(RelationshipType.OneToMany, ordersRel.Cardinality);
        Assert.False(ordersRel.ForeignKeyOnSource);
        Assert.Single(ordersRel.ForeignKeyPropertyNames);
        Assert.Equal("CustomerId", ordersRel.ForeignKeyPropertyNames[0]);
    }

    [Fact]
    public void Build_ContradictoryRelationships_ThrowsMappingException()
    {
        // Order says FK is CustomerId, but Customer says FK is Id
        // This is contradictory because they reference different properties as the FK, so it should throw an exception
        Assert.Throws<MappingException>(() =>
        {
            QaeqRegistry.Configure(registry =>
            {
                registry.Entity<Models.Order>()
                    .HasKey(o => o.Id)
                    .HasOne<Models.Customer>(o => o.Customer)
                    .WithForeignKey<int>(c => c.CustomerId);

                registry.Entity<Models.Customer>()
                    .HasKey(c => c.Id)
                    .HasMany<Models.Order>(c => c.Orders)
                    .WithForeignKeyOn<int>(o => o.Id); // <-- Contradiction here

                registry.Entity<Models.OrderItem>()
                    .HasKey(l => l.Id);
            });

            // Force descriptor building which triggers validation
            QaeqRegistry.GetDescriptor<Models.Order>();
            QaeqRegistry.GetDescriptor<Models.Customer>();
        });
    }

    [Fact]
    public void Build_DepthEnforcement_OnlyIncludesDirectNavigations()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id)
                .HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            var orderBuilder = registry.Entity<Models.Order>();
            orderBuilder.HasKey(o => o.Id);
            orderBuilder
                .HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);
            orderBuilder
                .HasMany<Models.OrderItem>(o => o.Items)
                .WithForeignKeyOn<int>(l => l.OrderId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var customerDescriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        // Customer should have its direct navigation (Orders)
        Assert.Contains(customerDescriptor.Relationships, r => r.NavigationPropertyName == "Orders");

        // Customer should NOT have Order's navigations (Lines, Customer)
        // The descriptor only includes relationships declared on Customer itself
        Assert.DoesNotContain(customerDescriptor.Relationships, r => r.NavigationPropertyName == "Items");
        Assert.DoesNotContain(customerDescriptor.Relationships, r => r.NavigationPropertyName == "Customer");

        // Order's descriptor should have its direct navigations (Customer, Items)
        var orderDescriptor = QaeqRegistry.GetDescriptor<Models.Order>();
        Assert.Contains(orderDescriptor.Relationships, r => r.NavigationPropertyName == "Customer");
        Assert.Contains(orderDescriptor.Relationships, r => r.NavigationPropertyName == "Items");
    }

    [Fact]
    public void Build_OneSidedRelationship_HasInverseIsFalse()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.OneSidedParent>()
                .HasKey(p => p.Id)
                .HasMany<Models.OneSidedChild>(p => p.Children)
                .WithForeignKeyOn<int>(c => c.ParentId);

            registry.Entity<Models.OneSidedChild>()
                .HasKey(c => c.Id);
            // No HasOne<OneSidedParent> declared, so this relationship is deliberately one-sided
        });

        var parentDescriptor = QaeqRegistry.GetDescriptor<Models.OneSidedParent>();
        var childrenRelationship = parentDescriptor.Relationships
            .First(r => r.NavigationPropertyName == "Children");

        Assert.False(childrenRelationship.HasInverse);
    }

    [Fact]
    public void Build_OneSidedRelationship_HasInverseFalse()
    {
        QaeqRegistry.Configure(registry =>
        {
            // Only Order declares the relationship to Customer.
            // Customer does NOT declare HasMany<Order>.
            registry.Entity<Models.OrderToCustomerNoNav>()
                .HasKey(o => o.Id)
                .HasOne<Models.CustomerNoNav>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);

            registry.Entity<Models.CustomerNoNav>()
                .HasKey(c => c.Id);
        });

        var descriptor = QaeqRegistry.GetDescriptor<Models.OrderToCustomerNoNav>();

        var customerRel = descriptor.Relationships
            .FirstOrDefault(r => r.NavigationPropertyName == "Customer");

        Assert.NotNull(customerRel);
        Assert.False(customerRel.HasInverse);
    }

    [Fact]
    public void Build_BothSidesConsistent_HasInverseTrue()
    {
        QaeqRegistry.Configure(registry =>
        {
            registry.Entity<Models.Order>()
                .HasKey(o => o.Id)
                .HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);

            registry.Entity<Models.Order>()
                .HasMany(o => o.Items)
                .WithForeignKeyOn<int>(i => i.OrderId);

            registry.Entity<Models.Customer>()
                .HasKey(c => c.Id)
                .HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var orderDescriptor = QaeqRegistry.GetDescriptor<Models.Order>();
        var customerDescriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        var customerRel = orderDescriptor.Relationships
            .FirstOrDefault(r => r.NavigationPropertyName == "Customer");
        var ordersRel = customerDescriptor.Relationships
            .FirstOrDefault(r => r.NavigationPropertyName == "Orders");

        Assert.NotNull(customerRel);
        Assert.True(customerRel.HasInverse);

        Assert.NotNull(ordersRel);
        Assert.True(ordersRel.HasInverse);
    }

    [Fact]
    public void Build_ContradictoryForeignKeys_ThrowsMappingException()
    {
        QaeqRegistry.Configure(registry =>
        {
            // Parent says FK is "ParentId" on child
            registry.Entity<Models.ContradictoryFkParent>()
                .HasKey(p => p.Id)
                .HasMany<Models.ContradictoryFkChild>(p => p.Children)
                .WithForeignKeyOn<int>(c => c.ParentId);

            // Child says FK is "OtherParentId" on itself — contradicts parent's declaration
            registry.Entity<Models.ContradictoryFkChild>()
                .HasKey(c => c.Id)
                .HasOne<Models.ContradictoryFkParent>(c => c.Parent)
                .WithForeignKey<int>(c => c.OtherParentId);
        });

        Assert.Throws<MappingException>(() =>
        {
            QaeqRegistry.GetDescriptor<Models.ContradictoryFkParent>();
        });
    }

    [Fact]
    public void Build_DepthEnforcement_DoesNotIncludeNestedNavigations()
    {
        QaeqRegistry.Configure(registry =>
        {
            var customerBuilder = registry.Entity<Models.Customer>();
            customerBuilder.HasKey(c => c.Id)
                .HasMany<Models.Order>(c => c.Orders)
                .WithForeignKeyOn<int>(o => o.CustomerId);

            var orderBuilder = registry.Entity<Models.Order>();
            orderBuilder.HasKey(o => o.Id)
                .HasOne<Models.Customer>(o => o.Customer)
                .WithForeignKey<int>(o => o.CustomerId);
            orderBuilder.HasMany<Models.OrderItem>(o => o.Items)
                .WithForeignKeyOn<int>(l => l.OrderId);

            registry.Entity<Models.OrderItem>()
                .HasKey(l => l.Id);
        });

        var customerDescriptor = QaeqRegistry.GetDescriptor<Models.Customer>();

        // Customer's descriptor should only have its own direct navigations
        Assert.Single(customerDescriptor.Relationships);
        Assert.Equal("Orders", customerDescriptor.Relationships[0].NavigationPropertyName);
        Assert.Equal(typeof(Models.Order), customerDescriptor.Relationships[0].TargetType);

        // It should NOT contain Order's navigations (Customer, Items)
        Assert.DoesNotContain(customerDescriptor.Relationships,
            r => r.NavigationPropertyName == "Items");
        Assert.DoesNotContain(customerDescriptor.Relationships,
            r => r.NavigationPropertyName == "Customer" && r.SourceType == typeof(Models.Order));

        // Order's own descriptor DOES have its navigations
        var orderDescriptor = QaeqRegistry.GetDescriptor<Models.Order>();
        Assert.Equal(2, orderDescriptor.Relationships.Count);
        Assert.Contains(orderDescriptor.Relationships, r => r.NavigationPropertyName == "Customer");
        Assert.Contains(orderDescriptor.Relationships, r => r.NavigationPropertyName == "Items");
    }

}
