// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//get elements with same class name
var slides = document.getElementsByClassName("images");
//set initial poster values
var imageLink = document.getElementById("0").getAttribute("src");
document.getElementById("mySlides").setAttribute("src", imageLink);
var titles = document.getElementById("0").getAttribute("name");
document.getElementById("title").innerHTML = titles;
document.getElementById("mySlides").setAttribute("value", document.getElementById("0").getAttribute("name"));

var slideIndex = 0;

// Next/previous controls
function plusSlides(n)
{
    slideIndex += n;
    if (slideIndex < 0 || slideIndex > (slides.length - 1))
    {
        if (slideIndex < 0)
        {
            slideIndex = 0;
        }
        else if (slideIndex > (slides.length - 1))
        {
            slideIndex = slides.length - 1;
        }
    }
    else
    {
        showSlides(slideIndex);
    }
}

// showSlides(slideIndex);
function showSlides(ids)
{
    getDetails(ids);
}

function mySlidesClicked()
{
    window.location.href = ("./Index?id=" + document.getElementById("mySlides").getAttribute("value"));
}

// Thumbnail image controls
function currentSlide(id)
{
    getDetails(id);
    var index = document.getElementById(id).getAttribute("id");
    slideIndex = parseInt(index, 10);
}

function getDetails(id)
{
    imageLink = document.getElementById(id).getAttribute("src");
    document.getElementById("mySlides").setAttribute("src", imageLink);
    var titles = document.getElementById(id).getAttribute("name");
    document.getElementById("title").innerHTML = titles;
    document.getElementById("mySlides").setAttribute("value", document.getElementById(id).getAttribute("name"));
}

var show = "false";
function showSignupForm()
{
    // open panel
        $("#signupForm").animate({
            height: 'toggle'
    }, 220, 'swing');

    $("#blocker").animate({
        height: 'toggle'
    }, 220, 'swing');

    show = "true";
    //show panel
    //document.getElementById("form").style.display = 'block';
}

function showBigPoster()
{
    $("#searchImage").animate({
        height: 'toggle'
    }, 220, 'swing');


    /*if (document.getElementById("selectedPosterdiv").style.display == "block")
    {
        document.getElementById("selectedPosterdiv").style.display = "none";
        document.getElementById("panel").style.display = "none";
    }
    else
    {
        document.getElementById("selectedPosterdiv").style.display = "block";
        document.getElementById("panel").style.display = "block";
    }*/
}

function setdivdisplay()
{
    showBigPoster();

    document.getElementById("selectedPosterdiv").style.display = "none";
    document.getElementById("panel").style.display = "none";
    document.getElementById("video").style.display = "none";
    document.getElementById("rating").style.display = "none";
    document.getElementById("desc").style.display = "none";
    document.getElementById("cast").style.display = "none";
}

function closeForm()
{
    if (show == "true")
    {
        showSignupForm();
    }
    show = "false";
}

function closetooltip()
{
    setTimeout(function () {
        $("#tooltiptext").fadeOut(3000)
    }, 2000);
}

function showCart()
{
    $("#previewCart").animate({
        height: 'toggle'
    }, 220, 'swing');
}

function closeCart()
{
    $("#previewCart").animate({
        height: 'toggle'
    }, 220, 'swing');
}

function displayCart()
{
    document.getElementById("previewCart").css.display = "inline";
}