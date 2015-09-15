var initialLineItems = "";

var counter = 0;
var lineItemDisplay = function () {

    this.ProductID;
    this.ProductName;
    this.Quantity;
    this.UnitPrice;
    this.QuantityPerUnit;
    this.Discount;
    this.OriginalQuantity;
    this.OriginalDiscount;
    this.EditMode;
    this.DisplayMode;
    this.DisplayDeleteEditButtons;
    this.DisplayCancelSaveButtons;
};

// Overall viewmodel for this screen, along with initial state
var viewModel = {

    LineItems: ko.observableArray(),
    AddNewLineItem: ko.observable(false),
    EditFields: ko.observable(false),
   // ReadOnlyMode: ko.observable(false),
    MessageBox: ko.observable(),
    SetBackgroundColor: function (currentLineItemData) {
        var rowIndex = this.LineItems.indexOf(currentLineItemData);
        var colorCode = rowIndex % 2 == 0 ? "White" : "WhiteSmoke";
        return colorCode;
    },
    SelectedShipVia: ko.observable($("#OriginalShipVia").val()),
    Shippers: ko.observableArray(([
            new lineItemDisplay("UK", 65000000),
            new lineItemDisplay("USA", 320000000),
            new lineItemDisplay("Sweden", 29000000)
    ])),
    IntializeLineItem: function (datajsn) {
        IntializeLineItem(datajsn);
    },
    
    BindUIwithViewModel: function (viewModel) {
        ko.applyBindings(viewModel);
    },
    OrderID: ko.observable($("#OrderID").val()),
    RegisterUIEventHandlers: function () {

        $('#save').click(function (e) {

            // Check whether the form is valid. Note: Remove this check, if you are not using HTML5
            //  viewModel.MessageBox(Product.ViewModel);
            // jsondatax = initialLineItems.length;

        });

    }

}
/*
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
*/
function IntializeLineItem(viewModeljson) {

    initialLineItems = viewModeljson;
    for (i = 0; i < initialLineItems.length; i++) {
        var newLineItem = CreateLineItem(initialLineItems[i]);
        viewModel.LineItems.push(newLineItem);

    }
}

//$("#OrderNo").html(initialLineItems[i - 1].OrderID);

function CreateLineItem(LineItem) {

    var lineItem = new lineItemDisplay();

    lineItem.ProductID = ko.observable(LineItem.ProductID);
    lineItem.ProductName = ko.observable(LineItem.ProductName);
    lineItem.Quantity = ko.observable(LineItem.Quantity);
    lineItem.OriginalQuantity = ko.observable(LineItem.Quantity);
    lineItem.OriginalDiscount = ko.observable(LineItem.Discount);
    lineItem.UnitPrice = ko.observable(LineItem.UnitPrice);
    lineItem.QuantityPerUnit = ko.observable(LineItem.QuantityPerUnit);
    lineItem.Discount = ko.observable(LineItem.Discount);
    lineItem.BackgroundColor = ko.observable(LineItem.BackgroundColor);
    lineItem.EditMode = ko.observable(false);
    lineItem.DisplayMode = ko.observable(true);
    lineItem.DisplayDeleteEditButtons = ko.observable(true);
    lineItem.DisplayCancelSaveButtons = ko.observable(false);

    return lineItem;

}



function LineItem() {
    this.OrderID;
    this.ProductID;
    this.Quantity;
    this.Discount;
    this.RowIndex;
};

function ShowAddLineItem() {
    viewModel.AddNewLineItem(true);
}

function AddNewLineItem() {
    var newLineItem = new LineItem();
    newLineItem.OrderID = $("#OrderID").val();
    newLineItem.ProductID = $("#ProductID").val();
    newLineItem.Quantity = $("#Quantity").val();
    newLineItem.Discount = $("#Discount").val();

    var url = "/Orders/AddOrderDetailLineItem";

    $.post(url, newLineItem, function (results, textStatus) {
        AddNewLineItemComplete(results);
    });

}

function AddNewLineItemComplete(results) {
    if (results.ReturnStatus == true) {

        var lineItem = new lineItemDisplay();

        lineItem.ProductID = results.ViewModel.OrderLineItem.OrderDetails.ProductIDFormatted;
        lineItem.ProductName = results.ViewModel.OrderLineItem.Products.ProductName;
        lineItem.Quantity = results.ViewModel.OrderLineItem.OrderDetails.Quantity;
        lineItem.UnitPrice = results.ViewModel.OrderLineItem.OrderDetails.UnitPriceFormatted;
        lineItem.QuantityPerUnit = results.ViewModel.OrderLineItem.Products.QuantityPerUnit;
        lineItem.Discount = results.ViewModel.OrderLineItem.OrderDetails.DiscountFormatted;

        var newLineItem = CreateLineItem(lineItem);
        viewModel.LineItems.push(newLineItem);

        $("#ProductID").val("");
        $("#ProductName").html("");
        $("#Quantity").val("");
        $("#Discount").val("");
        $("#UnitPrice").html("");
        $("#QuantityPerUnit").html("");

    }

    viewModel.MessageBox(results.MessageBoxView);

}

function UpdateOrderDetail(currentLineItem, rowIndex) {

    var updateLineItem = new LineItem();

    updateLineItem.OrderID = $("#OrderID").val();
    updateLineItem.ProductID = currentLineItem.ProductID(),
    updateLineItem.Quantity = currentLineItem.Quantity();
    updateLineItem.Discount = currentLineItem.Discount();
    updateLineItem.RowIndex = rowIndex;

    var url = "/Orders/UpdateOrderDetailLineItem";

    $.post(url, updateLineItem, function (results, textStatus) {
        UpdateOrderDetailComplete(results);
    });

}

function UpdateOrderDetailComplete(results) {
    if (results.ReturnStatus == true) {
        discount = results.ViewModel.OrderLineItem.OrderDetails.DiscountFormatted;
        viewModel.UpdateOrderDetailComplete(results.RowIndex, discount);
    }

    viewModel.MessageBox(results.MessageBoxView);

}





function CheckoutConfirm() {
    $("#CheckoutConfirm").submit();
}

function ProductIDEntered(element, e) {

    var key;

    if (window.event)
        key = window.event.keyCode;     //IE
    else
        key = e.which;     //firefox

    if (key == 13) {
        viewModel.MessageBox("");
        event.keyCode = 0;
        var productID = $("#ProductID").val();
        GetProductInformation(productID)
    }

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

    viewModel.RegisterUIEventHandlers();
    viewModel.BindUIwithViewModel(viewModel);
    
});
