namespace ITControlCenter
{
    public partial class MenuFrm : Form
    {
        public MenuFrm()
        {
            InitializeComponent();
        }

        private void btnAD_Click(object sender, EventArgs e)
        {
            ADForm aDForm = new ADForm();
            aDForm.FormClosed += aDForm_FormClosed;
            aDForm.Show();
            this.Hide();
        }

        private void aDForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
