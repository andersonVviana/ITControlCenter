using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            LoadDataCountIps();
            LoadDataAvailable();
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
                        HIH.WarrantyDateFrom,
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
                    WarrantyDateFrom,
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

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        private void LoadDataCountIps()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT Name, ipv4Address 
                    FROM CONSOLIDATED_HARDWARE_INV 
                    WHERE 
                        (Status LIKE '%Ativo%' OR 
                         Status LIKE '%Atestado / Licença%' OR 
                         Status LIKE '%Disponível no TI%' OR 
                         Status LIKE '%Em Análise%' OR 
                         Status LIKE '%Na Assistência Técnica%') 
                    AND 
                        (Name LIKE '%ABTC%' OR 
                         Name LIKE '%ABNB%' OR 
                         Name LIKE '%ABWS%'); ";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Filtrar por planta (DMA e PLC)
                    var dma = dataTable.AsEnumerable().Where(row =>
                        row["ipv4Address"].ToString().StartsWith("192.168.0") ||
                        row["ipv4Address"].ToString().StartsWith("192.168.1") ||
                        row["ipv4Address"].ToString().StartsWith("192.168.20") ||
                        row["ipv4Address"].ToString().StartsWith("192.168.21"));

                    var plc = dataTable.AsEnumerable().Where(row =>
                        row["ipv4Address"].ToString().StartsWith("192.168.3"));

                    // Totais por planta
                    int totalDMA = dma.Count();
                    int totalPLC = plc.Count();

                    // Contagens por tipo para DMA
                    int laptopsDMA = dma.Count(row => row["Name"].ToString().StartsWith("ABNB"));
                    int desktopsDMA = dma.Count(row => row["Name"].ToString().StartsWith("ABWS"));
                    int thinClientsDMA = dma.Count(row => row["Name"].ToString().StartsWith("ABTC"));

                    // Contagens por tipo para PLC
                    int laptopsPLC = plc.Count(row => row["Name"].ToString().StartsWith("ABNB"));
                    int desktopsPLC = plc.Count(row => row["Name"].ToString().StartsWith("ABWS"));
                    int thinClientsPLC = plc.Count(row => row["Name"].ToString().StartsWith("ABTC"));

                    // Exibe as contagens nos TextBoxes
                    textBox2.Text = totalDMA.ToString();
                    textBox3.Text = totalPLC.ToString();

                    textBox4.Text = laptopsDMA.ToString();
                    textBox6.Text = desktopsDMA.ToString();
                    textBox8.Text = thinClientsDMA.ToString();

                    textBox5.Text = laptopsPLC.ToString();
                    textBox7.Text = desktopsPLC.ToString();
                    textBox9.Text = thinClientsPLC.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
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
                                HIH.WarrantyDateFrom,
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
                            WarrantyDateFrom,
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
                                    HIH.WarrantyDateFrom,
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
                                WarrantyDateFrom,
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

        private void LoadDataAvailable()
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
                            ROW_NUMBER() OVER (PARTITION BY Name, Serial ORDER BY TraceDateTime DESC) AS RowNum
                        FROM CONSOLIDATED_HARDWARE_INV
                        WHERE (Name IS NOT NULL AND Name <> '') AND Status = 'Disponível no TI'
                    )
                    SELECT 
                        Name,
						Manufacturer,
                        Model
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
                        dataGridView2.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
                }
            }
        }

        private void materialButton3_Click_1(object sender, EventArgs e)
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

        private void materialButton2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void materialButton1_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Obter a linha selecionada no DataGridView
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;

                // Obter os valores de Name e Serial da linha selecionada
                string currentStatus = selectedRow.Cells["Status"].Value.ToString();
                string manufacturer = selectedRow.Cells["Manufacturer"].Value.ToString();
                string model = selectedRow.Cells["Model"].Value.ToString();
                string serial = selectedRow.Cells["Serial"].Value.ToString();

                string name = selectedRow.Cells["Name"].Value.ToString();

                // Instanciar o formulário UpdateStatusForm
                UpdateStatusForm updateForm = new UpdateStatusForm
                {
                    CurrentStatus = currentStatus,
                    Manufacturer = manufacturer,
                    Model = model,
                    Serial = serial,
                    Name = name
                };

                // Exibir o formulário
                updateForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione uma linha antes de continuar.");
            }
        }

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Chama o método para filtrar os dados no DataGridView
                FilterData(textBox1.Text);
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            CreateHIForm createHIForm = new CreateHIForm();

            createHIForm.ShowDialog();
        }


        private void materialButton6_Click(object sender, EventArgs e)
{
    if (dataGridView1.CurrentRow != null)
    {
        // Obter a linha selecionada no DataGridView
        DataGridViewRow selectedRow = dataGridView1.CurrentRow;

        // Verifica se as células contêm valores válidos para datas antes de converter
        if (DateTime.TryParse(selectedRow.Cells["WarrantyDateFrom"].Value?.ToString(), out DateTime warrantyDateFrom) &&
            DateTime.TryParse(selectedRow.Cells["WarrantyDateTo"].Value?.ToString(), out DateTime warrantyDateTo))
        {
            // Instancia o formulário WarrantyForm e define os valores dos DateTimePickers
            WarrantyForm updateForm = new WarrantyForm();
            updateForm.dateTimePicker1.Value = warrantyDateFrom;
            updateForm.dateTimePicker2.Value = warrantyDateTo;

            // Exibir o formulário
            updateForm.ShowDialog();
        }
        else
        {
            MessageBox.Show("As datas selecionadas são inválidas.");
        }
    }
    else
    {
        MessageBox.Show("Selecione uma linha antes de continuar.");
    }
}


    }

}
