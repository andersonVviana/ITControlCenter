using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HardwareInventoryWF.PrinterInventory
{
    public partial class PrinterInventoryForm : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";

        public PrinterInventoryForm()
        {
            InitializeComponent();

            materialButton3.Click += materialButton3_Click;

            // Configurar a coluna ID como somente leitura
            if (dataGridView2.Columns.Contains("ID"))
            {
                dataGridView2.Columns["ID"].ReadOnly = true;
            }
            else
            {
                //MessageBox.Show("A coluna 'ID' não foi encontrada.");
            }

            LoadData();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                     SELECT 
                       *
                    FROM PRINTER_INVENTORY";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Define a fonte de dados do DataGridView
                        dataGridView2.DataSource = dataTable;

                        dataGridView2.Columns["ID"].ReadOnly = true;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            CRUDPrinterForm cRUDPrinterForm = new CRUDPrinterForm();

            cRUDPrinterForm.FormClosed += (s, args) => this.Show();

            cRUDPrinterForm.Show();
        }



        private void materialButton4_Click(object sender, EventArgs e)
        {
            // Verificar se uma linha foi selecionada
            if (dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Por favor, selecione uma impressora para deletar.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obter o SerialNumber da linha selecionada
            string serialNumber = dataGridView2.CurrentRow.Cells["ID"].Value.ToString();

            // Exibir uma mensagem de confirmação
            DialogResult result = MessageBox.Show(
                $"Tem certeza de que deseja deletar a impressora com Serial: {serialNumber}?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Deletar a impressora do banco de dados
                DeletarImpressora(serialNumber);
            }
        }

        private void DeletarImpressora(string serialNumber)
        {
            // String de conexão (ajuste conforme necessário)
            var conn = connectionString;

            // Query SQL para deletar a impressora
            string query = "DELETE FROM PRINTER_INVENTORY WHERE ID = @ID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adicionar o parâmetro SerialNumber
                        command.Parameters.AddWithValue("@ID", serialNumber);

                        // Abrir a conexão e executar a query
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Verificar se a exclusão foi bem-sucedida
                        if (result > 0)
                        {
                            MessageBox.Show("Impressora deletada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Atualizar o DataGridView (recarregar os dados do banco)
                            CarregarImpressoras();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao deletar a impressora.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Exibir mensagem de erro
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CarregarImpressoras()
        {
            var conn = connectionString;
            string query = "SELECT * FROM PRINTER_INVENTORY";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }
        }

        private void PrinterInventoryForm_Load(object sender, EventArgs e)
        {
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Formata a coluna de data no DataGridView
            if (dataGridView2.Columns["WarrantyDateTo"] != null)
            {
                dataGridView2.Columns["WarrantyDateTo"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita pular para a próxima linha
                //SalvarAlteracao();
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {

            if (dataGridView2.CurrentRow != null)
            {
                var valorID = dataGridView2.CurrentRow.Cells["ID"].Value;

                if (valorID != DBNull.Value && int.TryParse(valorID.ToString(), out int PrinterID))
                {
                    // Abrir o formulário de edição com os dados do fornecedor
                    using (var formEdicao = new EditPrinterForm(PrinterID))
                    {
                        if (formEdicao.ShowDialog() == DialogResult.OK)
                        {
                            CarregarImpressoras(); // Recarrega a lista após edição
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um fornecedor válido para editar.");
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Chama o método para filtrar os dados no DataGridView
                FilterData(textBox1.Text);
            }
        }

        private void FilterData(string searchTerm)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query;

                    if (string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = @"
                          SELECT * FROM PRINTER_INVENTORY
                    ";
                    }
                    else
                    {
                        query = @"
                         SELECT 
                            *
                         FROM 
                            PRINTER_INVENTORY
                         WHERE 
                            SerialNumber LIKE @searchTerm OR
                            Name LIKE @searchTerm";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adiciona o parâmetro de pesquisa
                        command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Define a fonte de dados do DataGridView
                        dataGridView2.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }
    }
}
