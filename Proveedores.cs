using actividad3;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EjercicioBaseDeDatosWinForm
{
    public partial class Proveedores : Form
    {
        // 🔗 Conexión a la base de datos
        private const string connectionString = @"Data Source=DESKTOP-H0RUUE2;Initial Catalog=Practica3;Integrated Security=True;";

        public Proveedores()
        {
            InitializeComponent();
        }

        // ----------------------------------------------------------------------
        // CARGAR DATOS DE PROVEEDORES
        // ----------------------------------------------------------------------
        private void CargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT ProveedorID, NombreProveedor, Telefono, CorreoElectronico FROM Proveedores;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgProveedores.DataSource = dt;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al cargar proveedores: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        // ----------------------------------------------------------------------
        // INSERTAR PROVEEDOR
        // ----------------------------------------------------------------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textNombreProveedor.Text) ||
                string.IsNullOrWhiteSpace(textTelefono.Text) ||
                string.IsNullOrWhiteSpace(textCorreoElectronico.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryInsertar = @"INSERT INTO Proveedores (NombreProveedor, Telefono, CorreoElectronico)
                                     VALUES (@Nombre, @Telefono, @Correo);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryInsertar, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", textNombreProveedor.Text);
                    cmd.Parameters.AddWithValue("@Telefono", textTelefono.Text);
                    cmd.Parameters.AddWithValue("@Correo", textCorreoElectronico.Text);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Proveedor agregado correctamente. ✅");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar proveedor: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            CargarDatos();
        }

        // ----------------------------------------------------------------------
        // ACTUALIZAR PROVEEDOR
        // ----------------------------------------------------------------------
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProveedorIDActualizado.Text) ||
                !int.TryParse(txtProveedorIDActualizado.Text, out int proveedorID))
            {
                MessageBox.Show("Debe introducir un ID válido de proveedor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProveedorActualizado.Text) ||
                string.IsNullOrWhiteSpace(textTelefonoActualizado.Text) ||
                string.IsNullOrWhiteSpace(textCorreoElectronicoActualizado.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryActualizar = @"UPDATE Proveedores
                                       SET NombreProveedor = @Nombre,
                                           Telefono = @Telefono,
                                           CorreoElectronico = @Correo
                                       WHERE ProveedorID = @ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryActualizar, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtProveedorActualizado.Text);
                    cmd.Parameters.AddWithValue("@Telefono", textTelefonoActualizado.Text);
                    cmd.Parameters.AddWithValue("@Correo", textCorreoElectronicoActualizado.Text);
                    cmd.Parameters.AddWithValue("@ID", proveedorID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Proveedor actualizado correctamente. 🔄");
                        else
                            MessageBox.Show("No se encontró el proveedor con ese ID.", "Información");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al actualizar proveedor: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            CargarDatos();
        }

        // ----------------------------------------------------------------------
        // ELIMINAR PROVEEDOR
        // ----------------------------------------------------------------------
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || !int.TryParse(txtID.Text, out int proveedorID))
            {
                MessageBox.Show("Debe introducir un ID válido de proveedor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryEliminar = @"DELETE FROM Proveedores WHERE ProveedorID = @ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryEliminar, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", proveedorID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Proveedor eliminado correctamente. 🗑️");
                        else
                            MessageBox.Show("No se encontró un proveedor con ese ID.", "Información");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar proveedor: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            CargarDatos();
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textNombreProveedor.Text) ||
               string.IsNullOrWhiteSpace(textTelefono.Text) ||
               string.IsNullOrWhiteSpace(textCorreoElectronico.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryInsertar = @"INSERT INTO Proveedores (NombreProveedor, Telefono, CorreoElectronico)
                                     VALUES (@Nombre, @Telefono, @Correo);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryInsertar, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", textNombreProveedor.Text);
                    cmd.Parameters.AddWithValue("@Telefono", textTelefono.Text);
                    cmd.Parameters.AddWithValue("@Correo", textCorreoElectronico.Text);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Proveedor agregado correctamente. ✅");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar proveedor: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            CargarDatos();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            CargarDatos();

        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || !int.TryParse(txtID.Text, out int proveedorID))
            {
                MessageBox.Show("Debe introducir un ID válido de proveedor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryEliminar = @"DELETE FROM Proveedores WHERE ProveedorID = @ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryEliminar, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", proveedorID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Proveedor eliminado correctamente. 🗑️");
                        else
                            MessageBox.Show("No se encontró un proveedor con ese ID.", "Información");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar proveedor: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            CargarDatos();
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProveedorIDActualizado.Text) ||
               !int.TryParse(txtProveedorIDActualizado.Text, out int proveedorID))
            {
                MessageBox.Show("Debe introducir un ID válido de proveedor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProveedorActualizado.Text) ||
                string.IsNullOrWhiteSpace(textTelefonoActualizado.Text) ||
                string.IsNullOrWhiteSpace(textCorreoElectronicoActualizado.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryActualizar = @"UPDATE Proveedores
                                       SET NombreProveedor = @Nombre,
                                           Telefono = @Telefono,
                                           CorreoElectronico = @Correo
                                       WHERE ProveedorID = @ID;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryActualizar, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtProveedorActualizado.Text);
                    cmd.Parameters.AddWithValue("@Telefono", textTelefonoActualizado.Text);
                    cmd.Parameters.AddWithValue("@Correo", textCorreoElectronicoActualizado.Text);
                    cmd.Parameters.AddWithValue("@ID", proveedorID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Proveedor actualizado correctamente. 🔄");
                        else
                            MessageBox.Show("No se encontró el proveedor con ese ID.", "Información");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al actualizar proveedor: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            CargarDatos();
        }
    }
}
