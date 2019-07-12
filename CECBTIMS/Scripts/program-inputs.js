//Hide the inputs at the page start
HideAll();

$(window).ready(function() {

    $("select#ProgramType").change(function() {

        var selectedValue = $(this).children("option:selected").text();

        switch (selectedValue) {
        case "Local":
                ShowOne("localInput");
                HideOthers(["foreignInput", "postGradInput","InHouseInput"]);
            break;
        case "Foreign":
            alert("Foreign selected");
            break;
        case "InHouse":
            alert("InHouse selected");
            break;
        case "PostGraduation":
            alert("Post selected");
            break;

        default:
            HideAll();
        }

    });

});


function ShowOne(classname)
{
    //get all the divs with current class
    var elements = document.getElementsByClassName(classname);
    //loop through them and show them
    for (var i = 0; i < elements.length; ++i) {
        var element = elements[i].style;
        element.display = element.display === "none" ? "block": "";
    }

}

function HideOthers(classes) {

}

//
//show curent input class
//    hide others exepct contining current class



function HideAll() {
    $(".localInput").hide();
    $(".foreignInput").hide();
    $(".postGradInput").hide();
    $(".InHouseInput").hide();
}
//
//function ToggleLocal() {
//
//    var els = document.getElementsByClassName("localInput");
//
//    for (var i = 0; i < els.length; ++i) {
//        var s = els[i].style;
//        s.display = s.display === 'none' ? 'block' : 'none';
//    }
//
//}
//
//function HidePostGrad() {
//    $(".foreignInput").hide();
//}
//
//function HideForeign() {
//    $(".postGradInput").hide();
//}
//
//function HideInHouse() {
//    $(".InHouseInput").hide();
//}