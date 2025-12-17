using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormViewTrainingCourse : Form
    {
        public FormViewTrainingCourse()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void LoadCourses()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "EXEC dbo.GetAllTrainingCourses";

            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(cmd);

            DataTable dt = new DataTable();
            dt.Load(rdr);
            rdr.Close();

            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadCourses();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }

    }
}
