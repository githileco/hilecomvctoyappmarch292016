var initialLineItems ="";


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
    MessageBox: ko.observable(),
    AddNewLineItem: ko.observable(false),

    SetBackgroundColor: function (currentLineItemData) {
        var rowIndex = this.LineItems.indexOf(currentLineItemData);
        var colorCode = rowIndex % 2 == 0 ? "White" : "WhiteSmoke";
        return colorCode;
    },

    IntializeLineItem: function (datajsn) {
        IntializeLineItem(datajsn);
    },
    EditLineItem: function (currentLineItemData) {

        var currentLineItem = this.LineItems.indexOf(currentLineItemData);

        this.LineItems()[currentLineItem].DisplayMode(false);
        this.LineItems()[currentLineItem].EditMode(true);
        this.LineItems()[currentLineItem].DisplayDeleteEditButtons(false);
        this.LineItems()[currentLineItem].DisplayCancelSaveButtons(true);

    },

    DeleteLineItem: function (currentLineItemData) {

        var currentLineItem = this.LineItems.indexOf(currentLineItemData);

        var productName = this.LineItems()[currentLineItem].ProductName();
        var productID = this.LineItems()[currentLineItem].ProductID();

        ConfirmDeleteLineItem(productID, productName, currentLineItem);

    },

    DeleteLineItemConfirmed: function (currentLineItem) {
        var row = this.LineItems()[currentLineItem];
        this.LineItems.remove(row);
    },

    CancelLineItem: function (currentLineItemData) {

        currentLineItem = this.LineItems.indexOf(currentLineItemData);

        this.LineItems()[currentLineItem].DisplayMode(true);
        this.LineItems()[currentLineItem].EditMode(false);
        this.LineItems()[currentLineItem].DisplayDeleteEditButtons(true);
        this.LineItems()[currentLineItem].DisplayCancelSaveButtons(false);

        this.LineItems()[currentLineItem].Quantity(this.LineItems()[currentLineItem].OriginalQuantity());
        this.LineItems()[currentLineItem].Discount(this.LineItems()[currentLineItem].OriginalDiscount());

    },

    UpdateLineItem: function (currentLineItemData) {
        currentLineItem = this.LineItems.indexOf(currentLineItemData);
        var lineItem = this.LineItems()[currentLineItem];
        UpdateOrderDetail(lineItem, currentLineItem);
    },

    UpdateOrderDetailComplete: function (currentLineItem, discount) {

        this.LineItems()[currentLineItem].DisplayMode(true);
        this.LineItems()[currentLineItem].EditMode(false);
        this.LineItems()[currentLineItem].DisplayDeleteEditButtons(true);
        this.LineItems()[currentLineItem].DisplayCancelSaveButtons(false);

        this.LineItems()[currentLineItem].OriginalQuantity(this.LineItems()[currentLineItem].Quantity());
        this.LineItems()[currentLineItem].OriginalDiscount(discount);
        this.LineItems()[currentLineItem].Discount(discount);

    },
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

function ShowProductInquiryModal() {
    var url = "/Products/BeginProductInquiry";

    $.post(url, null, function (html, textStatus) {
        ShowProductInquiryModalComplete(html);
    });
}

function ShowProductInquiryModalComplete(productInquiryHtml) {
    $("#ProductInquiryModalDiv").html(productInquiryHtml);
    $("#dialog-modal").dialog({
        height: 500,
        width: 900,
        modal: true
    });
    setTimeout("ProductInquiryInitializeGrid()", 1000);
}

function GetProductInformation(productID) {

    $("#dialog-modal").dialog('close');
    $("#ProductInquiryModalDiv").html("");

    var ProductLookup = function () {
        this.ProductID;
    };

    var productLookup = new ProductLookup();
    productLookup.ProductID = productID;

    var url = "/Products/GetProductInformation";

    $.post(url, productLookup, function (results, textStatus) {
        GetProductInformationComplete(results);
    });
}

function GetProductInformationComplete(results) {

    if (results.ReturnStatus == true) {
        $("#ProductID").val(results.ViewModel.Product.ProductIDFormatted);
        $("#ProductName").html(results.ViewModel.Product.ProductName);
        $("#QuantityPerUnit").html(results.ViewModel.Product.QuantityPerUnit);
        $("#UnitPrice").html(results.ViewModel.Product.UnitPriceFormatted);
        $("#Quantity").focus();
    }
    else {
        viewModel.MessageBox(results.MessageBoxView);
    }
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


function ConfirmDeleteLineItem(productID, productName, currentLineItem) {

    $("#DeleteConfirmationText").html("Are you sure you want to delete <b>" + productID + " - " + productName + "</b> from this order?");

    $("#dialog-confirm").dialog({
        resizable: false,
        height: 200,
        width: 600,
        modal: true,
        buttons: {
            "Delete line item": function () {
                DeleteLineItem(productID, productName, currentLineItem);
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

}

function DeleteLineItem(productID, productName, currentLineItem) {

    var orderID = $("#OrderID").val();

    var postData = {
        OrderID: orderID,
        ProductID: productID,
        ProductName: productName,
        RowIndex: currentLineItem
    };

    var url = "/Orders/DeleteOrderDetailLineItem";

    $.post(url, postData, function (data, textStatus) {
        DeleteLineItemComplete(data, textStatus);
    });

}

function DeleteLineItemComplete(results) {

    viewModel.MessageBox(results.MessageBoxView);

    if (results.ReturnStatus == true) {
        viewModel.DeleteLineItemConfirmed(results.RowIndex);
    }

}


function ShowOrderHeader() {
    $("#OrderEdit #OrderID").val($("#OrderID").val());
    $("#OrderEdit").submit();
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
