public class RequestResult {
        public int RequestId {get; set;}
        public string RequestName {get; set;}
        public string Requestor {get; set;}
        public int? Assigned {get; set;}
        public string ProblemDescription {get; set;}
        public int Priority {get; set;}
        public int Status {get; set;}
        public DateTime? DueDate {get; set;}
        public DateTime LastModifiedDate {get; set;}
    }