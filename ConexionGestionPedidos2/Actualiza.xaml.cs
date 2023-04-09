using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using System.Data;
using System.Configuration;

namespace ConexionGestionPedidos2
{
    /// <summary>
    /// Lógica de interacción para Actualiza.xaml
    /// </summary>
    public partial class Actualiza : Window
    {

        private int z; 
        public Actualiza(int elId)
        {
            InitializeComponent();

            z = elId;

            string miConexion = ConfigurationManager.ConnectionStrings[
                "ConexionGestionPedidos2.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConexionSql = new SqlConnection(miConexion);
        }


        SqlConnection miConexionSql;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string consulta = "UPDATE Cliente SET nombre = @nombre WHERE Id = " + z;

            SqlCommand sqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();

            sqlCommand.Parameters.AddWithValue("@nombre", cuadroActualiza.Text);

            sqlCommand.ExecuteNonQuery();

            miConexionSql.Close();

            this.Close();
        }
    }
}
