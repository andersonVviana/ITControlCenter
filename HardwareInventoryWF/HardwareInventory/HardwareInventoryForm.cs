using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardwareInventoryWF.HardwareInventory
{
    public partial class HardwareInventoryForm : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        string connectionString = "Server=ARCADE-DBDW01;Database=SQL_ARC_HARDWARE_INVENTORY;User Id=gateway;Password=2rD&2Bn?F4;";

        public HardwareInventoryForm()
        {
            InitializeComponent();
			LoadData();
            LoadDataAT();
        }

        private void HardwareInventoryForm_MouseDown(object sender, MouseEventArgs e)
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
                WITH LatestHardware AS (
                    SELECT 
                        HIH.Name,
                        HIH.Status,
                        HIH.OperatingSystem,
                        HIH.ServicePack,
                        HIH.TraceDateTime,
                        HIH.UserLogged,
                        HIH.DisplayName,
                        HIH.ipv4Address,
                        HIH.Department,
                        HIH.Description,
                        HIH.Office,
                        HIH.Manufacturer,
                        HIH.Model,
                        HIH.Serial,
                        HIH.NumberOfCores,
                        HIH.ProcessorManufacturer,
                        HIH.ProcessorName,
                        HIH.RAM,
                        HIH.DiskDrive,
                        HIH.Size,
                        HIH.Graphics,
                        HIH.GraphicsRAM,
                        HIH.Processed,
                        HIH.WarrantyDateTo,
                        ROW_NUMBER() OVER (PARTITION BY HIH.Name ORDER BY HIH.TraceDateTime DESC) AS RowNum
                    FROM CONSOLIDATED_HARDWARE_INV HIH
                    WHERE (Name IS NOT NULL AND Name <> '')
                )
                SELECT 
                    Name,
                    Status,
                    OperatingSystem,
                    ServicePack,
                    TraceDateTime,
                    UserLogged,
                    DisplayName,
                    ipv4Address,
                    Department,
                    Description,
                    Office,
                    Manufacturer,
                    Model,
                    Serial,
                    NumberOfCores,
                    ProcessorManufacturer,
                    ProcessorName,
                    RAM,
                    DiskDrive,
                    Size,
                    Graphics,
                    GraphicsRAM,
                    Processed,
                    WarrantyDateTo
                FROM LatestHardware
                WHERE RowNum = 1
                ORDER BY TraceDateTime DESC;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Define a fonte de dados do DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Chama o novo método para contar os IPs com base no status
                        ContarIPs(dataTable);

                        ContarTiposPorFaixaIP(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        private void ContarIPs(DataTable dataTable)
        {
            // Inicializar contadores
            int totalIPs_0_20_21 = 0, totalIPs_3 = 0;

            // Lista de status permitidos
            var statusPermitidos = new List<string>
            {
                "Ativo", "Disponível no TI", "Na Assistência Técnica",
                "Atestado / Licença", "Em Análise"
            };

            foreach (DataRow row in dataTable.Rows)
            {
                string status = row["Status"]?.ToString().Trim();
                string ip = row["ipv4Address"]?.ToString().Trim();

                // Verificar se o status é permitido
                if (!statusPermitidos.Contains(status)) continue;

                // Verificar faixa de IP
                if (ip.StartsWith("192.168.0.") || ip.StartsWith("192.168.1.") || ip.StartsWith("192.168.21.") || ip.StartsWith("192.168.20."))
                    totalIPs_0_20_21++;
                else if (ip.StartsWith("192.168.3."))
                    totalIPs_3++;
            }

            // Atualizar as TextBoxes com os resultados
            textBox2.Text = totalIPs_0_20_21.ToString();
            textBox3.Text = totalIPs_3.ToString();
        }


        private void ContarTiposPorFaixaIP(DataTable dataTable)
        {
            // Inicializar contadores
            int laptops_0_20_21 = 0, laptops_3 = 0;
            int desktops_0_20_21 = 0, desktops_3 = 0;
            int thinClients_0_20_21 = 0, thinClients_3 = 0;

            // Lista de status permitidos
            var statusPermitidos = new List<string>
            {
                "Ativo", "Disponível no TI", "Na Assistência Técnica",
                "Atestado / Licença", "Em Análise"
            };

            foreach (DataRow row in dataTable.Rows)
            {
                string status = row["Status"]?.ToString().Trim();
                string name = row["Name"]?.ToString().Trim();
                string ip = row["ipv4Address"]?.ToString().Trim();

                // Verificar se o status é permitido
                if (!statusPermitidos.Contains(status)) continue;

                // Verificar faixa de IP e tipo de dispositivo
                if (ip.StartsWith("192.168.0.") || ip.StartsWith("192.168.1.") || ip.StartsWith("192.168.21.") || ip.StartsWith("192.168.20."))
                {
                    if (name.StartsWith("ABNB", StringComparison.InvariantCultureIgnoreCase))
                        laptops_0_20_21++;
                    else if (name.StartsWith("ABWS", StringComparison.InvariantCultureIgnoreCase))
                        desktops_0_20_21++;
                    else if (name.StartsWith("ABTC", StringComparison.InvariantCultureIgnoreCase))
                        thinClients_0_20_21++;
                }
                else if (ip.StartsWith("192.168.3."))
                {
                    if (name.StartsWith("ABNB", StringComparison.InvariantCultureIgnoreCase))
                        laptops_3++;
                    else if (name.StartsWith("ABWS", StringComparison.InvariantCultureIgnoreCase))
                        desktops_3++;
                    else if (name.StartsWith("ABTC", StringComparison.InvariantCultureIgnoreCase))
                        thinClients_3++;
                }
            }

            // Exibir resultados para depuração
            Console.WriteLine($"Laptops (192.168.0/20/21): {laptops_0_20_21}");
            Console.WriteLine($"Laptops (192.168.3): {laptops_3}");
            Console.WriteLine($"Desktops (192.168.0/20/21): {desktops_0_20_21}");
            Console.WriteLine($"Desktops (192.168.3): {desktops_3}");
            Console.WriteLine($"ThinClients (192.168.0/20/21): {thinClients_0_20_21}");
            Console.WriteLine($"ThinClients (192.168.3): {thinClients_3}");

            // Atualizar as TextBoxes com os resultados
            textBox4.Text = laptops_0_20_21.ToString();
            textBox5.Text = laptops_3.ToString();
            textBox6.Text = desktops_0_20_21.ToString();
            textBox7.Text = desktops_3.ToString();
            textBox8.Text = thinClients_0_20_21.ToString();
            textBox9.Text = thinClients_3.ToString();
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
                          WITH LatestHardware AS (
                            SELECT 
                                HIH.Name,
                                HIH.Status,
                                HIH.OperatingSystem,
                                HIH.ServicePack,
                                HIH.TraceDateTime,
                                HIH.UserLogged,
                                HIH.DisplayName,
                                HIH.ipv4Address,
                                HIH.Department,
                                HIH.Description,
                                HIH.Office,
                                HIH.Manufacturer,
                                HIH.Model,
                                HIH.Serial,
                                HIH.NumberOfCores,
                                HIH.ProcessorManufacturer,
                                HIH.ProcessorName,
                                HIH.RAM,
                                HIH.DiskDrive,
                                HIH.Size,
                                HIH.Graphics,
                                HIH.GraphicsRAM,
                                HIH.Processed,
                                HIH.WarrantyDateTo,
                                ROW_NUMBER() OVER (PARTITION BY HIH.Name ORDER BY HIH.TraceDateTime DESC) AS RowNum
                            FROM CONSOLIDATED_HARDWARE_INV HIH
                            WHERE (Name IS NOT NULL AND Name <> '')
                        )
                        SELECT 
                            Name,
                            Status,
                            OperatingSystem,
                            ServicePack,
                            TraceDateTime,
                            UserLogged,
                            DisplayName,
                            ipv4Address,
                            Department,
                            Description,
                            Office,
                            Manufacturer,
                            Model,
                            Serial,
                            NumberOfCores,
                            ProcessorManufacturer,
                            ProcessorName,
                            RAM,
                            DiskDrive,
                            Size,
                            Graphics,
                            GraphicsRAM,
                            Processed,
                            WarrantyDateTo
                        FROM LatestHardware
                        WHERE RowNum = 1
                        ORDER BY TraceDateTime DESC;
                    ";
                    }
                    else
                    {
                        query = @"
                            WITH LatestHardware AS (
                                SELECT 
                                    HIH.Name,
                                    HIH.OperatingSystem,
		                            HIH.Status,
                                    HIH.ServicePack,
                                    HIH.TraceDateTime,
                                    HIH.UserLogged,
                                    HIH.DisplayName,
                                    HIH.ipv4Address,
                                    HIH.Department,
                                    HIH.Description,
                                    HIH.Office,
                                    HIH.Manufacturer,
                                    HIH.Model,
                                    HIH.Serial,
                                    HIH.NumberOfCores,
                                    HIH.ProcessorManufacturer,
                                    HIH.ProcessorName,
                                    HIH.RAM,
                                    HIH.DiskDrive,
                                    HIH.Size,
                                    HIH.Graphics,
                                    HIH.GraphicsRAM,
                                    HIH.Processed,
                                    HIH.WarrantyDateTo,
                                    ROW_NUMBER() OVER (PARTITION BY HIH.Name ORDER BY HIH.TraceDateTime DESC) AS RowNum
                                FROM CONSOLIDATED_HARDWARE_INV HIH
                                WHERE (HIH.Name IS NOT NULL AND HIH.Name <> '')
                            )
                            SELECT 
                                Name,
                                OperatingSystem,
	                            Status,
                                ServicePack,
                                TraceDateTime,
                                UserLogged,
                                DisplayName,
                                ipv4Address,
                                Department,
                                Description,
                                Office,
                                Manufacturer,
                                Model,
                                Serial,
                                NumberOfCores,
                                ProcessorManufacturer,
                                ProcessorName,
                                RAM,
                                DiskDrive,
                                Size,
                                Graphics,
                                GraphicsRAM,
                                Processed,
                                WarrantyDateTo
                            FROM LatestHardware
                            WHERE RowNum = 1 AND 
                        (Name LIKE @searchTerm OR UserLogged LIKE @searchTerm OR Serial LIKE @searchTerm)
                        ORDER BY TraceDateTime DESC";
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adiciona o parâmetro de pesquisa
                        command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Define a fonte de dados do DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        private void HardwareInventoryForm_Load(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Obter a linha selecionada no DataGridView
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;

                // Obter os valores de Name e Serial da linha selecionada
                string assetName = selectedRow.Cells["Name"].Value.ToString();
                string serial = selectedRow.Cells["Serial"].Value.ToString();
                string currentStatus = selectedRow.Cells["Status"].Value.ToString();

                // Instanciar o formulário UpdateStatusForm
                UpdateStatusForm updateForm = new UpdateStatusForm
                {
                    SelectedAssetName = assetName,
                    SelectedSerial = serial,
                    CurrentStatus = currentStatus
                };

                // Exibir o formulário
                updateForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione uma linha antes de continuar.");
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Excel Button
        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                // Cria um DataTable para armazenar os dados do DataGridView
                DataTable dt = new DataTable();

                // Adiciona as colunas ao DataTable
                foreach (DataGridViewColumn coluna in dataGridView1.Columns)
                {
                    dt.Columns.Add(coluna.HeaderText);
                }

                // Adiciona as linhas ao DataTable
                foreach (DataGridViewRow linha in dataGridView1.Rows)
                {
                    if (!linha.IsNewRow)
                    {
                        DataRow row = dt.NewRow();
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            row[i] = linha.Cells[i].Value ?? "";
                        }
                        dt.Rows.Add(row);
                    }
                }

                // Cria um workbook do ClosedXML e adiciona o DataTable
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    // Cria um arquivo temporário para a planilha
                    string tempFile = Path.Combine(Path.GetTempPath(), "Relatorio.xlsx");

                    workbook.Worksheets.Add(dt, "Relatorio");
                    workbook.SaveAs(tempFile);

                    // Abre o Excel com a planilha gerada
                    Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataAT()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Substitua sua consulta SQL abaixo
                    string query = @"
                    WITH LatestInventory AS
                    (
                        SELECT 
                            Name,
							Manufacturer,
                            Model,
							UserLogged,
                            ROW_NUMBER() OVER (PARTITION BY Name, Serial ORDER BY TraceDateTime DESC) AS RowNum
                        FROM CONSOLIDATED_HARDWARE_INV
                        WHERE (Name IS NOT NULL AND Name <> '') AND Status = 'Na Assistência Técnica'
                    )
                    SELECT 
                        Name,
						Manufacturer,
                        Model,
						UserLogged
                    FROM LatestInventory
                    WHERE RowNum = 1
                    ORDER BY Name DESC
					";  // Insira sua consulta SQL aqui

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Define a fonte de dados do DataGridView
                        dataGridView3.DataSource = dataTable;
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
