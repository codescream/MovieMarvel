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

                login.CommandText = ("[spLogin]");

                login.CommandType = System.Data.CommandType.StoredProcedure;

                int result = 0;
                if(login.ExecuteScalar() != null)
                {
                    result = Convert.ToInt32(login.ExecuteScalar());
                }

                SqlCommand GetPassword = new SqlCommand
                {
                    Connection = myConn
                };

                GetPassword.Parameters.AddWithValue("@userID", result);

                GetPassword.CommandText = ("SELECT password FROM [User] WHERE id = @userID");

                GetPassword.CommandType = System.Data.CommandType.Text;

                var result1 = GetPassword.ExecuteScalar();

                if (result1 != null)
                {
                    if(Program.Hasher.Verify(password, Convert.ToString(result1)))
                    {
                        UserExist = result;
                    }
                }

                if(UserExist == 0)
                {
                    return -1;
                }
                else
                {
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
        }

        public int CreateAccount(string email, string password)
        {
            int userid = 0;
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand checkUser = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                checkUser.Parameters.AddWithValue("@email", email);

                checkUser.CommandText = ("SELECT id FROM [User] WHERE email = @email");

                checkUser.CommandType = System.Data.CommandType.Text;

                var result = checkUser.ExecuteScalar();
                if (result != null)
                {
                    myConn.Close();
                    return -1;
                }
                else
                {
                    SqlCommand addUser = new SqlCommand
                    {
                        Connection = myConn
                    };

                    string userPassword = Program.Hasher.Hash(password); //hash inserted password

                    addUser.Parameters.AddWithValue("@email", email);
                    addUser.Parameters.AddWithValue("@password", userPassword);

                    addUser.CommandText = ("[spAddUser]");

                    addUser.CommandType = System.Data.CommandType.StoredProcedure;

                    var result1 = addUser.ExecuteScalar();
                    if (result1 != null)
                    {
                        userid = Convert.ToInt32(result1);
                    }


                    SqlCommand addCart = new SqlCommand
                    {
                        Connection = myConn
                    };

                    addCart.Parameters.AddWithValue("@UserID", userid);

                    addCart.CommandText = ("[spAddCart]");

                    addCart.CommandType = System.Data.CommandType.StoredProcedure;

                    var result2 = addCart.ExecuteScalar();
                    if (result2 != null)
                    {
                        CartID = Convert.ToInt32(result2);
                    }
                    myConn.Close();
                    return 0;
                }
            }
        }
    }
}