$(window).ready(function () {

    tableEntryCount();


});

/**
 * Redirect the Page with Variables to load
 * table Entries
 */
function tableEntryCount() {
    $('.entryCountSelect').on('change', function () {
        
        var url = $(this).val();

        if (url) {
            window.location = url;
        }
        return false;
    });
}