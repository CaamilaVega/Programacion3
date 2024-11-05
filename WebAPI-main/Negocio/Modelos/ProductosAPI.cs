using Dapper;
using MySql.Data.MySqlClient;
using Negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Modelos
{
    public  class ProductosAPI
    {
        string connStr = "Server=db4free.net;Database=lasnieves110424;Uid=lasnieves110424;Pwd=lasnieves110424;";
        public List<Datos> GetProduct()
        {
            List<Datos> ListaProducts = new List<Datos>();
            using (MySqlConnection myConn = new MySqlConnection(connStr))
            {
                myConn.Open();
                string sql = "SELECT* FROM Products";
                ListaProducts= myConn.Query <Datos>(sql).ToList();
            }
            return ListaProducts;

        }

        public List<string> GetCategories()
        {
            List<string> ListaProducts = new List<string>();
            using (MySqlConnection myConn = new MySqlConnection(connStr))
            {
                myConn.Open();
                string sql = "SELECT Category FROM Categories";
                ListaProducts = myConn.Query<string>(sql).ToList();
            }
            return ListaProducts;

        }
        public Datos GetId(int id)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();
            string sql = "SELECT Id,Description,Title,Price,Category FROM Products where id = @id";
            Datos datProd = conn.QueryFirst<Datos>(sql, new { Id = id });

            return datProd;
        }

        public Datos GetCategory(string category)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT Id, Description, Title, Price,Category FROM Products WHERE category = @category";
                Datos datCateg = conn.QueryFirst<Datos>(sql, new { category });

                return datCateg;
            }
        }

        public Datos PostDat(Datos producto)
        {
            using (MySqlConnection myConn = new MySqlConnection(connStr))
            {
                myConn.Open();
                string sql = "INSERT INTO Products (Title,Price,Category,Description) VALUES(@Title,@Price,@Category,@Description)";
                myConn.Execute(sql, new
                {
                    Title = producto.Title,
                    Price = producto.Price,
                    Category = producto.Category,
                    Description = producto.Description,

                });

                return producto;
            }
        }

        public void PutId(Datos datos)
        {
            using (MySqlConnection myConn = new MySqlConnection(connStr))
            {
                myConn.Open();
                string sql = "UPDATE Products SET Title=@Title, Price=@Price, Category=@Category, Description=@Description WHERE Id=@Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, myConn))
                {
                    cmd.Parameters.AddWithValue("@Id", datos.Id);
                    cmd.Parameters.AddWithValue("@Title", datos.Title);
                    cmd.Parameters.AddWithValue("@Price", datos.Price);
                    cmd.Parameters.AddWithValue("@Category", datos.Category);
                    cmd.Parameters.AddWithValue("@Description", datos.Description);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void PutCategory(Datos datosCat)
        {
            using (MySqlConnection myConn = new MySqlConnection(connStr))
            {
                myConn.Open();
                string sql = "UPDATE Products SET Title=@Title, Price=@Price,Id=@Id, Description=@Description WHERE Category = @Category";

                using (MySqlCommand cmd = new MySqlCommand(sql, myConn))
                {
                    cmd.Parameters.AddWithValue("@Id", datosCat.Id);
                    cmd.Parameters.AddWithValue("@Title", datosCat.Title);
                    cmd.Parameters.AddWithValue("@Price", datosCat.Price);
                    cmd.Parameters.AddWithValue("@Category", datosCat.Category);
                    cmd.Parameters.AddWithValue("@Description", datosCat.Description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteID(int Id)
        {
            using (MySqlConnection myConn = new MySqlConnection(connStr))
            {
                myConn.Open();
                string sql = "DELETE FROM Products WHERE Id=@Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, myConn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteCategory(string category)
        {
            using (MySqlConnection myConn = new MySqlConnection(connStr))
            {
                myConn.Open();
                string sql = "DELETE FROM Products WHERE category=@category";

                using (MySqlCommand cmd = new MySqlCommand(sql, myConn))
                {
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.ExecuteNonQuery();
                }
            }
        }




    }
}

