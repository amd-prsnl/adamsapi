using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "MyCors", builder =>
            {
                //for when you're running on localhost
                builder.AllowAnyOrigin()
                .AllowAnyHeader().AllowAnyMethod();


                //builder.WithOrigins("url from where you're trying to do the requests") this should be specified to get it working on other environments
            });
        });

var app = builder.Build();
app.UseCors("MyCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();/*
app.MapGet("/request", () =>
{
    List<RequestResult> tbl = new List<RequestResult>();
    SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
    SqlCommand cmd = new SqlCommand("SELECT [Request_ID],[Request_Name],[Requestor],[Assigned],[Problem_Description],[Priority],[Status],[Due_Date],[Last_Modified_Date] from dbo.Request order by request_id desc", connection);
    connection.Open();
    SqlDataReader dr = cmd.ExecuteReader();
    while (dr.NextResult()) {
        RequestResult temp = new RequestResult();
        temp.RequestId = dr.GetInt32(0);
        temp.RequestName = dr.GetString(1);
        temp.Requestor = dr.GetString(2);
        temp.Assigned = dr.GetInt32(3);
        temp.ProblemDescription = dr.GetString(4);
        temp.Priority = dr.GetInt32(5);
        temp.Status = dr.GetInt32(6);
        temp.DueDate = dr.GetDateTime(7);
        temp.LastModifiedDate = dr.GetDateTime(8);
        tbl.Add(temp);
    }
    connection.Close();
    return tbl;
})
.WithName("GetRequests")
.WithOpenApi();

app.MapPut("/request", ([FromBody] RequestResult updatedRequest) =>
{
    string formattedDate = updatedRequest.DueDate == null ? null: "'" + ((DateTime)updatedRequest.DueDate).Year + "-" +  ((DateTime)updatedRequest.DueDate).Month + "-" + ((DateTime)updatedRequest.DueDate).Day + " 00:00:00 AM'";
    SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
    SqlCommand cmd = new SqlCommand("update request set request_name = '" + updatedRequest.RequestName + "', assigned = " + updatedRequest.Assigned + 
    ", requestor = '" + updatedRequest.Requestor + "', problem_description = '" + updatedRequest.ProblemDescription + "', priority = " + 
    updatedRequest.Priority + ", status = " + updatedRequest.Status + ", due_date = " + formattedDate + ", last_modified_date=getdate()" +
    " where request_id = " + updatedRequest.RequestId, connection);
    connection.Open();
    cmd.ExecuteNonQuery();
    connection.Close();
    return true;
})
.WithName("EditRequest")
.WithOpenApi();

app.MapPost("/request", ([FromBody] RequestResult newRequest) =>
{
    string formattedDate = newRequest.DueDate == null ? "''": "'" + ((DateTime)newRequest.DueDate).Year + "-" +  ((DateTime)newRequest.DueDate).Month + "-" + ((DateTime)newRequest.DueDate).Day + " 00:00:00 AM'";
    SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
    SqlCommand cmd = new SqlCommand("insert into request (request_name, requestor, assigned, problem_description, priority, status, due_date, last_modified_date) " + 
     "values ('" + newRequest.RequestName + "','" + newRequest.Requestor + "'," + newRequest.Assigned + ",'" + newRequest.ProblemDescription + "'," +
     newRequest.Priority + "," + newRequest.Status + "," + formattedDate + ", getdate())", connection);
    connection.Open();
    cmd.ExecuteNonQuery();
    connection.Close();
    return true;
})
.WithName("AddRequest")
.WithOpenApi();
*/
app.Run();

