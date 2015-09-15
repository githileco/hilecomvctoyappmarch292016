
//shippers = eval($("#Shippers").text());
//if (shippers)
//shippers = $.parseJSON($("#Shippers").text());

// Overall viewmodel for this screen, along with initial state
var shippers = "";
var viewModel = {
    IntializeLineItem: function (shippers) {
        IntializeLineItem(shippers);
    },
    EditFields: ko.observable(false),
    ReadOnlyMode: ko.observable(false),
    DisplayCheckoutConfirm: ko.observable(false),
    DisplayCreateOrderButton: ko.observable(false),
    DisplayEditOrderButton: ko.observable(false),
    DisplayUpdateOrderButton: ko.observable(false),
    DisplayOrderDetailsButton: ko.observable(false),
    DisplayCancelChangesButton: ko.observable(true),

    SelectedShipVia: ko.observable($("#OriginalShipVia").val()),
    Shippers: ko.observableArray(([
            new lineItemDisplay("UK", 65000000),
            new lineItemDisplay("USA", 320000000),
            new lineItemDisplay("Sweden", 29000000)
    ])),

    OrderID: ko.observable($("#OrderID").val()),
    ShipperName: ko.observable($("#ShipperName").val()),
    CustomerID: ko.observable($("#CustomerID").val()),

    OriginalShipName: ko.observable($("#OriginalShipName").val()),
    OriginalShipAddress: ko.observable($("#OriginalShipAddress").val()),
    OriginalShipCity: ko.observable($("#OriginalShipCity").val()),
    OriginalShipRegion: ko.observable($("#OriginalShipRegion").val()),
    OriginalShipPostalCode: ko.observable($("#OriginalShipPostalCode").val()),
    OriginalShipCountry: ko.observable($("#OriginalShipCountry").val()),
    OriginalRequiredDate: ko.observable($("#OriginalRequiredDate").val()),
    OriginalShipVia: ko.observable($("#OriginalShipVia").val()),

    ShipName: ko.observable($("#OriginalShipName").val()),
    ShipAddress: ko.observable($("#OriginalShipAddress").val()),
    ShipCity: ko.observable($("#OriginalShipCity").val()),
    ShipRegion: ko.observable($("#OriginalShipRegion").val()),
    ShipPostalCode: ko.observable($("#OriginalShipPostalCode").val()),
    ShipCountry: ko.observable($("#OriginalShipCountry").val()),
    RequiredDate: ko.observable($("#OriginalRequiredDate").val()),

    MessageBox: ko.observable(""),
    BindUIwithViewModel: function (viewModel) {
    ko.applyBindings(viewModel);
},
RegisterUIEventHandlers: function () {

    $('#save').click(function (e) {

        // Check whether the form is valid. Note: Remove this check, if you are not using HTML5
        //  viewModel.MessageBox(Product.ViewModel);
        // jsondatax = initialLineItems.length;

    });

}

}


$("#btnCheckoutConfirm").click(function () {
    CheckoutConfirm();
    Session["userCheckoutCompleted"]
});
$("#btnCreateOrder").click(function () {
    CreateOrder();
});

$("#btnUpdateOrder").click(function () {
    UpdateOrder();
});

$("#btnOrderDetails").click(function () {
    OrderDetails();
});

$("#btnCancelChanges").click(function () {

    viewModel.ShipName(viewModel.OriginalShipName());
    viewModel.ShipAddress(viewModel.OriginalShipAddress());
    viewModel.ShipCity(viewModel.OriginalShipCity());
    viewModel.ShipRegion(viewModel.OriginalShipRegion());
    viewModel.ShipPostalCode(viewModel.OriginalShipPostalCode());
    viewModel.ShipCountry(viewModel.OriginalShipCountry());
    viewModel.RequiredDate(viewModel.OriginalRequiredDate());
    viewModel.SelectedShipVia(viewModel.OriginalShipVia());

    viewModel.DisplayEditOrderButton(true);
    viewModel.DisplayUpdateOrderButton(false);
    viewModel.DisplayOrderDetailsButton(true);
    viewModel.DisplayCancelChangesButton(false);
    viewModel.EditFields(false);
    viewModel.ReadOnlyMode(true);

});

$("#btnEditOrder").click(function () {
    viewModel.DisplayEditOrderButton(false);
    viewModel.DisplayUpdateOrderButton(true);
    viewModel.DisplayOrderDetailsButton(false);
    viewModel.DisplayCancelChangesButton(true);
    viewModel.EditFields(true);
    viewModel.ReadOnlyMode(false);
});


if (viewModel.OrderID() == "0" || viewModel.OrderID() == "") {
    // order entry mode
    viewModel.DisplayEditOrderButton(false);
    viewModel.DisplayUpdateOrderButton(false);
    viewModel.DisplayOrderDetailsButton(false);
    viewModel.DisplayCancelChangesButton(false);
    viewModel.DisplayCreateOrderButton(true);
    viewModel.EditFields(true);
    viewModel.ReadOnlyMode(false); 
    viewModel.OrderID("");
}
else {
    // order edit mode
    viewModel.DisplayEditOrderButton(true);
    viewModel.DisplayUpdateOrderButton(false);

    viewModel.DisplayCheckoutConfirm(true);
    viewModel.DisplayOrderDetailsButton(true);
    viewModel.DisplayCancelChangesButton(false);
    viewModel.DisplayCreateOrderButton(false);
    viewModel.EditFields(false);
    viewModel.ReadOnlyMode(true);
}
function IntializeLineItem(ishippers) {

    shippers = ishippers;
}
function ShippingInformation() {
    this.OrderID;
    this.CustomerID;
    this.ShipName;
    this.ShipAddress;
    this.ShipCity;
    this.ShipRegion;
    this.ShipPostalCode;
    this.ShipCountry;
    this.RequiredDate;
    this.Shipper;
};

