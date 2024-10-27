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
    public partial class ContractsForm : Form
    {
        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_ITCONTROLCENTER;User Id=gateway;Password=2rD&2Bn?F4;";

        public ContractsForm()
        {
            InitializeComponent();

            CarregarFornecedores();

        }

        private void CarregarFornecedores()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ID, Supplier, Type, CNPJ, RelationType, Contact1, Contact2, Phone, Phone2, Email FROM CONTRACTSMASTER";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var valorID = dataGridView1.CurrentRow.Cells["ID"].Value;

                if (valorID != DBNull.Value && int.TryParse(valorID.ToString(), out int fornecedorID))
                {
                    CarregarContratos(fornecedorID); // Carrega contratos do fornecedor selecionado
                }
            }
        }

        private void CarregarContratos(int fornecedorID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ID, NumberOfContract, Description, Nature, Site, AXAccount, DueDate, Status FROM CONTRACTSDETAIL WHERE ContractMasterID = @FornecedorID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FornecedorID", fornecedorID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt; // Bind the data to the second DataGridView
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            var createSupllierForm = new CreateSupllierForm();
            createSupllierForm.FormClosed += (s, args) => CarregarFornecedores(); // Atualiza a lista ao fechar
            createSupllierForm.ShowDialog();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            // Verifica se há uma linha selecionada no dgvFornecedores
            if (dataGridView1.CurrentRow != null)
            {
                var valorID = dataGridView1.CurrentRow.Cells["ID"].Value;

                if (valorID != DBNull.Value && int.TryParse(valorID.ToString(), out int fornecedorID))
                {
                    // Confirmação antes de deletar
                    var confirmResult = MessageBox.Show(
                        "Deseja realmente excluir este fornecedor e todos os contratos vinculados?",
                        "Confirmar Exclusão",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        try
                        {
                            DeletarFornecedorEContratos(fornecedorID);
                            CarregarFornecedores(); // Atualiza a lista de fornecedores
                            dataGridView2.DataSource = null; // Limpa o segundo grid
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao excluir: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um fornecedor para deletar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void DeletarFornecedorEContratos(int fornecedorID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Deleta os contratos vinculados ao fornecedor
                    string deleteContractsQuery = "DELETE FROM CONTRACTSDETAIL WHERE ContractMasterID = @ContractMasterID";
                    using (SqlCommand cmd = new SqlCommand(deleteContractsQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ContractMasterID", fornecedorID);
                        cmd.ExecuteNonQuery();
                    }

                    // Deleta o fornecedor
                    string deleteFornecedorQuery = "DELETE FROM CONTRACTSMASTER WHERE ID = @ID";
                    using (SqlCommand cmd = new SqlCommand(deleteFornecedorQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ID", fornecedorID);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Fornecedor e contratos excluídos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    transaction.Rollback();
                    throw; // Repassa a exceção para ser tratada no nível superior
                }
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var valorID = dataGridView1.CurrentRow.Cells["ID"].Value;

                if (valorID != DBNull.Value && int.TryParse(valorID.ToString(), out int fornecedorID))
                {
                    // Abrir o formulário de edição com os dados do fornecedor
                    using (var formEdicao = new EditSupplierForm(fornecedorID))
                    {
                        if (formEdicao.ShowDialog() == DialogResult.OK)
                        {
                            CarregarFornecedores(); // Recarrega a lista após edição
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um fornecedor válido para editar.");
                }
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            // Verifica se há uma linha selecionada no DataGridView
            if (dataGridView1.CurrentRow != null)
            {
                var valorID = dataGridView1.CurrentRow.Cells["ID"].Value;

                if (valorID != DBNull.Value && int.TryParse(valorID.ToString(), out int fornecedorID))
                {
                    // Cria e mostra o formulário de novo contrato, passando o ID do fornecedor
                    using (var formNovoContrato = new CreateContractForm(fornecedorID))
                    {
                        if (formNovoContrato.ShowDialog() == DialogResult.OK)
                        {
                            CarregarContratos(fornecedorID); // Atualiza os contratos após adicionar
                        }
                    }
                }
            }
            else
            {
                // Mensagem se nenhum fornecedor estiver selecionado
                MessageBox.Show("Selecione um fornecedor para adicionar um contrato.");
            }
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            // Verifica se há uma linha selecionada no dgvContratos
            if (dataGridView2.CurrentRow != null)
            {
                var valorID = dataGridView2.CurrentRow.Cells["ID"].Value;

                if (valorID != DBNull.Value && int.TryParse(valorID.ToString(), out int contratoID))
                {
                    // Confirmação antes de excluir o contrato
                    var confirmResult = MessageBox.Show(
                        "Deseja realmente excluir este contrato?",
                        "Confirmar Exclusão",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (confirmResult == DialogResult.Yes)
                    {
                        try
                        {
                            DeletarContrato(contratoID);

                            // Atualiza os contratos vinculados ao fornecedor selecionado
                            var fornecedorID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                            CarregarContratos(fornecedorID);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao excluir o contrato: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um contrato para deletar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeletarContrato(int contratoID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM CONTRACTSDETAIL WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", contratoID);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Contrato excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
