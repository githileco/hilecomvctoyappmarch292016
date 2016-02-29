var counter = 0;
function onEditCompleted(results) {
    if (results.ReturnStatus == true) {
        counter = counter + 1;
        $('#cart-status').text('Cart (' + counter + ')');
    };
}
function GetCartCountComplete(results) {
    counter = results.TotalCatCount;

    $('#cart-status').text('Cart (' + counter + ')');
}
$(document).ready(function () {

    var InitialiceCartCount = 0;
    var url = "/Carts/GetCartCount";

    $.post(url, InitialiceCartCount, function (results, textStatus) {
        GetCartCountComplete(results);
    });

    //viewModel.RegisterUIEventHandlers();


});
