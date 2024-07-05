using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITControlCenter
{
    public partial class ChangePasswordForm : Form
    {
        public string NewPassword { get; private set; }

        public ChangePasswordForm()
        {
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newPassword = textBox1.Text;
            string repeatPassword = textBox2.Text;

            if (newPassword != repeatPassword)
            {
                MessageBox.Show("As senhas digitadas não coincidem. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Se as senhas coincidem, definir a nova senha e fechar o formulário com DialogResult.OK
            NewPassword = newPassword;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
