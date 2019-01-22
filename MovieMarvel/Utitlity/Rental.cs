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
        public List<double> movieCosts = new List<double>();

        public string status = "";
        public double totalMovieCost = 0.0;

        public int addedListItems;
        public int AddItem(int movieID, int cartID, float cost, string movieTitle, string moviePoster)
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
                    movieCosts.Add(cost);

                    totalMovieCost = GetTotalMovieCost();
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

                        GetMovieList(cartID);

                        totalMovieCost = GetTotalMovieCost();

                        return 1;
                    }
                    else
                    {
                        if (status == "" || status == "A")
                        {
                            //do not re-add item
                            //should flash movie exist tooltip!
                            GetMovieList(cartID);
                            myConn.Close();
                            return 0;
                        }
                        else
                        {

                            SqlCommand ChangeItemStatus = new SqlCommand
                            {
                                Connection = myConn
                            };

                            ChangeItemStatus.Parameters.AddWithValue("@cartID", cartID);
                            ChangeItemStatus.Parameters.AddWithValue("@movieID", movieID);

                            ChangeItemStatus.CommandText = ("UPDATE Item SET [Status] = 'A' WHERE cartID = @cartID AND MovieID = @movieID");

                            ChangeItemStatus.CommandType = System.Data.CommandType.Text;

                            ChangeItemStatus.ExecuteNonQuery();

                            GetMovieList(cartID);
                            myConn.Close();
                            return 1;
                        }
                    }
                }
            }
        }

        public void AddItemsFromList(int cartID) // transfers items in cart to database once customer logs in
        {
            movieCosts.Clear();
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

                totalMovieCost = GetTotalMovieCost();

                movieIDs.Clear();
                moviePosters.Clear();
                movieTitles.Clear();
                movieCosts.Clear();

                GetMovieList(cartID);

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

                var test = checkItem.ExecuteScalar();

                if (test == null)
                {
                    status = Convert.ToString(test);
                    myConn.Close();
                    return 0;
                } 
                else
                {
                    status = Convert.ToString(test);
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



        public void GetMovieList(int cartID)
        {
            movieIDs.Clear();
            moviePosters.Clear();
            movieTitles.Clear();
            movieCosts.Clear();

            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand getMovieList = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                getMovieList.Parameters.AddWithValue("@cartID", cartID);

                getMovieList.CommandText = ("[spGetMovieList]");

                getMovieList.CommandType = System.Data.CommandType.StoredProcedure;

                var result = getMovieList.ExecuteReader();

                int movieID = 0;
                int moviePoster = 0;
                int movieTitle = 0;
                int movieCost = 0;
                if (result != null)
                {
                    movieID = result.GetOrdinal("MovieID");
                    moviePoster = result.GetOrdinal("MoviePoster");
                    movieTitle = result.GetOrdinal("MovieTitle");
                    movieCost = result.GetOrdinal("Cost");
                }

                while(result.Read())
                {
                   movieIDs.Add(Convert.ToInt32(result.GetString(movieID)));
                   moviePosters.Add(result.GetString(moviePoster));
                   movieTitles.Add(result.GetString(movieTitle));
                   movieCosts.Add(Convert.ToSingle(result.GetValue(movieCost)));
                }
                myConn.Close();
            }
            totalMovieCost = GetTotalMovieCost();
        }

        public double GetTotalMovieCost()
        {
            double total = 0.0;
            for (int i = 0; i < movieCosts.Count; i++)
            {
                total += movieCosts[i];
            }
            return total;
        }

        public void RemoveItemFromDatabase(int movieID, int cartID)
        {
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand deleteItem = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                deleteItem.Parameters.AddWithValue("@cartID", cartID);
                deleteItem.Parameters.AddWithValue("@movieID", movieID);

                deleteItem.CommandText = ("[spDeleteItem]");

                deleteItem.CommandType = System.Data.CommandType.StoredProcedure;

                deleteItem.ExecuteNonQuery();

                myConn.Close();

                GetMovieList(cartID);
            }
        }
    }
}
