var KeepAlive = function () {
    $.get(urlRoute, function (data) {

    });
};

setInterval(KeepAlive, 200000);

function select2Focus() {
    var select2 = $(this).closest('.select2').prev('select');

    setTimeout(function () {
        if (!select2.opened()) {
            select2.open();
        }
    }, 0);

    //$(this).closest('.select2').prev('select').select2('open');

}

$(function () {

    $(".select2-selection").on("focus", function () {
        $(this).parent().parent().prev().select2("open");
    });

    $('table').addClass('table-condensed');

});


function setPlaceHolderToSelect2(ispartial) {

    var select2InPage = $('*[data-plugin="select2"]');//.val('').trigger('change');


    select2InPage.each(function (i, e) {
        var $this = $(e);
        if ($this.data('isedit') != null)
            if ($this.data('isedit') == false) {
                $this.val('').trigger('change');
            }
    });
}

// Global function to replace partialview's html
// You need to add the following data-attributes to @Html.DropDownList to get it work
// data-url : Destination request
// data-partial : Div's id to replace the result data
function onChangeDropDownFillPartial(dropDown) {

    var $this = $(dropDown);
    var value = $this.val();
    var url = $this.data('url');
    var partial = $('#' + $this.data('partial'));

    console.log('Fetching data from: ' + url);
    $.ajax({
        cache: false,
        method: 'POST',
        url: url,
        data: { 'SelectID': value },
        success: function (data) {
            partial.empty();
            partial.append(data);
        }
    });
}

// Global function to replace partialview's html sending data from a <form> (Sames logic as modal helper)
// You need to add the following data-attributes to <button> to get it work
// data-form: The form's id
// data-partial : Div's id to replace the result data
// FYI:
// model: are the form's inputs, it will be serialized and sending to the action
// button must be <button type="button" ... />
function peudoPostFillPartial(button) {

    var $this = $(button);
    var form = $('#' + $this.data('form'));
    var model = form.serialize();
    var url = form.attr('action');
    var partial = $('#' + $this.data('partial'));

    $.ajax({
        cache: false,
        method: 'POST',
        url: url,
        data: model,
        success: function (data) {
            partial.empty();
            partial.append(data);
        }
    });
}

// Global function to fill Select2 from other Select2 when the value changes
// You need to add the following data-attributes to @Html.DropDownList to get it work
// data-url: The url of the action
// data-target: The select2 to fil when the data comes
function onChangeSelect2Cascade(select2) {

    var $this = $(select2);
    var value = $this.val();
    var url = $this.data('url');
    var select2target = $('#' + $this.data('target'));

    $.ajax({
        cache: false,
        method: 'POST',
        url: url,
        data: { 'SelectedID': value },
        success: function (data) {
            select2target.select2({
                data: data,
                language: 'es',
                placeholder: "[ -- Seleccione -- ]",
                allowClear: true
            });
        }
    });
}
//TODO:
//http://gist.github.com/ajaxray/187e7c9a00666a7ffff52a8a69b8bf31
//Cascade select2 helper (no hay internet :/ )
var select2Cascade = (function (window, $) {

})

function ShowMessage(type, message) {
    toastr.options.closeButton = true;
    //toastr.options.timeOut = 0;
    //toastr.options.extendedTimeOut = 0;
    if (type === 'Success')
        toastr.success(message);
    else if (type === 'Error' || type === 'error' )
        toastr.error(message);
    else if (type === 'Warning')
        toastr.warning(message);
    else
        toastr.info(message);
}