using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HardwareInventoryWF
{
    public partial class Teste : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private string apiUrl = "http://10.2.0.107/zabbix//api_jsonrpc.php";
        private string authToken;

        public Teste()
        {
            InitializeComponent();
            LoadHosts();
        }

        public class ZabbixApi
        {
            private static readonly HttpClient client = new HttpClient();
            private string apiUrl = "http://10.2.0.107/zabbix/api_jsonrpc.php";
            private string authToken;

            public async Task Authenticate(string username, string password)
            {
                var authData = new
                {
                    jsonrpc = "2.0",
                    method = "user.login",
                    @params = new
                    {
                        username = username, // Corrigido para "username"
                        password = password
                    },
                    id = 1
                };

                var content = new StringContent(JObject.FromObject(authData).ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);

                if (jsonResponse["result"] != null)
                {
                    authToken = jsonResponse["result"].ToString();
                }
                else if (jsonResponse["error"] != null)
                {
                    throw new Exception("Erro de autenticação: " + jsonResponse["error"]["data"]);
                }
                else
                {
                    throw new Exception("Resposta inesperada da API.");
                }
            }


            public async Task<JArray> GetHosts()
            {
                var requestData = new
                {
                    jsonrpc = "2.0",
                    method = "host.get",
                    @params = new
                    {
                        output = new[] { "hostid", "name" }
                    },
                    auth = authToken,
                    id = 1
                };

                var content = new StringContent(JObject.FromObject(requestData).ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);

                if (jsonResponse["result"] != null)
                {
                    MessageBox.Show("Resposta da API: " + jsonResponse["result"].ToString());
                    return (JArray)jsonResponse["result"];
                }
                else if (jsonResponse["error"] != null)
                {
                    throw new Exception("Erro ao obter os hosts: " + jsonResponse["error"]["data"]);
                }
                else
                {
                    throw new Exception("Resposta inesperada da API.");
                }
            }



        }

        public async Task Authenticate(string user, string password)
        {
            var authData = new
            {
                jsonrpc = "2.0",
                method = "user.login",
                @params = new
                {
                    user = user,
                    password = password
                },
                id = 1
            };

            var content = new StringContent(JObject.FromObject(authData).ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);

            authToken = jsonResponse["result"].ToString();
        }

        private async void LoadHosts()
        {
            try
            {
                var zabbixApi = new ZabbixApi();
                await zabbixApi.Authenticate("Admin", "2rD&2Bn?F4");
                MessageBox.Show("Autenticação bem-sucedida!");

                var hosts = await zabbixApi.GetHosts();
                MessageBox.Show("Hosts recebidos: " + hosts.Count);

                listView1.View = View.Details;
                listView1.Columns.Add("HostID", 100, HorizontalAlignment.Left);
                listView1.Columns.Add("Name", 200, HorizontalAlignment.Left);

                foreach (var host in hosts)
                {
                    if (host["hostid"] != null && host["name"] != null)
                    {
                        ListViewItem item = new ListViewItem(host["hostid"].ToString());
                        item.SubItems.Add(host["name"].ToString());
                        listView1.Items.Add(item);
                    }
                }

                MessageBox.Show("Dados carregados na ListView!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }



    }
}
