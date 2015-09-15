
var counter = 0;
var lineItemDisplay = function () {
    this.ImagePath;
    this.ProductID;
    this.ProductName;
    this.Quantity;
    this.UnitPrice;
    this.SupplierID;
    this.QuantityPerUnit;
    this.UnitsInStock;
    this.UnitsOnOrder;
    this.ReorderLevel;
    this.Discontinued;
    this.Description;
    this.DisplayMode;
    this.DisplayCancelSaveButtons; 
};


var newDetailProductLineItem = new lineItemDisplay();


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

    AddLineItem: function (currentLineItemData) {
        currentLineItem = this.LineItems.indexOf(currentLineItemData);
        var lineItem = this.LineItems()[currentLineItem];
        AddProductDetail(lineItem, currentLineItem);
    },
    DetailLineItem: function (currentLineItemData)
    {
        currentLineItem = this.LineItems.indexOf(currentLineItemData);
        var lineItem = this.LineItems()[currentLineItem];
        ShowDetailLineItem(lineItem, currentLineItem);
     },
    BindUIwithViewModel: function (viewModel) {
        ko.applyBindings(viewModel);
    },

    IntializeLineItem:function(datajsn){
        IntializeL(datajsn);
    }
   
    //AddNewProductDetailComplete: function (currentLineItem) {

    //    this.LineItems()[currentLineItem].DisplayMode(true);
    //    this.LineItems()[currentLineItem].DisplayCancelSaveButtons(false);

    //    this.LineItems()[currentLineItem].OriginalQuantity(this.LineItems()[currentLineItem].Quantity());

    //}

};

function CreateLineItem(LineItem) {
     
    var lineItem = new lineItemDisplay();
    lineItem.ImagePath = ko.observable('/Catalog/Images/Thumbs/'+LineItem.ImagePath+'.png');
    lineItem.ProductID = ko.observable(LineItem.ProductID);
    lineItem.ProductName = ko.observable(LineItem.ProductName);
    lineItem.Quantity = ko.observable(LineItem.Quantity);
    lineItem.SupplierID =ko.observable( LineItem.SupplierID);
    lineItem.CategoryID = ko.observable(LineItem.CategoryID);
    lineItem.QuantityPerUnit =ko.observable( LineItem.QuantityPerUnit);
    lineItem.UnitsInStock = ko.observable(LineItem.UnitsInStock);
    lineItem.ReorderLevel = ko.observable(LineItem.ReorderLevel);
    lineItem.Discontinued = ko.observable(LineItem.Discontinued);
    lineItem.UnitsOnOrder = ko.observable(LineItem.UnitsOnOrder);
    lineItem.UnitPrice = ko.observable(LineItem.UnitPrice);
    lineItem.Description = ko.observable(LineItem.Description);
    lineItem.EditMode = ko.observable(true);
    lineItem.DisplayMode = ko.observable(true);
    lineItem.DisplayCancelSaveButtons = ko.observable(true);
    return lineItem;

}
$("#addtocart").click(function () {
    AddDetail();
});
function IntializeL(viewModeljson)
{
    initialLineItems = viewModeljson;
    for (i = 0; i < initialLineItems.length; i++) {
        var newLineItem = CreateLineItem(initialLineItems[i]);
        viewModel.LineItems.push(newLineItem);
    }
}
function ShowAddLineItem() {
    viewModel.AddNewLineItem(true);
    
}

function LineItem() {

    this.ProductID;
    this.Quantity;
    this.RowIndex;
}

function GetProductInformationComplete(results)
{
    if (results.ReturnStatus == true)
    {
        counter = counter + 1;
        $('#cart-status').text('Cart (' + counter + ')');
    }
    else {
        viewModel.MessageBox(results.MessageBoxView);
    }
   // $("#DeleteConfirmationText").html(x);
}
function AddProductDetail(currentLineItem, rowIndex) {

    var newProductLineItem = new LineItem();

   // newProductLineItem.CartID = $("#CartID").val();
    newProductLineItem.ProductID = currentLineItem.ProductID();
    newProductLineItem.ProductName = currentLineItem.ProductName();
    newProductLineItem.Quantity = currentLineItem.Quantity();
    newProductLineItem.UnitPrice = currentLineItem.UnitPrice();
   // newProductLineItem.ImagePath = currentLineItem.ImagePath();
    // newProductLineItem.RowIndex = rowIndex;
   // $("#DeleteConfirmationText").html(newProductLineItem.UnitPrice);
    var url = "/Carts/AddNewProductCartDetailLineItem";

    $.post(url, newProductLineItem, function (results, textStatus) {
        GetProductInformationComplete(results);
    });

}
function ShowDetailLineItem(currentLineItem, rowIndex)
{
    $('#PeepsForm').visible = true;
    // newProductLineItem.CartID = $("#CartID").val();
    newDetailProductLineItem.ProductID = currentLineItem.ProductID();
    newDetailProductLineItem.ProductName = currentLineItem.ProductName();
    newDetailProductLineItem.Quantity = currentLineItem.Quantity();
    newDetailProductLineItem.UnitPrice = currentLineItem.UnitPrice();
    newDetailProductLineItem.ImagePath = currentLineItem.ImagePath();
   // $("#DeleteConfirmationText").html(newDetailProductLineItem.ImagePath);
    $("#ProductID").html(currentLineItem.ProductID());
    $("#ProductName").html(currentLineItem.ProductName());
    $("#UnitPrice").html(currentLineItem.UnitPrice());
    $("#SupplierID").html(currentLineItem.SupplierID());
    $("#CategoryID").html(currentLineItem.CategoryID());
    $("#QuantityPerUnit").html(currentLineItem.QuantityPerUnit());
    $("#UnitsInStock").html(currentLineItem.UnitsInStock());
    $("#UnitsOnOrder").html(currentLineItem.UnitsOnOrder());
    $("#ReorderLevel").html(currentLineItem.ReorderLevel());
    $("#Discontinued").html(currentLineItem.Discontinued());
    $("#Description").html(currentLineItem.Description());
    // newProductLineItem.RowIndex = rowIndex;



   
}
function AddDetail() {
    var newDetailProductLineItem2 = new lineItemDisplay();

    newDetailProductLineItem2.ProductID = newDetailProductLineItem.ProductID;
    newDetailProductLineItem2.ProductName = newDetailProductLineItem.ProductName;
    newDetailProductLineItem2.UnitPrice = newDetailProductLineItem.UnitPrice;
    newDetailProductLineItem2.Quantity = $("#txtNewQuantity").val();
    var url = "/Carts/AddNewProductCartDetailLineItem";

    $.post(url, newDetailProductLineItem2, function (results, textStatus) {
        GetProductInformationComplete(results);
    });
}
function AddNewProductDetailComplete(results) 
{
    if (results.ReturnStatus == true) 
    {
        
        viewModel.UpdateCartDetailComplete(results.RowIndex);
    }

    viewModel.MessageBox(results.MessageBoxView);

}


function GetCartCountComplete(results)
{
    counter = results.TotalCatCount;

    $('#cart-status').text('Cart (' + counter + ')');
}

$(document).ready(function () {
   // Product.BindUIwithViewModel(Product.ViewModel);
    //Product.RegisterUIEventHandlers();
    // Product.IntializeLineItem(Product.ViewModel);
    var InitialiceCartCount = 0;
    var url = "/Carts/GetCartCount";

    $.post(url, InitialiceCartCount, function (results, textStatus) {
        GetCartCountComplete(results);
    });

    viewModel.RegisterUIEventHandlers();
    viewModel.BindUIwithViewModel(viewModel);
   
    
 });

