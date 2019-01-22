using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieMarvel.Utitlity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MovieMarvel.Pages
{
    public class IndexModel : PageModel
    {
        public List<string> movies = new List<string>();
        public List<string> posterPath = new List<string>();

        public List<string> id = new List<string>();
        public List<string> vidClips = new List<string>();
        public List<string> title = new List<string>();
        public List<string> actorImg = new List<string>();
        public List<string> castID = new List<string>();
        public List<string> filter = new List<string>();
        public List<string> rentedMoviePosters = Program.Rental.moviePosters;
        public List<string> rentedMovieTitles = Program.Rental.movieTitles;

        public string rating = "";
        public string description = "";
        public string goldStar = "";
        public string display = "none";
        public string search = "";
        public string showSignUpForm = "none";
        public bool emptySearch = false;
        public bool invalidSearch = false;
        public string backDrop = "";
        public string moviePoster;
        public string moviePosterShow = "none";
        public string showMoviePosters = "block";
        public string cartDisplay = History.GetCartDisplay();
        public string position = History.GetCartPosition();
        public string tooltip = "none";
        public string holder = "search";
        public string movieID = "";
        public float thumbsUp = 1;
        public float thumbsDown = 1;
        public int cartItemCount = History.GetItemCount();
        public float inBetween = 1;

        JsonNinja jNinja;
        JsonNinja vidNinja;

        public async Task OnGet(string id)
        {
            if (id == null)
            {
                //do nothing
            }
            else
            {
                await OnPostPoster(id);
            }
        }
            public async Task OnPostVote(string vote)
        {
            using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand Vote = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                Vote.Parameters.AddWithValue("@MovieID", History.value);
                Vote.Parameters.AddWithValue("@VoteRating", Convert.ToInt32(vote));

                if (DisplayVote() == -1)
                {
                    Vote.CommandText = ("[spAddVote]");
                }
                else
                {
                    Vote.CommandText = ("[spUpdateVote]");
                }

                Vote.CommandType = System.Data.CommandType.StoredProcedure;

                Vote.ExecuteNonQuery();

                myConn.Close();
            }
            await OnPostMovies(History.value);
            DisplayVote();
        }

        public int DisplayVote()
        {
            int voteStat = -1;
             using (SqlConnection myConn = new SqlConnection(Program.Fetch.cs))
            {
                SqlCommand checkVote = new SqlCommand
                {
                    Connection = myConn
                };
                myConn.Open();

                checkVote.Parameters.AddWithValue("@MovieID", History.value);

                checkVote.CommandText = ("[spGetVote]");
                checkVote.CommandType = System.Data.CommandType.StoredProcedure;

                if(checkVote.ExecuteScalar() != null)
                {
                    voteStat = Convert.ToInt32(checkVote.ExecuteScalar());
                }

                myConn.Close();
            }
            return voteStat;
        }

        public async Task OnPostPoster(string search)
        {
            this.search = search;
            await ShowPoster(search);
        }

        public async Task OnPostLogin(string email, string password)
        {
            int cartCount = Program.Control.Login(email, password); // login and retrieve cart item count
            History.SetItemCount(cartCount);
            cartItemCount = History.GetItemCount();
            cartDisplay = "inline";

            if(Program.Rental.movieIDs.Count > 0)
            {
                Program.Rental.AddItemsFromList(cartID: Program.Control.CartID);
                cartItemCount = History.GetItemCount() + Program.Rental.addedListItems;
                History.SetItemCount(cartItemCount);
            }
            else
            {
                //return current cart item count
               cartItemCount = History.GetItemCount();
            }

            await ReloadPageContent();
        }

        public async Task OnPostSignout()
        {
            Program.Control.UserExist = 0;
            History.ResetItemCount();
            cartItemCount = History.GetItemCount();
            await ReloadPageContent();
        }
        public async Task OnPostCreateAccount(string email, string password)
        {
            Program.Control.CreateAccount(email, password);
            
            await ReloadPageContent();
        }

        public void OnPostShowcart()
        {
            //do nothing for now
        }

        public async Task OnPostRentMovie()
        {
            int movieID = Convert.ToInt32(History.GetMovieID());
            string movieTitle = History.GetMovieTitle();
            string moviePoster = History.GetBackDrop();//movie poster
            
            int addItem = Program.Rental.AddItem(movieID, Program.Control.CartID, movieTitle, moviePoster);
            if(addItem == 0)//item already in cart
            {
                //show and fade away 'movie already exist' tooltip
                tooltip = "inline";
                await AdjustCartDivs();
            }
            else
            {
                History.IncrementItemCount();
                cartItemCount = History.GetItemCount();
                await AdjustCartDivs();
            }

            
        }
        public async Task OnPostMovies(string intent)
        {
            showMoviePosters = "none";
            await ShowPoster(Program.Fetch.Search);
            search = Program.Fetch.Search;
            this.search = Program.Fetch.Search;

            await Program.Fetch.GrabMovieAsync(intent);
            vidNinja = new JsonNinja(Program.Fetch.Videos);

            filter = vidNinja.GetDetails("\"results\"");

            vidNinja = new JsonNinja(filter[0]);

            List<string> vidNames = vidNinja.GetNames();
            List<string> vidVals = vidNinja.GetVals();

            jNinja = new JsonNinja(Program.Fetch.Details);
            List<string> detailNames = jNinja.GetNames();
            List<string> detailVals = jNinja.GetVals();

            //get movie details
            display = "grid";
            title = jNinja.GetDetails("\"title\"");
            List<string> ratings = jNinja.GetIds("\"vote_average\"");
            
            rating = ratings[0] + "/10.0";
            List<string> descriptions = jNinja.GetDetails("\"overview\"");
            description = descriptions[0];
            List<string> backDrops = jNinja.GetDetails("\"poster_path\"");
            
            if(backDrops.Count == 2)
            {
                backDrop = backDrops[1];// backdrop or movieposter
            }
            else
            {
                backDrop = backDrops[0];// backdrop or movieposter
            }

            //get youtube movie keys for poster
            vidClips = vidNinja.GetDetails("\"key\"");

            //get movie cast
            jNinja = new JsonNinja(Program.Fetch.Credits);
            List<string> creditNames = jNinja.GetNames();
            List<string> creditVals = jNinja.GetVals();

            filter = jNinja.GetDetails("\"cast\"");

            jNinja = new JsonNinja(filter[0]);

            History.SetMovieID(creditVals[0]);//to be used by rental method
            History.SetBackDrop(backDrop);//same as poster, for cast page backdrop and rental method
            History.SetMovieTitle(title[0]);//for use with rental method


            List<string> actorName = jNinja.GetDetails("\"name\"");
            castID = jNinja.GetIds("\"id\"");
            actorImg = jNinja.GetDetails("\"profile_path\"");

            List<string> character = jNinja.GetDetails("\"character\"");

            int voteStat = DisplayVote();

           if(voteStat == 2)
            {
                thumbsDown = 0.4f;
                inBetween = 0.4f;
            }
            else if(voteStat == 1)
            {
                thumbsUp = 0.4f;
                thumbsDown = 0.4f;
            }
            else if(voteStat == 0)
            {
                thumbsUp = 0.4f;
                inBetween = 0.4f;
            }
            else
            {
                //do nothing
            }
            moviePosterShow = "block";
        }
        private async Task ShowPoster(string search)
        {
            await Program.Fetch.GrabPosterAsync(search);

            if(Program.Fetch.Data == null)
            {
                emptySearch = true;
            }
            else
            {
                jNinja = new JsonNinja(Program.Fetch.Data);

                List<string> names = jNinja.GetNames();
                List<string> vals = jNinja.GetVals();

                if(vals[1] == "0")
                {
                    invalidSearch = true;
                }
                else
                {
                    filter = jNinja.GetDetails("\"results\"");

                    jNinja = new JsonNinja(filter[0]);

                    posterPath = jNinja.GetPosters("\"poster_path\"");
                    id = jNinja.GetIds("\"id\"");
                }
            }
        }

        public async Task ReloadPageContent()
        {
            if (Program.Fetch.Search != null)
            {
                await ShowPoster(Program.Fetch.Search);
            }
            if (History.GetMovieID() != null)
            {
                await OnPostMovies(History.GetMovieID());
            }
        }

        public async Task AdjustCartDivs()
        {
            cartDisplay = "inline";
            position = "-13px";
            History.SetCartDisplay("inline");
            History.SetCartPosition("-13px");
            await ReloadPageContent();
        }
    }
}