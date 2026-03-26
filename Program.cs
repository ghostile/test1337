var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => new
{
    message = "API is running",
    swagger = "enabled",
    utc = DateTime.UtcNow
});

app.MapGet("/claims/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        claimId = id,
        status = "Pending"
    });
});

app.MapPost("/claims", (CreateClaimRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.MemberName))
        return Results.BadRequest(new { error = "MemberName is required" });

    if (request.Amount <= 0)
        return Results.BadRequest(new { error = "Amount must be greater than zero" });

    return Results.Ok(new
    {
        message = "Claim accepted",
        memberName = request.MemberName,
        amount = request.Amount,
        status = "Pending"
    });
});

app.Run();

record CreateClaimRequest(string MemberName, decimal Amount);