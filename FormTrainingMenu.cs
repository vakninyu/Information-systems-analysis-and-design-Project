using System;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormTrainingMenu : Form
    {
        public FormTrainingMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCreateTrainingCourse form = new FormCreateTrainingCourse();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormEditDeleteTrainingCourse form = new FormEditDeleteTrainingCourse();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormViewTrainingCourse form = new FormViewTrainingCourse();
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormRegisterForTraining form = new FormRegisterForTraining();
            form.Show();
        }

        // אם אין צורך בפעולה בלחיצה על הלייבל אפשר למחוק את זה או להשאיר ריק
        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormManageAttendance form = new FormManageAttendance();
            form.Show();
        }
    }
}
