using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel
{
    public class History
    {
        public static string value;
        public static string backDropImg = "";
        public static string movieTitle = "";
        static string cartDisplay = "none";
        public static string cartPosition = "0px";
        public static int itemCount = 0;
        public static int CastID { get; set; }
        public static void SetMovieID(string id)
        {
            value = id;
        }

        public static string GetMovieID()
        {
            return value;
        }

        public static void SetCartDisplay(string display)
        {
            cartDisplay = display;
        }

        public static void SetItemCount(int cartCount)
        {
            itemCount = cartCount;
        }
        public static int GetItemCount()
        {
            return itemCount;
        }

        public static void IncrementItemCount()
        {
            itemCount++;
        }

        public static void ResetItemCount()
        {
            itemCount = 0;
        }

        public static string GetCartDisplay()
        {
            return cartDisplay;
        }

        public static void SetCartPosition(string position)
        {
            cartPosition = position;
        }
        public static string GetCartPosition()
        {
            return cartPosition;
        }
        public static void SetMovieTitle(string title)
        {
            movieTitle = title;
        }

        public static string GetMovieTitle()
        {
            return movieTitle;
        }

        public static void SetBackDrop(string img)
        {
            backDropImg = img;
        }

        public static string GetBackDrop()
        {
            return backDropImg;
        }
    }
}
