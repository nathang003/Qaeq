using Qaeq.Exceptions;
using Qaeq.Internal;
using System.Linq.Expressions;

namespace Qaeq.Tests.Unit.Mapping;

public class ExpressionHelperTests
{
    [Fact]
    public void GetPropertyNames_SimpleMemberAccess_ReturnsSingleName()
    {
        Expression<Func<Models.Order, object>> expr = o => o.Id;

        var names = ExpressionHelper.GetPropertyNames(expr);

        Assert.Single(names);
        Assert.Equal("Id", names[0]);
    }

    [Fact]
    public void GetPropertyNames_StringProperty_ReturnsSingleName()
    {
        Expression<Func<Models.Customer, object>> expr = c => c.Name;

        var names = ExpressionHelper.GetPropertyNames(expr);

        Assert.Single(names);
        Assert.Equal("Name", names[0]);
    }

    [Fact]
    public void GetPropertyNames_AnonymousType_ReturnsMultipleNames()
    {
        Expression<Func<Models.CompositeKeyEntity, object>> expr = e => new { e.TenantId, e.OrderId };

        var names = ExpressionHelper.GetPropertyNames(expr);

        Assert.Equal(2, names.Count);
        Assert.Equal("TenantId", names[0]);
        Assert.Equal("OrderId", names[1]);
    }

    [Fact]
    public void GetPropertyNames_MethodCall_ThrowsMappingException()
    {
        Expression<Func<Models.Customer, object>> expr = c => c.Name.ToUpper();

        Assert.Throws<MappingException>(() => ExpressionHelper.GetPropertyNames(expr));
    }

    [Fact]
    public void GetPropertyNames_Constant_ThrowsMappingException()
    {
        Expression<Func<Models.Customer, object>> expr = c => 42;

        Assert.Throws<MappingException>(() => ExpressionHelper.GetPropertyNames(expr));
    }

    [Fact]
    public void GetNavigationPropertyName_SimpleMemberAccess_ReturnsName()
    {
        Expression<Func<Models.Order, Models.Customer?>> expr = o => o.Customer;

        var name = ExpressionHelper.GetNavigationPropertyName(expr);

        Assert.Equal("Customer", name);
    }

    [Fact]
    public void GetNavigationPropertyName_Collection_ReturnsName()
    {
        Expression<Func<Models.Customer, List<Models.Order>>> expr = o => o.Orders;

        var name = ExpressionHelper.GetNavigationPropertyName(expr);

        Assert.Equal("Orders", name);
    }

    [Fact]
    public void GetNavigationPropertyName_MethodCall_ThrowsMappingException()
    {
        Expression<Func<Models.Customer, string>> expr = c => c.Name.ToUpper();

        Assert.Throws<MappingException>(() => ExpressionHelper.GetNavigationPropertyName(expr));
    }
}
