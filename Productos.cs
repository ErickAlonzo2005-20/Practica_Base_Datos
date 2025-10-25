using actividad3;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EjercicioBaseDeDatosWinForm
{
    public partial class Productos : Form
    {
        private const string connectionString = @"Data Source=DESKTOP-H0RUUE2;Initial Catalog=Practica3;Integrated Security=True;";

        public Productos()
        {
            InitializeComponent();
        }


        private void cargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                  
                    string query = @"
                        SELECT 
                            p.ProductoID, 
                            p.NombreProducto, 
                            p.Descripcion,
                            p.Precio,
                            p.Stock,
                            p.CategoriaID, -- <--- AÑADIDO AQUÍ
                            c.NombreCategoria 
                        FROM 
                            Productos p
                        INNER JOIN 
                            Categorias c ON p.CategoriaID = c.CategoriaID
                        ORDER BY p.ProductoID;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgProductos.DataSource = dt;

                       
                            if (dgProductos.Columns.Contains("CategoriaID"))
                            {
                                dgProductos.Columns["CategoriaID"].HeaderText = "CategoriaID";
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                 
                    string errorMessage = "Error al cargar productos: " + ex.Message + "\n\nAsegúrese de que el servidor SQL y la base de datos están disponibles.";
                    MessageBox.Show(errorMessage, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            this.cargarDatos();
        }

     

        private void btnCargarProductos_Click_2(object sender, EventArgs e)
        {
            this.cargarDatos();
        }

        private void btnAgregarProducto_Click_1(object sender, EventArgs e)
        {
            // Validación con 5 campos de entrada
            if (string.IsNullOrWhiteSpace(txtNombreProducto.Text) ||
                string.IsNullOrWhiteSpace(txtDescripcion.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) ||
                string.IsNullOrWhiteSpace(txtCategoriaID.Text))
            {
                MessageBox.Show("Todos los campos (Nombre, Descripción, Precio, Stock, Categoría ID) son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) ||
                !int.TryParse(txtStock.Text, out int stock) ||
                !int.TryParse(txtCategoriaID.Text, out int categoriaID))
            {
                MessageBox.Show("Precio, Stock y Categoría ID deben ser números válidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Inserción con 5 campos
            string query = "INSERT INTO Productos (NombreProducto, Descripcion, Precio, Stock, CategoriaID) VALUES (@Nombre, @Descripcion, @Precio, @Stock, @CategoriaID)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreProducto.Text);
                    cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@CategoriaID", categoriaID);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Producto insertado correctamente ✔️");
                        else
                            MessageBox.Show("No se pudo insertar el producto (posiblemente la CategoriaID no existe).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar producto: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtNombreProducto.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtCategoriaID.Clear();
        }

        private void btnEliminarProducto_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDProducto.Text) || !int.TryParse(txtIDProducto.Text, out int productoID))
            {
                MessageBox.Show("Debe introducir un ID de producto válido para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "DELETE FROM Productos WHERE ProductoID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", productoID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Producto eliminado correctamente 🗑️");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un producto con ese ID.", "Información");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar producto: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtIDProducto.Clear();
        }

        private void btnActualizarProducto_Click_1(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtIDProductoActualizar.Text) || !int.TryParse(txtIDProductoActualizar.Text, out int productoID))
            {
                MessageBox.Show("Debe introducir un ID válido para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreProductoActualizado.Text) &&
                string.IsNullOrWhiteSpace(txtDescripcionActualizada.Text) &&
                string.IsNullOrWhiteSpace(txtPrecioActualizado.Text) &&
                string.IsNullOrWhiteSpace(txtStockActualizado.Text) &&
                string.IsNullOrWhiteSpace(txtCategoriaIDActualizado.Text))
            {
                MessageBox.Show("Debe introducir al menos un campo para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Productos SET ";
            bool firstField = true;

            if (!string.IsNullOrWhiteSpace(txtNombreProductoActualizado.Text))
            {
                query += "NombreProducto = @Nombre";
                firstField = false;
            }

            if (!string.IsNullOrWhiteSpace(txtDescripcionActualizada.Text))
            {
                query += (firstField ? "" : ", ") + "Descripcion = @Descripcion";
                firstField = false;
            }

            decimal precio = 0;
            if (!string.IsNullOrWhiteSpace(txtPrecioActualizado.Text))
            {
                if (!decimal.TryParse(txtPrecioActualizado.Text, out precio))
                {
                    MessageBox.Show("El Precio debe ser un número válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                query += (firstField ? "" : ", ") + "Precio = @Precio";
                firstField = false;
            }

            int stock = 0;
            if (!string.IsNullOrWhiteSpace(txtStockActualizado.Text))
            {
                if (!int.TryParse(txtStockActualizado.Text, out stock))
                {
                    MessageBox.Show("El Stock debe ser un número entero válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                query += (firstField ? "" : ", ") + "Stock = @Stock";
                firstField = false;
            }

            int categoriaID = 0;
            if (!string.IsNullOrWhiteSpace(txtCategoriaIDActualizado.Text))
            {
                if (!int.TryParse(txtCategoriaIDActualizado.Text, out categoriaID))
                {
                    MessageBox.Show("La Categoría ID debe ser un número entero válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                query += (firstField ? "" : ", ") + "CategoriaID = @CategoriaID";
            }

            query += " WHERE ProductoID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", productoID);

                    if (!string.IsNullOrWhiteSpace(txtNombreProductoActualizado.Text))
                        cmd.Parameters.AddWithValue("@Nombre", txtNombreProductoActualizado.Text);

                    if (!string.IsNullOrWhiteSpace(txtDescripcionActualizada.Text))
                        cmd.Parameters.AddWithValue("@Descripcion", txtDescripcionActualizada.Text);

                    if (!string.IsNullOrWhiteSpace(txtPrecioActualizado.Text))
                        cmd.Parameters.AddWithValue("@Precio", precio);

                    if (!string.IsNullOrWhiteSpace(txtStockActualizado.Text))
                        cmd.Parameters.AddWithValue("@Stock", stock);

                    if (!string.IsNullOrWhiteSpace(txtCategoriaIDActualizado.Text))
                        cmd.Parameters.AddWithValue("@CategoriaID", categoriaID);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Producto actualizado correctamente 🔄");
                        else
                            MessageBox.Show("No se encontró un producto con ese ID o la Categoría ID no existe.", "Información");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al actualizar producto: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtIDProductoActualizar.Clear();
            txtNombreProductoActualizado.Clear();
            txtDescripcionActualizada.Clear();
            txtPrecioActualizado.Clear();
            txtStockActualizado.Clear();
            txtCategoriaIDActualizado.Clear();
        }

        private void dgProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}