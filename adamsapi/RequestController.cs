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
    public bool Put([FromBody] RequestResult updatedRequest) {
        string formattedDate = updatedRequest.DueDate == null ? "''": "'" + ((DateTime)updatedRequest.DueDate).Year + "-" +  ((DateTime)updatedRequest.DueDate).Month + "-" + ((DateTime)updatedRequest.DueDate).Day + " 00:00:00 AM'";
        string assignedText = updatedRequest.Assigned == null ? "null" : updatedRequest.Assigned.ToString();
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("update request set request_name = '" + updatedRequest.RequestName + "', assigned = " + assignedText + 
        ", requestor = '" + updatedRequest.Requestor + "', problem_description = '" + updatedRequest.ProblemDescription + "', priority = " + 
        updatedRequest.Priority + ", status = " + updatedRequest.Status + ", due_date = " + formattedDate + ", last_modified_date=getdate()" +
        " where request_id = " + updatedRequest.RequestId, connection);
        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();
        return true;
    }

    [HttpPost]
    [Route("request/")]
    public bool Post([FromBody] RequestResult newRequest) {
        string formattedDate = newRequest.DueDate == null ? "null": "'" + ((DateTime)newRequest.DueDate).Year + "-" +  ((DateTime)newRequest.DueDate).Month + "-" + ((DateTime)newRequest.DueDate).Day + " 00:00:00 AM'";
        SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionString"));
        SqlCommand cmd = new SqlCommand("insert into request (request_name, requestor, assigned, problem_description, priority, status, due_date, last_modified_date) " + 
        "values ('" + newRequest.RequestName + "','" + newRequest.Requestor + "'," + (newRequest.Assigned == null ? "null" : newRequest.Assigned) + ",'" + newRequest.ProblemDescription + "'," +
        newRequest.Priority + "," + newRequest.Status + "," + formattedDate + ", getdate())", connection);
        connection.Open();
        cmd.ExecuteNonQuery();
        connection.Close();
        return true;
    }
}