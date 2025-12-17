using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormViewAllCaregivers : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormViewAllCaregivers()
        {
            InitializeComponent();

            // חיבור הכפתור לפונקציה
            button1.Click += button1_Click;
            button2.Click += button2_Click;

            // רישום אירועים
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            // טקסט התחלתי
            textBox1.Text = "Search by ID";
            textBox1.ForeColor = Color.Gray;

            textBox1.TextChanged += textBox1_TextChanged; //השורה רושמת את הפונקציה כארוע שמתרחש אוטמטית בכל פעם שהתוכן משתנה בטקסט בוקס

        }

        private void FormViewAllCaregivers_Load(object sender, EventArgs e)
        {
            LoadCaregiverData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadCaregiverData();
        }

        private void LoadCaregiverData()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("dbo.Get_all_Caregivers");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = sql.execute_query(cmd);
                DataTable dt = new DataTable();
                dt.Load(reader);

                dataGridView1.DataSource = dt;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading caregivers: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string id = textBox1.Text.Trim();

            if (id == "Search by ID" || string.IsNullOrEmpty(id))
            {
                LoadCaregiverData(); // מציג את כל המטפלים
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("dbo.SearchCaregiverByIdPartial");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@caregiverId", id);

                SqlDataReader reader = sql.execute_query(cmd);
                DataTable dt = new DataTable();
                dt.Load(reader);

                dataGridView1.DataSource = dt;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching caregiver: " + ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(id) && id != "Search")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.SearchCaregiverByIdPartial");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@caregiverId", id);

                    SqlDataReader reader = sql.execute_query(cmd);
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dataGridView1.DataSource = dt;
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching caregiver: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter an ID to search.");
            }
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search by ID")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Search by ID";
                textBox1.ForeColor = Color.Gray;
            }
        }


    }
}
