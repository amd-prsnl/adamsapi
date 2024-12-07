using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

[ApiController]
public class RequestController : Controller {
    public RequestController() {

    }

    [HttpGet]
    [Route("request/")]
    public List<RequestResult> Get() {
        List<RequestResult> tbl = new List<RequestResult>();
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("SELECT [Request_ID],[Request_Name],[Requestor],[Assigned],[Problem_Description],[Priority],[Status],[Due_Date],[Last_Modified_Date] from dbo.Request order by request_id desc", connection);
        connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read()) {
            RequestResult temp = new RequestResult();
            temp.RequestId = dr.GetInt32(0);
            temp.RequestName = dr.GetString(1);
            temp.Requestor = dr.GetString(2);
            if (!dr.IsDBNull(3)) temp.Assigned = dr.GetInt32(3);
            temp.ProblemDescription = dr.GetString(4);
            temp.Priority = dr.GetInt32(5);
            temp.Status = dr.GetInt32(6);
            if (!dr.IsDBNull(7)) temp.DueDate = dr.GetDateTime(7);
            temp.LastModifiedDate = dr.GetDateTime(8);
            tbl.Add(temp);
        }
        connection.Close();
        return tbl;
    }

    [HttpPut]
    [Route("request/")]
    public List<RequestResult> Put([FromBody] RequestResult updatedRequest) {
        List<RequestResult> tbl = new List<RequestResult>();
        string formattedDate = updatedRequest.DueDate == null ? "''": "'" + ((DateTime)updatedRequest.DueDate).Year + "-" +  ((DateTime)updatedRequest.DueDate).Month + "-" + ((DateTime)updatedRequest.DueDate).Day + " 00:00:00 AM'";
        string assignedText = updatedRequest.Assigned == null ? "null" : updatedRequest.Assigned.ToString();
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("update request set request_name = '" + updatedRequest.RequestName + "', assigned = " + assignedText + 
        ", requestor = '" + updatedRequest.Requestor + "', problem_description = '" + updatedRequest.ProblemDescription + "', priority = " + 
        updatedRequest.Priority + ", status = " + updatedRequest.Status + ", due_date = " + formattedDate + ", last_modified_date=getdate()" +
        " where request_id = " + updatedRequest.RequestId, connection);
        connection.Open();
        cmd.ExecuteNonQuery();
        
        cmd = new SqlCommand("SELECT [Request_ID],[Request_Name],[Requestor],[Assigned],[Problem_Description],[Priority],[Status],[Due_Date],[Last_Modified_Date] from dbo.Request order by request_id desc", connection);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read()) {
            RequestResult temp = new RequestResult();
            temp.RequestId = dr.GetInt32(0);
            temp.RequestName = dr.GetString(1);
            temp.Requestor = dr.GetString(2);
            if (!dr.IsDBNull(3)) temp.Assigned = dr.GetInt32(3);
            temp.ProblemDescription = dr.GetString(4);
            temp.Priority = dr.GetInt32(5);
            temp.Status = dr.GetInt32(6);
            if (!dr.IsDBNull(7)) temp.DueDate = dr.GetDateTime(7);
            temp.LastModifiedDate = dr.GetDateTime(8);
            tbl.Add(temp);
        }
        connection.Close();
        return tbl;
    }

    [HttpPost]
    [Route("request/")]
    public List<RequestResult> Post([FromBody] RequestResult newRequest) {
        List<RequestResult> tbl = new List<RequestResult>();
        string formattedDate = newRequest.DueDate == null ? "null": "'" + ((DateTime)newRequest.DueDate).Year + "-" +  ((DateTime)newRequest.DueDate).Month + "-" + ((DateTime)newRequest.DueDate).Day + " 00:00:00 AM'";
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("insert into request (request_name, requestor, assigned, problem_description, priority, status, due_date, last_modified_date) " + 
        "values ('" + newRequest.RequestName + "','" + newRequest.Requestor + "'," + (newRequest.Assigned == null ? "null" : newRequest.Assigned) + ",'" + newRequest.ProblemDescription + "'," +
        newRequest.Priority + "," + newRequest.Status + "," + formattedDate + ", getdate())", connection);
        connection.Open();
        cmd.ExecuteNonQuery();

        cmd = new SqlCommand("SELECT [Request_ID],[Request_Name],[Requestor],[Assigned],[Problem_Description],[Priority],[Status],[Due_Date],[Last_Modified_Date] from dbo.Request order by request_id desc", connection);
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read()) {
            RequestResult temp = new RequestResult();
            temp.RequestId = dr.GetInt32(0);
            temp.RequestName = dr.GetString(1);
            temp.Requestor = dr.GetString(2);
            if (!dr.IsDBNull(3)) temp.Assigned = dr.GetInt32(3);
            temp.ProblemDescription = dr.GetString(4);
            temp.Priority = dr.GetInt32(5);
            temp.Status = dr.GetInt32(6);
            if (!dr.IsDBNull(7)) temp.DueDate = dr.GetDateTime(7);
            temp.LastModifiedDate = dr.GetDateTime(8);
            tbl.Add(temp);
        }
        connection.Close();
        return tbl;
    }

    [HttpGet]
    [Route("status/")]
    public List<Status> Status() {
        List<Status> statuses = new List<Status>();
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("SELECT * from status", connection);
        connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read()) {
            Status temp = new Status();
            temp.StatusId = dr.GetInt32(0);
            temp.StatusDescription = dr.GetString(1);
            statuses.Add(temp);
        }
        connection.Close();
        return statuses;
    }

    [HttpGet]
    [Route("priority/")]
    public List<Priority> Priorities() {
        List<Priority> priorities = new List<Priority>();
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("SELECT * from priority", connection);
        connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read()) {
            Priority temp = new Priority();
            temp.PriorityId = dr.GetInt32(0);
            temp.PriorityDescription = dr.GetString(1);
            priorities.Add(temp);
        }
        connection.Close();
        return priorities;
    }

    [HttpGet]
    [Route("user/")]
    public List<User> Users() {
        List<User> users = new List<User>();        
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("SELECT * from [user]", connection);
        connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read()) {
            User temp = new User();
            temp.UserId = dr.GetInt32(0);
            temp.UserName = dr.GetString(1);
            users.Add(temp);
        }
        connection.Close();
        return users;
    }

    [HttpGet]
    [Route("request/{id}")]
    public RequestResult Request(int id) {
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("SELECT * from request where request_id = " + id, connection);
        connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        dr.Read();

        RequestResult temp = new RequestResult();
        temp.RequestId = dr.GetInt32(0);
        temp.RequestName = dr.GetString(1);
        temp.Requestor = dr.GetString(2);
        if (!dr.IsDBNull(3)) temp.Assigned = dr.GetInt32(3);
        temp.ProblemDescription = dr.GetString(4);
        temp.Priority = dr.GetInt32(5);
        temp.Status = dr.GetInt32(6);
        if (!dr.IsDBNull(7)) temp.DueDate = dr.GetDateTime(7);
        temp.LastModifiedDate = dr.GetDateTime(8);
        connection.Close();
        return temp;
    }
}