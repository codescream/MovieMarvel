using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieMarvel.Pages
{
    public class CastModel : PageModel
    {
        public string castId = "";
        public string bio;
        public string img;
        public string backDrop = "";
        public string name = "";
        public string movieID = "";
        public string showSignUpForm = "none";
        public string errDisplay = "none";
        public string creationDisplay = "none";
        public string errCreationDisplay = "none";
        public string cartDisplay = History.GetCartDisplay();
        public string position = History.GetCartPosition();
        public int cartItemCount = History.GetItemCount();
        public double totalCost = Program.Rental.totalMovieCost;

        public List<string> bios = new List<string>();
        public List<string> castImg = new List<string>();
        public List<string> posterPaths = new List<string>();
        public List<string> titles = new List<string>();
        public List<string> vote = new List<string>();
        public List<string> movieId = new List<string>();
        public List<string> rentedMoviePosters = Program.Rental.moviePosters;
        public List<string> rentedMovieTitles = Program.Rental.movieTitles;
        public List<double> rentedMovieCosts = Program.Rental.movieCosts;

        public async Task OnGet(string id)
        {
            backDrop = History.GetBackDrop();
            History.CastID = Convert.ToInt32(id);

            movieID = History.GetMovieID();
            await Program.Fetch.GrabCastBioAsync(id);

            string bioData = Program.Fetch.Bios;
            string movieCredits = Program.Fetch.movieCredits;

            JsonNinja bNinja = new JsonNinja(bioData);

            bios = bNinja.GetDetails("\"biography\"");
            name = bNinja.GetDetails("\"name\"")[0];
            bio = ((bios[0].Replace("\n", "")).Replace("\\", "")).Replace(".nn", ". ");
            castImg = bNinja.GetPosters("\"profile_path\"");
            img = castImg[0];

            bNinja = new JsonNinja(movieCredits);

            List<string> cast = bNinja.GetDetails("\"cast\"");

            bNinja = new JsonNinja(cast[0]);

            vote = bNinja.GetIds("\"vote_average\"");
            movieId = bNinja.GetIds("\"id\"");
            posterPaths = bNinja.GetDetails("\"poster_path\"");
            titles = bNinja.GetDetails("\"original_title\"");
        }

        public async Task OnPostLogin(string emailsignin, string passwordsignin)
        {
            int cartCount = Program.Control.Login(emailsignin, passwordsignin); // login and retrieve cart item count

            if (cartCount < 0)
            {
                errDisplay = "inline";
                await ReloadPageContent();
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

                await AdjustCartDivs();
            }
        }

        public async Task ReloadPageContent()
        {
            await OnGet(Convert.ToString(History.CastID));
        }

        public async Task OnPostSignout()
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

            //Program.Control.UserExist = 0;
            await ReloadPageContent();
        }

        public async Task AdjustCartDivs()
        {
            cartDisplay = "inline";
            position = "-13px";
            History.SetCartDisplay("inline");
            History.SetCartPosition("-13px");
            await ReloadPageContent();
        }

        public async Task OnPostCreateAccount(string myemail, string mypassword)
        {
            int userExist = Program.Control.CreateAccount(myemail, mypassword);

            if (userExist < 0)
            {
                errCreationDisplay = "inline";
            }
            else
            {
                creationDisplay = "inline";
            }

            await ReloadPageContent();
        }
    }
}