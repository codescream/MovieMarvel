using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieMarvel.Pages
{
    public class CartModel : PageModel
    {
        public string showSignUpForm = "none";
        public string errDisplay = "none";
        public string creationDisplay = "none";
        public string errCreationDisplay = "none";
        public string cartDisplay = History.GetCartDisplay();
        public string position = History.GetCartPosition();
        public int cartItemCount = History.GetItemCount();
        public double totalCost = Program.Rental.totalMovieCost;

        public List<string> rentedMoviePosters = Program.Rental.moviePosters;
        public List<string> rentedMovieTitles = Program.Rental.movieTitles;
        public List<double> rentedMovieCosts = Program.Rental.movieCosts;
        public List<int> rentedMovieIDs = Program.Rental.movieIDs;
        public List<string> similarMovies;
        public List<string> similarMoviesPoster = new List<string>();
        public List<string> similarMoviesTitle = new List<string>();
        public List<string> similarMoviesID = new List<string>();

        public async Task OnGet(string id)
        {
            int moviePer;
            if (Convert.ToInt32(id) > 4)// if more than 4 movies in cart show only 1 similar movie else show 2 per
            {
                moviePer = 1;
            }
            else
            {
                moviePer = 2;
            }
            for (int i = 0; i < rentedMovieIDs.Count; i++)
            {
                await Program.Fetch.GetSImilarMovies(rentedMovieIDs[i]);

                JsonNinja similarMovieDetails = new JsonNinja(Program.Fetch.SimilarMovies);
                similarMovies  = similarMovieDetails.GetVals();
                similarMovieDetails = new JsonNinja(similarMovies[1]);
                List<string> moviePoster = similarMovieDetails.GetDetails("\"poster_path\"");
                List<string> movieTitle = similarMovieDetails.GetDetails("\"title\"");
                List<string> movieID = similarMovieDetails.GetIds("\"id\"");
                List<string> movieRating = similarMovieDetails.GetIds("\"vote_average\"");

                int count = 0;
                for(int j = 0; j < movieTitle.Count; j++)
                {
                    if(Convert.ToDouble(movieRating[j]) > 6.0 && !rentedMovieTitles.Contains(movieTitle[j]) 
                       && !similarMoviesTitle.Contains(movieTitle[j]))
                    {
                        count++;
                        similarMoviesPoster.Add(moviePoster[j]);
                        similarMoviesTitle.Add(movieTitle[j]);
                        similarMoviesID.Add(movieID[j]);

                        if (count == moviePer)
                        {
                            break;
                        }
                    }
                }
            }
        }
        public async Task OnPostRemove(string remove)
        {
            if(Program.Control.UserExist == 0)
            {
                //int movieIndex = Convert.ToInt32(remove);
                int movieIndex = Program.Rental.movieTitles.IndexOf(remove);

                Program.Rental.moviePosters.RemoveAt(movieIndex);
                Program.Rental.movieTitles.RemoveAt(movieIndex);
                Program.Rental.movieCosts.RemoveAt(movieIndex);
                Program.Rental.movieIDs.RemoveAt(movieIndex);

                ReturnHome();
            }
            else
            {
                int movieIndex = Program.Rental.movieTitles.IndexOf(remove);
                int movieID = Program.Rental.movieIDs[movieIndex];
                Program.Rental.RemoveItemFromDatabase(movieID, Program.Control.CartID);

                ReturnHome();
            }
            await OnGet(Convert.ToString(Program.Rental.moviePosters.Count));
        }

        public int UpdateMovieDetails()
        {
            rentedMoviePosters = Program.Rental.moviePosters;
            rentedMovieTitles = Program.Rental.movieTitles;
            rentedMovieCosts = Program.Rental.movieCosts;

            History.SetItemCount(rentedMovieTitles.Count);
            cartItemCount = History.GetItemCount();

            totalCost = Program.Rental.GetTotalMovieCost();

            return cartItemCount;
        }

        public void OnPostSignout()
        {
            Program.Control.UserExist = 0;
            Program.Control.CartID = 0;
            Program.Rental.movieIDs.Clear();
            Program.Rental.moviePosters.Clear();
            Program.Rental.movieTitles.Clear();
            Program.Rental.movieCosts.Clear();
            History.ResetItemCount();
            History.SetCartDisplay("none");
            History.SetCartPosition("0px");
            cartDisplay = History.GetCartDisplay();
            position = History.GetCartPosition();
            cartItemCount = History.GetItemCount();

            Response.Redirect("./Index");
        }

        public void OnPostLogin(string emailsignin, string passwordsignin)
        {
            int cartCount = Program.Control.Login(emailsignin, passwordsignin); // login and retrieve cart item count

            if (cartCount < 0)
            {
                errDisplay = "inline";
            }
            else
            {
                History.SetItemCount(cartCount);
                cartItemCount = History.GetItemCount();
                totalCost = Program.Rental.totalMovieCost;
                cartDisplay = "inline";

                if (Program.Rental.movieIDs.Count > 0)
                {
                    Program.Rental.AddItemsFromList(cartID: Program.Control.CartID);
                    totalCost = Program.Rental.totalMovieCost;
                    cartItemCount = History.GetItemCount() + Program.Rental.addedListItems;
                    History.SetItemCount(cartItemCount);
                }
                else
                {
                    //return current cart item count
                    cartItemCount = History.GetItemCount();

                    //get movie details from database
                    Program.Rental.GetMovieList(cartID: Program.Control.CartID);
                    totalCost = Program.Rental.totalMovieCost;
                }

                AdjustCartDivs();
            }
        }
        public void AdjustCartDivs()
        {
            cartDisplay = "inline";
            position = "-13px";
            History.SetCartDisplay("inline");
            History.SetCartPosition("-13px");
        }

        public void ReturnHome()
        {
            int count = UpdateMovieDetails();
            if (count == 0)
            {
                History.SetCartPosition("0px");
                cartDisplay = History.GetCartDisplay();
                position = History.GetCartPosition();
                Response.Redirect("./Index");
            }
        }
    }
}