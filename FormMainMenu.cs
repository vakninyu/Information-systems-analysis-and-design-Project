using System;
using System.Drawing;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormMainMenu : Form
    {
        public FormMainMenu()
        {
            InitializeComponent();

            this.Text = "Matan Health Services";
            this.StartPosition = FormStartPosition.CenterScreen;
           

        }
 
        private void FormMainMenu_Load(object sender, EventArgs e)
        {
            // אם את רוצה למקם או לשנות דברים עיצוביים (כמו מיקום לוגו או כותרת) זה המקום.
        }

        // כפתורי ניווט
        private void button1_Click(object sender, EventArgs e)
        {
            new FormCaregiverMenu().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new FormEquipmentMenu().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new FormTrainingMenu().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new FormFindCaregiverForPatient().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new FormMenuComplaint().ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new FormTimeReport().ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            new FormEquipmentMenu().ShowDialog();
        }

        private void FormMainMenu_Load_1(object sender, EventArgs e)
        {

        }
    }
}
