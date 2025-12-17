using MatanProject2;
using System.Data.SqlClient;

public class Employee
{
    private string employeeId;
    private string name;
    private string role;
    private string email;
    private string phone;
    private string branch;

    public Employee(string employeeId, string name, string role, string email, string phone, string branch, bool is_new)
    {
        this.employeeId = employeeId;
        this.name = name;
        this.role = role;
        this.email = email;
        this.phone = phone;
        this.branch = branch;

        if (is_new)
        {
            this.CreateEmployee();
            Program.Employees.Add(this); // ודאי שקיימת רשימה
        }
    }

    // Getters
    public string GetEmployeeId() => employeeId;
    public string GetName() => name;
    public string GetRole() => role;
    public string GetEmail() => email;
    public string GetPhone() => phone;
    public string GetBranch() => branch;

    // Setters
    public void SetName(string value) => name = value;
    public void SetRole(string value) => role = value;
    public void SetEmail(string value) => email = value;
    public void SetPhone(string value) => phone = value;
    public void SetBranch(string value) => branch = value;

    // Create
    public void CreateEmployee()
    {
        SqlCommand c = new SqlCommand();
        c.CommandText = "EXECUTE dbo.CreateEmployee @employeeId, @name, @role, @email, @phone, @branch";
        c.Parameters.AddWithValue("@employeeId", employeeId);
        c.Parameters.AddWithValue("@name", name);
        c.Parameters.AddWithValue("@role", role);
        c.Parameters.AddWithValue("@email", email);
        c.Parameters.AddWithValue("@phone", phone);
        c.Parameters.AddWithValue("@branch", branch);

        SQL_CON SC = new SQL_CON();
        SC.execute_non_query(c);
    }

    // עדכונים ומחיקה - כמו בדוגמת הקורס
}
