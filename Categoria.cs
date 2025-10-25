﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EjercicioBaseDeDatosWinForm
{
    public partial class Categorias : Form
    {
        private const string connectionString = @"Data Source=DESKTOP-H0RUUE2;Initial Catalog=Practica3;Integrated Security=True;";

        public Categorias()
        {
            InitializeComponent();
        }

        // ----------------------------------------------------------------------
        // CARGAR DATOS
        // ----------------------------------------------------------------------
        private void cargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CategoriaID, NombreCategoria FROM Categorias;";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgClientes.DataSource = dt;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error al cargar categorías: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Categorias_Load(object sender, EventArgs e)
        {
            this.cargarDatos();
        }

        // ----------------------------------------------------------------------
        // INSERTAR CATEGORIA
        // ----------------------------------------------------------------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                MessageBox.Show("El nombre de la categoría es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Categorias (NombreCategoria) VALUES (@Nombre)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreCategoria.Text);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Categoría insertada correctamente ✔️");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar categoría: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtNombreCategoria.Clear();
        }

        // ----------------------------------------------------------------------
        // ELIMINAR CATEGORIA
        // ----------------------------------------------------------------------
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || !int.TryParse(txtID.Text, out int categoriaID))
            {
                MessageBox.Show("Debe introducir un ID válido de categoría.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "DELETE FROM Categorias WHERE CategoriaID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", categoriaID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Categoría eliminada correctamente 🗑️");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró una categoría con ese ID.", "Información");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar categoría: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtID.Clear();
        }

        // ----------------------------------------------------------------------
        // ACTUALIZAR CATEGORIA
        // ----------------------------------------------------------------------
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDActualizar.Text) || !int.TryParse(txtIDActualizar.Text, out int categoriaID))
            {
                MessageBox.Show("Debe introducir un ID válido para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreActualizado.Text))
            {
                MessageBox.Show("Debe introducir un nuevo nombre para la categoría.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Categorias SET NombreCategoria = @Nombre WHERE CategoriaID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreActualizado.Text);
                    cmd.Parameters.AddWithValue("@ID", categoriaID);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Categoría actualizada correctamente 🔄");
                        else
                            MessageBox.Show("No se encontró una categoría con ese ID.", "Información");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al actualizar categoría: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtIDActualizar.Clear();
            txtNombreActualizado.Clear();
        }

        // ----------------------------------------------------------------------
        // BOTÓN CARGAR DATOS
        // ----------------------------------------------------------------------
        private void btnCargar_Click(object sender, EventArgs e)
        {
            this.cargarDatos();
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
            {
                MessageBox.Show("El nombre de la categoría es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO Categorias (NombreCategoria) VALUES (@Nombre)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreCategoria.Text);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Categoría insertada correctamente ✔️");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al insertar categoría: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtNombreCategoria.Clear();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || !int.TryParse(txtID.Text, out int categoriaID))
            {
                MessageBox.Show("Debe introducir un ID válido de categoría.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "DELETE FROM Categorias WHERE CategoriaID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", categoriaID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Categoría eliminada correctamente 🗑️");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró una categoría con ese ID.", "Información");
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al eliminar categoría: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtID.Clear();

        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIDActualizar.Text) || !int.TryParse(txtIDActualizar.Text, out int categoriaID))
            {
                MessageBox.Show("Debe introducir un ID válido para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreActualizado.Text))
            {
                MessageBox.Show("Debe introducir un nuevo nombre para la categoría.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Categorias SET NombreCategoria = @Nombre WHERE CategoriaID = @ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreActualizado.Text);
                    cmd.Parameters.AddWithValue("@ID", categoriaID);

                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Categoría actualizada correctamente 🔄");
                        else
                            MessageBox.Show("No se encontró una categoría con ese ID.", "Información");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error al actualizar categoría: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.cargarDatos();
            txtIDActualizar.Clear();
            txtNombreActualizado.Clear();
        }
    }
}


