
var counter = 0;
var lineItemDisplay = function () {

    this.ProductID; 
    this.ProductName;
    this.Quantity;
    this.UnitPrice;
    this.RecordId;
    this.DisplayMode;
    this.DisplayCancelSaveButtons;
};


var viewModel = {

    LineItems: ko.observableArray(),
    MessageBox: ko.observable(),
    RegisterUIEventHandlers: function () {

        $('#save').click(function (e) {

            // Check whether the form is valid. Note: Remove this check, if you are not using HTML5
            //  viewModel.MessageBox(Product.ViewModel);
            // jsondatax = initialLineItems.length;

        });

    },
    IntializeLineItem: function (datajsn) {
        IntializeL(datajsn);
    },
    DeleteLineItem: function (currentLineItemData) {
        

        var currentLineItem = this.LineItems.indexOf(currentLineItemData);

        var productName = this.LineItems()[currentLineItem].ProductName();
        var productID = this.LineItems()[currentLineItem].ProductID();
        var RecordId = this.LineItems()[currentLineItem].RecordId();

        ConfirmDeleteLineItem(productID, productName,RecordId, currentLineItem);

    },
    DeleteLineItemConfirmed: function (currentLineItem) {
        var row = this.LineItems()[currentLineItem];
        this.LineItems.remove(row);
    },
    UpdateLineItem: function (currentLineItemData) {
        currentLineItem = this.LineItems.indexOf(currentLineItemData);
        var lineItem = this.LineItems()[currentLineItem];
        UpdateCartDetail(lineItem, currentLineItem); 
    },
    
   /* UpdateCartDetailComplete: function (currentLineItem, discount) {

        this.LineItems()[currentLineItem].DisplayMode(true);
        this.LineItems()[currentLineItem].EditMode(false);
        this.LineItems()[currentLineItem].DisplayDeleteEditButtons(true);
        this.LineItems()[currentLineItem].DisplayCancelSaveButtons(false);

        this.LineItems()[currentLineItem].OriginalQuantity(this.LineItems()[currentLineItem].Quantity());
        this.LineItems()[currentLineItem].OriginalDiscount(discount);
        this.LineItems()[currentLineItem].Discount(discount);

    },*/
    BindUIwithViewModel: function (viewModel) {
        ko.applyBindings(viewModel);
    }

};

function CreateLineItem(LineItem) {

    var lineItem = new lineItemDisplay();

    lineItem.ProductID = ko.observable(LineItem.ProductID);
    lineItem.ProductName = ko.observable(LineItem.ProductName);
    lineItem.Quantity = ko.observable(LineItem.Quantity);
    lineItem.UnitPrice = ko.observable(LineItem.UnitPrice);
    lineItem.RecordId = ko.observable(LineItem.RecordId);

    lineItem.EditMode = ko.observable(true);
    lineItem.DisplayMode = ko.observable(true);
    lineItem.DisplayDeleteEditButtons = ko.observable(true);
    return lineItem;

}

function IntializeL(viewModeljson) {

    initialLineItems = viewModeljson;
    for (i = 0; i < initialLineItems.length; i++) {
        var newLineItem = CreateLineItem(initialLineItems[i]);
        viewModel.LineItems.push(newLineItem);
       
    }
}


function LineItem() {

    this.ProductID;
    this.Quantity;
    this.RowIndex;
}


function UpdateCartDetail(currentLineItem, rowIndex)
{
    var updateLineItem = new LineItem();
    updateLineItem.ProductID = currentLineItem.ProductID();
    
    updateLineItem.Quantity = currentLineItem.Quantity();
    

    var url = "/Carts/UpdateCartDetailLineItem";

    $.post(url, updateLineItem, function (results, textStatus) {
        UpdateCartDetailComplete(results);
    });
}
function UpdateCartDetailComplete(results) {
    /*if (results.ReturnStatus == true) {
        discount = results.ViewModel.OrderLineItem.OrderDetails.DiscountFormatted;
        viewModel.UpdateCartDetailComplete(results.RowIndex, discount);
    }
    */
  //  viewModel.MessageBox(results.ReturnStatus);
    $("#TotalCarts").html(results.xtotal);
   // $("#DeleteConfirmationText2").html(results.ReturnStatus);

   
}

function ConfirmDeleteLineItem(productID, productName,RecordId, currentLineItem) {

    $("#DeleteConfirmationText").html("Are you sure you want to delete <b>" + productID + " - " + productName + "</b> from this order?");

   
    DeleteLineItem(productID, productName, RecordId, currentLineItem);
}
function DeleteLineItem(productID, productName,recordId, currentLineItem) {

   // var orderID = $("#OrderID").val();

    var postData = {
        RecordId:recordId,
        ProductID: productID,
        ProductName: productName,
        RowIndex: currentLineItem
    };

    var url = "/Carts/DeleteCartDetailLineItem";

    $.post(url, postData, function (data, textStatus) {
        DeleteLineItemComplete(data, textStatus);
    });

}
function ReadCartCount(readcounter) {
    counter = readcounter;
}
function DeleteLineItemComplete(results) {

    //viewModel.MessageBox(results.MessageBoxView);

    if (results.ReturnStatus == true) {
        counter = counter - 1;
        $('#cart-status').text('Cart (' + results.TotalCartCounts + ')');
        $("#TotalCarts").html(results.xtotal);

        viewModel.DeleteLineItemConfirmed(results.RowIndex);
    }

}
//function ShowCheckoutComplete(results) {

//    //viewModel.MessageBox(results.MessageBoxView);

//    var x = results;

//}
//function ShowCheckoutHeader() {
//    //$("#CartEdit #CartID").val($("#CartID").val());
//    $("#CheckoutEdit").submit();
//    //var url = "/Checkout/CheckoutStart";
//    ////var postData = 0;

//    //$.post(url);

//}
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

