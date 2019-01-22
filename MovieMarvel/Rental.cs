using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel
{
    public class Rental
    {
        public List<int> movieIDs = new List<int>();
        public List<string> moviePosters = new List<string>();
        public List<string> movieTitles = new List<string>();
        public int addedListItems;
        public int AddItem(int movieID, int cartID, string movieTitle, string moviePoster)
        {
            if (cartID == 0)
            {
                if (movieIDs.Contains(movieID))//deny addition of duplicate movie rental to list
                {
                    //flash movie exist tooltip!
                    return 0;
                }
                else
                {
                    movieIDs.Add(movieID);
                    moviePosters.Add(moviePoster);
                    movieTitles.Add(movieTitle);

                    return 1;
                }
            }
            else
            {
                using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
                {
                    SqlCommand addItem = new SqlCommand
                    {
                        Connection = myConn
                    };
                    myConn.Open();

                    if (CheckItemExist(movieID, cartID) == 0) // denies duplicate entries
                    {
                        AddItemToDatabase(addItem, movieID, cartID, moviePoster, movieTitle);
                        
                        myConn.Close();

                        return 1;
                    }
                    else
                    {
                        //do not re-add item
                        //should flash movie exist tooltip!
                        return 0;
                    }
                }
                
            }
        }

        public void AddItemsFromList(int cartID) // transfers items in cart to database once customer logs in
        {
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand addItem = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                for (int i = 0; i < movieIDs.Count; i++)
                {
                    if(CheckItemExist(movieIDs[i], cartID) == 0) // denies duplicate entries
                    {
                        AddItemToDatabase(addItem, movieIDs[i], cartID, moviePosters[i], movieTitles[i]);
                        
                        addItem.Parameters.Clear();

                        addedListItems++;
                    }
                    else
                    {
                        // do nothing - do not re-add item
                    }
                }

                movieIDs.Clear();
                moviePosters.Clear();
                movieTitles.Clear();

                myConn.Close();
            }
        }

        public int CheckItemExist(int movieID, int cartID) // checks to avoid duplicate entries
        {
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand checkItem = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                checkItem.Parameters.AddWithValue("@movieID", movieID);
                checkItem.Parameters.AddWithValue("@cartID", cartID);

                checkItem.CommandText = ("[spCheckItemExist]");

                checkItem.CommandType = System.Data.CommandType.StoredProcedure;

                if (checkItem.ExecuteScalar() == null)
                {
                    myConn.Close();
                    return 0;
                } 
                else
                {
                    myConn.Close();
                    return 1;
                }
            }
        }

        public void AddItemToDatabase(SqlCommand addItem, int movieID, int cartID, string moviePoster, string movieTitle)
        {
            addItem.Parameters.AddWithValue("@MovieID", movieID);
            addItem.Parameters.AddWithValue("@CartID", cartID);
            addItem.Parameters.AddWithValue("@MoviePoster", moviePoster);
            addItem.Parameters.AddWithValue("@MovieTitle", movieTitle);

            addItem.CommandText = ("[spAddItem]");

            addItem.CommandType = System.Data.CommandType.StoredProcedure;

            addItem.ExecuteNonQuery();
        }
    }
}
