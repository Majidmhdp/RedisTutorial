//using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
//builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
//{
//    var configuration = ConfigurationOptions.Parse(
//        builder.Configuration.GetConnectionString("Redis"));

//    configuration.Ssl = true;
//    configuration.AbortOnConnectFail = false;

//    return ConnectionMultiplexer.Connect(configuration);
//});



//var redis = await ConnectionMultiplexer.ConnectAsync(
//    builder.Configuration.GetConnectionString("Redis"));

//Console.WriteLine(redis.IsConnected);

//var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
//IDatabase db = redis.GetDatabase();

//await db.StringSetAsync("key", "value");
//var value = await db.StringGetAsync("key");

//Console.WriteLine(value);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
