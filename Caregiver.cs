using MatanProject2;
using System;
using System.Data.SqlClient;

namespace MatanProject2
{
    public class Caregiver
    {
        private string caregiverId;
        private string firstName;
        private string lastName;
        private string address;
        private string gender;
        private string languages;
        private int experience;
        private string phone;
        private DateTime startDate;
        private string workArea;
        private string employmentStatus;
        private string treatmentType;

        public Caregiver(string id, string firstName, string lastName, string address,
                         string gender, string languages, int experience, string phone,
                         DateTime startDate, string workArea, string employmentStatus,
                         string treatmentType, bool is_new)
        {
            this.caregiverId = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.gender = gender;
            this.languages = languages;
            this.experience = experience;
            this.phone = phone;
            this.startDate = startDate;
            this.workArea = workArea;
            this.employmentStatus = employmentStatus;
            this.treatmentType = treatmentType;

            if (is_new)
            {
                this.CreateCaregiver();
                Program.Caregivers.Add(this); // ודאי שיש רשימה כזו בתוכנית הראשית
            }
        }

        // Getters
        public string GetId() => caregiverId;
        public string GetFirstName() => firstName;
        public string GetLastName() => lastName;
        public string GetAddress() => address;
        public string GetGender() => gender;
        public string GetLanguages() => languages;
        public int GetExperience() => experience;
        public string GetPhone() => phone;
        public DateTime GetStartDate() => startDate;
        public string GetWorkArea() => workArea;
        public string GetEmploymentStatus() => employmentStatus;
        public string GetTreatmentType() => treatmentType;

        // Setters
        public void SetFirstName(string name) => firstName = name;
        public void SetLastName(string name) => lastName = name;
        public void SetAddress(string addr) => address = addr;
        public void SetGender(string g) => gender = g;
        public void SetLanguages(string langs) => languages = langs;
        public void SetExperience(int exp) => experience = exp;
        public void SetPhone(string phoneNum) => phone = phoneNum;
        public void SetStartDate(DateTime date) => startDate = date;
        public void SetWorkArea(string area) => workArea = area;
        public void SetEmploymentStatus(string status) => employmentStatus = status;
        public void SetTreatmentType(string type) => treatmentType = type;

        // SQL Operations
        public void CreateCaregiver()
        {
            SqlCommand c = new SqlCommand("EXECUTE dbo.CreateCaregiver @caregiverId, @firstName, @lastName, @address, @gender, @languages, @experience, @phone, @startDate, @workArea, @employmentStatus, @treatmentType");
            c.Parameters.AddWithValue("@caregiverId", caregiverId);
            c.Parameters.AddWithValue("@firstName", firstName);
            c.Parameters.AddWithValue("@lastName", lastName);
            c.Parameters.AddWithValue("@address", address);
            c.Parameters.AddWithValue("@gender", gender);
            c.Parameters.AddWithValue("@languages", languages);
            c.Parameters.AddWithValue("@experience", experience);
            c.Parameters.AddWithValue("@phone", phone);
            c.Parameters.AddWithValue("@startDate", startDate);
            c.Parameters.AddWithValue("@workArea", workArea);
            c.Parameters.AddWithValue("@employmentStatus", employmentStatus);
            c.Parameters.AddWithValue("@treatmentType", treatmentType);

            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(c);
        }

        public void UpdateCaregiver()
        {
            SqlCommand c = new SqlCommand("EXECUTE dbo.UpdateCaregiver @caregiverId, @firstName, @lastName, @address, @gender, @languages, @experience, @phone, @startDate, @workArea, @employmentStatus, @treatmentType");
            c.Parameters.AddWithValue("@caregiverId", caregiverId);
            c.Parameters.AddWithValue("@firstName", firstName);
            c.Parameters.AddWithValue("@lastName", lastName);
            c.Parameters.AddWithValue("@address", address);
            c.Parameters.AddWithValue("@gender", gender);
            c.Parameters.AddWithValue("@languages", languages);
            c.Parameters.AddWithValue("@experience", experience);
            c.Parameters.AddWithValue("@phone", phone);
            c.Parameters.AddWithValue("@startDate", startDate);
            c.Parameters.AddWithValue("@workArea", workArea);
            c.Parameters.AddWithValue("@employmentStatus", employmentStatus);
            c.Parameters.AddWithValue("@treatmentType", treatmentType);

            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(c);
        }

        public void DeleteCaregiver()
        {
            Program.Caregivers.Remove(this);
            SqlCommand c = new SqlCommand("EXECUTE dbo.DeleteCaregiver @caregiverId");
            c.Parameters.AddWithValue("@caregiverId", caregiverId);

            SQL_CON SC = new SQL_CON();
            SC.execute_non_query(c);
        }
    }
}
