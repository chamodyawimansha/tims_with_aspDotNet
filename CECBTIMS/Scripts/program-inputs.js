$(window).ready(function () {

    // listen to Text change
    $("select#ProgramType").change(function ()
    {
        selectedValue = $(this).children("option:selected").text();
        changeFormStatus(selectedValue);
    }).trigger("change") ;


});

//Add required to right inputs

function changeFormStatus(value) {
    if (value === "Local") {
        showLocal();
    } else if (value === "Foreign") {
        showForeign();
    } else if (value === "InHouse") {
        showInHouse();
    } else if (value === "PostGraduation") {
        showPostGrad();
    } else {
        $("select#ProgramType").prop('selectedIndex', 1);
        showLocal();
    }
}

function resetSelectElement() {
    $("select#ProgramType").prop('selectedIndex', 1);
    showLocal();
}

function showLocal() {
    $(".localInput input").prop('disabled', false);
    hideThem("localInput", ["foreignInput", "inHouseInput", "postGradInput"]);
}

function showForeign() {
    $(".foreignInput input").prop('disabled', false);
    hideThem("foreignInput", ["localInput", "inHouseInput", "postGradInput"]);
}

function showPostGrad() {
    $(".postGradInput input").prop('disabled', false);
    hideThem("postGradInput", ["localInput", "inHouseInput", "foreignInput"]);
}

function showInHouse() {
    $(".inHouseInput input").prop('disabled', false);
    hideThem("inHouseInput", ["localInput", "postGradInput", "foreignInput"]);
}

function hideThem(dontHide, thingsToHide) {

    var elements;
    // get the elements to hide
    for (var i = 0; i < thingsToHide.length; i++) {


        $("." + thingsToHide[i]).each(function (i, obj) {
            if (!$(this).hasClass(dontHide)) {

                $(this).find("input").prop('disabled', true);
            }
        });

    }

}