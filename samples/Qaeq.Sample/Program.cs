using Qaeq.Core;

using var context = new QaeqContext(new QaeqOptions
{
    ConnectionString = "Server=localhost;Database=Sample;Trusted_Connection=true;"
});

// Also works via builder:
// using var context = QaeqContext.Configure(opts =>
// {
//     opts.ConnectionString = "Server=localhost;Database=Sample;Trusted_Connection=true;";
// });

Console.WriteLine("Qaeq context created successfully.");