function CreateOrder() {

    var shippingInformation = new ShippingInformation();

    shippingInformation.CustomerID = viewModel.CustomerID();
    shippingInformation.ShipName = viewModel.ShipName();
    shippingInformation.ShipAddress = viewModel.ShipAddress();
    shippingInformation.ShipCity = viewModel.ShipCity();
    shippingInformation.ShipRegion = viewModel.ShipRegion();
    shippingInformation.ShipPostalCode = viewModel.ShipPostalCode();
    shippingInformation.ShipCountry = viewModel.ShipCountry();
    shippingInformation.RequiredDate = viewModel.RequiredDate();
    shippingInformation.Shipper = viewModel.SelectedShipVia();

    var url = "/Orders/CreateOrder";

    $(':input').removeClass('validation-error');

    $.post(url, shippingInformation, function (data, textStatus) {
        CreateOrderComplete(data);
    });

}


function UpdateOrder() {

    var shippingInformation = new ShippingInformation();

    shippingInformation.OrderID = viewModel.OrderID();
    shippingInformation.CustomerID = viewModel.CustomerID();
    shippingInformation.ShipName = viewModel.ShipName();
    shippingInformation.ShipAddress = viewModel.ShipAddress();
    shippingInformation.ShipCity = viewModel.ShipCity();
    shippingInformation.ShipRegion = viewModel.ShipRegion();
    shippingInformation.ShipPostalCode = viewModel.ShipPostalCode();
    shippingInformation.ShipCountry = viewModel.ShipCountry();
    shippingInformation.RequiredDate = viewModel.RequiredDate();
    shippingInformation.Shipper = viewModel.SelectedShipVia();

    var url = "/Orders/UpdateOrder";

    $(':input').removeClass('validation-error');

    $.post(url, shippingInformation, function (data, textStatus) {
        UpdateOrderComplete(data);
    });

}

function UpdateOrderComplete(result) {

    if (result.ReturnStatus == true) {

        viewModel.MessageBox(result.MessageBoxView);
        viewModel.OrderID(result.ViewModel.Order.OrderID);
        viewModel.ShipperName(result.ViewModel.Order.ShipperName);

        viewModel.DisplayEditOrderButton(true);
        viewModel.DisplayUpdateOrderButton(false);
        viewModel.DisplayOrderDetailsButton(true);
        viewModel.DisplayCancelChangesButton(false);
        viewModel.DisplayCreateOrderButton(false);

        viewModel.EditFields(false);
        viewModel.ReadOnlyMode(true);

        viewModel.OriginalShipName(result.ViewModel.Order.ShipName);
        viewModel.OriginalShipAddress(result.ViewModel.Order.ShipAddress);
        viewModel.OriginalShipCity(result.ViewModel.Order.ShipCity);
        viewModel.OriginalShipRegion(result.ViewModel.Order.ShipRegion);
        viewModel.OriginalShipPostalCode(result.ViewModel.Order.ShipPostalCode);
        viewModel.OriginalShipCountry(result.ViewModel.Order.ShipCountry);
        viewModel.OriginalRequiredDate(result.ViewModel.Order.RequiredDateFormatted);
        viewModel.OriginalShipVia(viewModel.SelectedShipVia());

    }
    else {
        viewModel.MessageBox(result.MessageBoxView);
    }

    for (var val in result.ValidationErrors) {
        var element = "#" + val;
        $(element).addClass('validation-error');
    }

}


function CreateOrderComplete(result) {

    if (result.ReturnStatus == true) {

        viewModel.MessageBox(result.MessageBoxView);
        viewModel.OrderID(result.ViewModel.Order.OrderID);
        viewModel.ShipperName(result.ViewModel.Order.ShipperName);

        viewModel.DisplayEditOrderButton(true);
        viewModel.DisplayUpdateOrderButton(false);
        viewModel.DisplayOrderDetailsButton(true);
        viewModel.DisplayCancelChangesButton(false);
        viewModel.DisplayCreateOrderButton(false);

        viewModel.EditFields(false);
        viewModel.ReadOnlyMode(true);

        viewModel.OriginalShipName(result.ViewModel.Order.ShipName);
        viewModel.OriginalShipAddress(result.ViewModel.Order.ShipAddress);
        viewModel.OriginalShipCity(result.ViewModel.Order.ShipCity);
        viewModel.OriginalShipRegion(result.ViewModel.Order.ShipRegion);
        viewModel.OriginalShipPostalCode(result.ViewModel.Order.ShipPostalCode);
        viewModel.OriginalShipCountry(result.ViewModel.Order.ShipCountry);
        viewModel.OriginalRequiredDate(result.ViewModel.Order.RequiredDateFormatted);
        viewModel.OriginalShipVia(viewModel.SelectedShipVia());

    }
    else {
        viewModel.MessageBox(result.MessageBoxView);
    }

    for (var val in result.ValidationErrors) {
        var element = "#" + val;
        $(element).addClass('validation-error');
    }

}

function OrderDetails() {
    $("#OrderDetails #OrderID").val(viewModel.OrderID());
    $("#OrderDetails").submit();
}

function CheckoutConfirm()
{
    $("#CheckoutConfirm").submit();
}



$(document).ready(function () {

    /*$("#RequiredDate").datepicker({
        showOn: "button",
        buttonImage: '@Url.Content("~/Content/images/icon-calendar.gif")',
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true
    });*/

    viewModel.RegisterUIEventHandlers();
    viewModel.BindUIwithViewModel(viewModel);
});

