using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel.Utitlity
{
    public class Control
    {
        public int UserExist { get; set; }
        public int CartID { get; set; }
        public int Login(string email, string password)
        {
            UserExist = 0;
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand login = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                login.Parameters.AddWithValue("@email", email);
                login.Parameters.AddWithValue("@password", password);

                login.CommandText = ("[spLogin]");

                login.CommandType = System.Data.CommandType.StoredProcedure;

                if(login.ExecuteScalar() != null)
                {
                    UserExist = Convert.ToInt32(login.ExecuteScalar());
                }

                SqlCommand getCartID = new SqlCommand
                {
                    Connection = myConn
                };

                getCartID.Parameters.AddWithValue("@UserID", UserExist);

                getCartID.CommandText = ("[spGetCartID]");

                getCartID.CommandType = System.Data.CommandType.StoredProcedure;

                if (getCartID.ExecuteScalar() != null)
                {
                    CartID = Convert.ToInt32(getCartID.ExecuteScalar());
                }

                SqlCommand getCartCount = new SqlCommand
                {
                    Connection = myConn
                };

                getCartCount.Parameters.AddWithValue("@cartID", CartID);

                getCartCount.CommandText = ("[spCartItemCount]");

                getCartCount.CommandType = System.Data.CommandType.StoredProcedure;

                if (getCartCount.ExecuteScalar() != null)
                {
                    int cartCount = Convert.ToInt32(getCartCount.ExecuteScalar());

                    myConn.Close();
                    return cartCount;
                }
                else
                {
                    myConn.Close();
                    return 0;
                }
            }
        }

        public void CreateAccount(string email, string password)
        {
            int userid = 0;
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand addUser = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                addUser.Parameters.AddWithValue("@email", email);
                addUser.Parameters.AddWithValue("@password", password);

                addUser.CommandText = ("[spAddUser]");

                addUser.CommandType = System.Data.CommandType.StoredProcedure;

                var result = addUser.ExecuteScalar();
                if (result != null)
                {
                    userid = Convert.ToInt32(result);
                }


                SqlCommand addCart = new SqlCommand
                {
                    Connection = myConn
                };

                addCart.Parameters.AddWithValue("@UserID", userid);

                addCart.CommandText = ("[spAddCart]");

                addCart.CommandType = System.Data.CommandType.StoredProcedure;

                var result1 = addCart.ExecuteScalar();
                if(result1 != null)
                {
                    CartID = Convert.ToInt32(result1);
                }
                
                myConn.Close();
            }
        }
    }
}