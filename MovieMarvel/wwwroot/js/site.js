// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//get elements with same class name

if (document.title == "Cast - MovieMarvel")
{
    //set initial poster values
    var imageLink = document.getElementById("0").getAttribute("src");
    document.getElementById("mySlides").setAttribute("src", imageLink);
    var titles = document.getElementById("0").getAttribute("name");
    document.getElementById("title").innerHTML = titles;
    document.getElementById("mySlides").setAttribute("value", document.getElementById("0").getAttribute("name"));
}
var show = 0;
var closeCartFunction;
var slides = document.getElementsByClassName("images");

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

function showSignupForm()
{
    closeCartInstant();
    // open panel
        $("#signupForm").animate({
            height: 'toggle'
    }, 220, 'swing');

    $("#blocker").animate({
        height: 'toggle'
    }, 220, 'swing');

    show += 1;
    //show panel
    //document.getElementById("form").style.display = 'block';
}

function showBigPoster()
{
    $("#searchImage").animate({
        height: 'toggle'
    }, 220, 'swing');
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
    if (show % 2 != 0)
    {
        showSignupForm();
        show = 2;
    }
}

function closetooltip()
{
    setTimeout(function () {
        $("#tooltiptext").fadeOut(3000)
    }, 2000);

    setTimeout(function () {
        $("#errlogin").fadeOut(1500)
    }, 500);

    setTimeout(function () {
        $("#errCreation").fadeOut(3000)
    }, 500);

    setTimeout(function () {
        $("#userCreation").fadeOut(3000)
    }, 500);
}

function showCart()
{
    closeForm();
    clearTimeout(closeCartFunction);
    $("#previewCart").css("visibility", "visible");
    $("#previewCart").css("display", "inline");

    /*$('#previewCart').animate({
        height: '150px'
    }, 220, 'swing');*/
}

function closeCart()
{
    closeCartFunction = setTimeout(function () { $("#previewCart").css("visibility", "hidden"); }, 1000);//wait a sec before closing
    
    /*$('#previewCart').animate({
        height: '0px'
    }, 220, 'swing');*/
}

function closeCartInstant()
{
    $("#previewCart").css("visibility", "hidden");
}

function deleteFromCart(id)
{
    document.getElementById("rentedMovie(" + id + ")").style.display = "none";
    //alert($("#moviediv(" + id + ")").attr("id"));
}

function validateSignInForm()
{
    var w = document.forms["myform"]["signinemail"].value;
    var z = document.forms["myform"]["signinpassword"].value;

    if (w == "" && z == "") {
        return false;
    }
    else if (w == "") {
        return false;
    }
    else if (z == "") {
        return false;
    }
}

function validateSignUpForm()
{
    var x = document.forms["myforms"]["signupemail"].value;
    var y = document.forms["myforms"]["signuppassword"].value;
    
    if (x == "" && y == "") {
        return false;
    }
    else if (x == "") {
        return false;
    }
    else if (y == "") {
        return false;
    }
}

function resetInputs()
{
    $("#signupemail").val('');
    $("#signuppassword").val('');
    $("#signinemail").val('');
    $("#signinpassword").val('');
}

function MovieVote(value)
{
    document.getElementById("movieVote").value = value;
}