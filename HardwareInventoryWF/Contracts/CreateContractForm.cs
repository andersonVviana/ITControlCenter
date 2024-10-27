using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardwareInventoryWF.Contracts
{
    public partial class CreateContractForm : Form
    {
        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_ITCONTROLCENTER;User Id=gateway;Password=2rD&2Bn?F4;";
        private int fornecedorID;

        public CreateContractForm(int id)
        {
            InitializeComponent();
            fornecedorID = id;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO CONTRACTSDETAIL (NumberOfContract, Description, Nature, AXAccount, DueDate, Status, ContractMasterID) VALUES (@NumberOfContract, @Description, @Nature, @AXAccount,@DueDate,@Status,@ContractMasterID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NumberOfContract", textBox1.Text);
                cmd.Parameters.AddWithValue("@Description", textBox4.Text);
                cmd.Parameters.AddWithValue("@Nature", comboBox2.Text);
                cmd.Parameters.AddWithValue("@Site", comboBox1.Text);
                cmd.Parameters.AddWithValue("@AXAccount", textBox3.Text);
                cmd.Parameters.AddWithValue("@DueDate", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@Status", textBox2.Text);
                cmd.Parameters.AddWithValue("@ContractMasterID", fornecedorID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Contrato adicionado com sucesso!");
            DialogResult = DialogResult.OK; // Fecha o formulário com sucesso
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void TimerAtualizacao_Tick(object sender, EventArgs e)
        {
            try
            {
                AtualizarStatusContratos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro na atualização dos contratos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void AtualizarStatusContratos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Query para obter todos os contratos e suas datas de vencimento
                string query = "SELECT ID, DueDate FROM CONTRACTSDETAIL";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<(int id, DateTime vencimento)> contratos = new List<(int, DateTime)>();

                    // Armazena os contratos e suas datas para processamento
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime vencimento = reader.GetDateTime(1);
                        contratos.Add((id, vencimento));
                    }

                    // Atualiza o status de cada contrato baseado na data atual
                    foreach (var contrato in contratos)
                    {
                        string novoStatus = CalcularStatus(contrato.vencimento);
                        AtualizarStatusNoBanco(contrato.id, novoStatus);
                    }
                }
            }
        }

        private string CalcularStatus(DateTime dataVencimento)
        {
            int diasRestantes = (dataVencimento - DateTime.Today).Days;

            if (diasRestantes > 30)
                return "Ativo";
            else if (diasRestantes <= 30 && diasRestantes > 10)
                return "Prestes a Vencer";
            else if (diasRestantes <= 10 && diasRestantes >= 0)
                return "Vencendo";
            else
                return "Vencido";
        }

        private void AtualizarStatusNoBanco(int contratoID, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE CONTRACTSDETAIL SET Status = @Status WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@ID", contratoID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
