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
        public string cartDisplay = History.GetCartDisplay();
        public string position = History.GetCartPosition();
        public int cartItemCount = History.GetItemCount();

        public List<string> bios = new List<string>();
        public List<string> castImg = new List<string>();
        public List<string> posterPaths = new List<string>();
        public List<string> titles = new List<string>();
        public List<string> vote = new List<string>();
        public List<string> movieId = new List<string>();

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

        public async Task OnPostLogin(string email, string password)
        {
            Program.Control.Login(email, password);
            await ReloadPageContent();
        }

        public async Task ReloadPageContent()
        {
            await OnGet(Convert.ToString(History.CastID));
        }

        public async Task OnPostSignout()
        {
            Program.Control.UserExist = 0;
            await ReloadPageContent();
        }
    }
}