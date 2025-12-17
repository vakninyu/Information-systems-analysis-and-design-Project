using System;
using System.Data.SqlClient;

namespace MatanProject2
{
    public class Attendance
    {
        private string courseId;
        private string caregiverId;
        private string attendanceStatus;

        public Attendance(string courseId, string caregiverId, string attendanceStatus, bool is_new)
        {
            this.courseId = courseId;
            this.caregiverId = caregiverId;
            this.attendanceStatus = attendanceStatus;

            if (is_new)
            {
                this.CreateAttendance();
                Program.AttendanceList.Add(this); // ודאי שיש לך רשימה כזו בתוכנית הראשית
            }
        }

        // Getters
        public string GetCourseId() => courseId;
        public string GetCaregiverId() => caregiverId;
        public string GetAttendanceStatus() => attendanceStatus;

        // Setters
        public void SetAttendanceStatus(string status) => attendanceStatus = status;

        // יצירת רשומת נוכחות
        public void CreateAttendance()
        {
            SqlCommand c = new SqlCommand();
            c.CommandText = "INSERT INTO dbo.Attendance (courseId, caregiverId, attendanceStatus) " +
                            "VALUES (@courseId, @caregiverId, @attendanceStatus)";
            c.Parameters.AddWithValue("@courseId", courseId);
            c.Parameters.AddWithValue("@caregiverId", caregiverId);
            c.Parameters.AddWithValue("@attendanceStatus", attendanceStatus);

            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(c);
        }

        // עדכון נוכחות בקורס
        public void UpdateAttendance()
        {
            SqlCommand c = new SqlCommand();
            c.CommandText = "UPDATE dbo.Attendance " +
                            "SET attendanceStatus = @attendanceStatus " +
                            "WHERE courseId = @courseId AND caregiverId = @caregiverId";
            c.Parameters.AddWithValue("@courseId", courseId);
            c.Parameters.AddWithValue("@caregiverId", caregiverId);
            c.Parameters.AddWithValue("@attendanceStatus", attendanceStatus);

            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(c);
        }

        // מחיקת נוכחות
        public void DeleteAttendance()
        {
            Program.AttendanceList.Remove(this); // ודאי שיש לך את הרשימה הזו בתוכנית הראשית
            SqlCommand c = new SqlCommand();
            c.CommandText = "DELETE FROM dbo.Attendance " +
                            "WHERE courseId = @courseId AND caregiverId = @caregiverId";
            c.Parameters.AddWithValue("@courseId", courseId);
            c.Parameters.AddWithValue("@caregiverId", caregiverId);

            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(c);
        }
    }
}
