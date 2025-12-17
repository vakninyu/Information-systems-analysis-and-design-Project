using System;
using System.Data.SqlClient;

namespace MatanProject2
{
    public class Complaint
    {
        // שדות פרטיים
        private string complaintId;
        private string caregiverId;
        private string patientId;
        private DateTime dateSubmitted;
        private string type;
        private string description;
        private string status;
        private string solution;

        // בנאי
        public Complaint(string complaintId, string caregiverId, string patientId, DateTime dateSubmitted, string type, string description, string status, string solution, bool isNew)
        {
            this.complaintId = complaintId;
            this.caregiverId = caregiverId;
            this.patientId = patientId;
            this.dateSubmitted = dateSubmitted;
            this.type = type;
            this.description = description;
            this.status = status;
            this.solution = solution;

            if (isNew)
            {
                CreateComplaint();
                Program.Complaints.Add(this); // תוודאי שקיימת רשימה ב־Program.cs
            }
        }

        // Getters
        public string GetComplaintId() => complaintId;
        public string GetCaregiverId() => caregiverId;
        public string GetPatientId() => patientId;
        public DateTime GetDateSubmitted() => dateSubmitted;
        public string GetType() => type;
        public string GetDescription() => description;
        public string GetStatus() => status;
        public string GetSolution() => solution;

        // Setters
        public void SetCaregiverId(string value) => caregiverId = value;
        public void SetPatientId(string value) => patientId = value;
        public void SetType(string value) => type = value;
        public void SetDescription(string value) => description = value;
        public void SetStatus(string value) => status = value;
        public void SetSolution(string value) => solution = value;

        // יצירת תלונה במסד הנתונים
        public void CreateComplaint()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXEC dbo.CreateComplaint @complaintId, @caregiverId, @patientId, @dateSubmitted, @type, @description, @status, @solution";
            cmd.Parameters.AddWithValue("@complaintId", complaintId);
            cmd.Parameters.AddWithValue("@caregiverId", caregiverId);
            cmd.Parameters.AddWithValue("@patientId", patientId);
            cmd.Parameters.AddWithValue("@dateSubmitted", dateSubmitted);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@solution", solution);

            SQL_CON sc = new SQL_CON();
            sc.execute_non_query(cmd);
        }

        // עדכון תלונה במסד הנתונים
        public void UpdateComplaint()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXEC dbo.UpdateComplaint @complaintId, @caregiverId, @patientId, @type, @description, @status, @solution";
            cmd.Parameters.AddWithValue("@complaintId", complaintId);
            cmd.Parameters.AddWithValue("@caregiverId", caregiverId);
            cmd.Parameters.AddWithValue("@patientId", patientId);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@solution", solution);

            SQL_CON sc = new SQL_CON();
            sc.execute_non_query(cmd);
        }
    }
}
