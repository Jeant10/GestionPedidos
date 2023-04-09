using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConexionGestionPedidos2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings[
                "ConexionGestionPedidos2.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);

            MuestraClientes();

            MuestraTodosPedidos();
        }

        private void MuestraClientes()
        {
            try
            {
                string consulta = "SELECT * FROM CLIENTE";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSql)
                {
                    DataTable clientesTabla = new DataTable();

                    miAdaptadorSql.Fill(clientesTabla);

                    ListaClientes.DisplayMemberPath = "nombre";
                    ListaClientes.SelectedValuePath = "Id";
                    ListaClientes.ItemsSource = clientesTabla.DefaultView;

                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MuestraPedidos()
        {
            try
            {
                string consulta = "SELECT * FROM Pedido P INNER JOIN Cliente C" +
               " ON C.Id = P.cCliente WHERE C.Id = @ClienteId";

                SqlCommand sqlComando = new SqlCommand(consulta, miConexionSql);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(sqlComando);

                using (miAdaptadorSql)
                {
                    sqlComando.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);

                    DataTable pedidosTabla = new DataTable();

                    miAdaptadorSql.Fill(pedidosTabla);

                    PedidosClientes.DisplayMemberPath = "fechaPedido";
                    PedidosClientes.SelectedValuePath = "Id";
                    PedidosClientes.ItemsSource = pedidosTabla.DefaultView;

                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void MuestraTodosPedidos()
        {
            try
            {
                string consulta = "SELECT *, CONCAT(cCliente, ' ', fechaPedido, ' ', formaPago) AS INFOCOMPLETA FROM PEDIDO";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdaptadorSql)
                {

                    DataTable pedidosTabla = new DataTable();

                    miAdaptadorSql.Fill(pedidosTabla);

                    TodosPedidos.DisplayMemberPath = "INFOCOMPLETA";
                    TodosPedidos.SelectedValuePath = "Id";
                    TodosPedidos.ItemsSource = pedidosTabla.DefaultView;

                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        SqlConnection miConexionSql;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(TodosPedidos.SelectedValue.ToString());

            string consulta = "DELETE FROM Pedido WHERE Id = @PedidoId";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();
            
            sqlCommand.Parameters.AddWithValue("@PedidoId", TodosPedidos.SelectedValue);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();
        
            MuestraTodosPedidos();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string consulta = "INSERT INTO Cliente (nombre) VALUES (@nombre)";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlCommand.Parameters.AddWithValue("@nombre", insertaCliente.Text);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraClientes();

            insertaCliente.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string consulta = "DELETE FROM Cliente WHERE Id=@ClienteId";

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlCommand.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            MuestraClientes();
        }

        private void ListaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Actualiza ventanaActualizar = new Actualiza((int)ListaClientes.SelectedValue);

            ventanaActualizar.Show();

            try
            {
                string consulta = "SELECT nombre FROM Cliente WHERE Id=@ClId";

                SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSql);
                
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(miSqlCommand);

                using (miAdaptadorSql)
                {
                    miSqlCommand.Parameters.AddWithValue("@ClId", ListaClientes.SelectedValue);
                    
                    DataTable clientesTabla = new DataTable();

                    miAdaptadorSql.Fill(clientesTabla);

                    ventanaActualizar.cuadroActualiza.Text = clientesTabla.Rows[0]["nombre"].ToString();
               
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MuestraClientes();
        }
    }
}
