var lastGirdHeight;
var urlDeleteFrefix = "";
var urlHideFrefix = "";
var urlShowFrefix = "";
var imageLoading = "Đang tải dữ liệu...";
var urlLists = '';
var urlForm = '';
var urlFormApprove = '';
var urlFormGroup = '';
var urlPostAction = '';
var urlPostActionGroup = '';
var urlSort = '';
var urlSortGroup = '';
var urlView = '';
var urlViewGroup = '';
var formHeight = 'auto';
var formWidth = '600';
var urlFormReset = '';
var urlFormPermission = '';




String.prototype.replaceAll = function (
            strTarget,
            strSubString
        ) {
    var strText = this;
    var intIndexOfMatch = strText.indexOf(strTarget);
    while (intIndexOfMatch != -1) {
        strText = strText.replace(strTarget, strSubString);
        intIndexOfMatch = strText.indexOf(strTarget);
    }
    return (strText);
};
(function ($) {
    $.fn.mySerialize = function () {
        var returning = '';
        var indexOfKeyValue = 0;
        var startValue = '';
        var endValue = '';
        //Dùng cho select, multiSelect đều đc
        $('select, input.group:checked', this).each(function () {
            if (this.value != 'null') {
                var relValue = $(this).attr('rel');
                indexOfKeyValue = returning.indexOf(this.name.replace(relValue, '') + '=') + ((this.name.replace(relValue, '') + '=').length); //Đoạn bắt đầu có 'name='
                if (indexOfKeyValue >= ((this.name.replace(relValue, '') + '=').length)) { //Nếu tồn tại key rồi
                    if (this.value.toString() != '' && this.value.toString() != 'null') { //nếu có giá trị
                        startValue = returning.substring(0, indexOfKeyValue); //Đoạn đầu
                        endValue = returning.substring(indexOfKeyValue); //Đoạn cuối
                        returning = startValue + this.value + ',' + endValue; //Đoạn đầu + giá trị mới + đoạn giữa
                    }
                }
                else {
                    returning += this.name.replace(relValue, '') + '=' + $(this).val() + '&';
                }
            }
        });

        //Checkbox = false;
        $('input[type=checkbox]:not(:checked)', this).not('.group').each(function () {
            this.value = 'false';
            returning += this.name + '=false&';
        });

        //checkbox = true
        $('input[type=checkbox]:checked', this).not('.group').each(function () {
            this.value = 'true';
        });

        //input và textarea
        returning += $('input, textarea', this).not('.group').serialize();

        return returning;

    };
})(jQuery);

function initAjaxLoad(urlListLoad, container) {
    $.address.unbind().change(function (event) {
        var urlTransform = urlListLoad;
        var urlHistory = event.value;
        if (urlHistory.length > 0) {
            urlHistory = urlHistory.substring(1, urlHistory.length);
            if (urlTransform.indexOf('?') > 0)
                urlTransform = urlTransform + '&' + urlHistory;
            else
                urlTransform = urlTransform + '?' + urlHistory;
        }
        $(container).html(imageLoading);
        $.post(urlTransform, function (data) {
            $(container).html(data);
        });
    });
}

function changeHashValue(key, value, source) {
    value = encodeURIComponent(value);
    var currentLink = source.substring(1);
    var returnLink = '#';
    var exits = false;
    if (currentLink.indexOf('&') > 0) { // lớn hơn 1
        var tempLink = currentLink.split('&');
        for (idx = 0; idx < tempLink.length; idx++) {
            if (key == tempLink[idx].split('=')[0]) { //check Exits
                returnLink += key + '=' + value;
                exits = true;
            }
            else {
                returnLink += tempLink[idx];
            }
            if (idx < tempLink.length - 1)
                returnLink += '&';
        }
        if (!exits)
            returnLink += '&' + key + '=' + value;
    } else if (currentLink.indexOf('=') > 0) { //Chỉ 1
        returnLink = '#' + currentLink + '&' + key + '=' + value;
    }
    else
        returnLink = '#' + key + '=' + value;
    return returnLink;
}

//Chuyển trang với value mới
function changeHashUrl(key, value) {
    var currentLink = $.address.value();
    return changeHashValue(key, value, currentLink);
}


function registerGridView(selector) {
    //Đổi màu row
    $(selector + " .gridView tr").each(function (index) {
        if (index % 2 == 0)
            $(this).addClass("odd");
    });

    //Sắp xếp các cột
    $(selector + " .gridView th a").each(function (idx) {
        var link = $(this).attr("href");
        link = link.substring(1, link.length);
        if ($.address.value().indexOf(link) > 0) {
            if ($.address.value().indexOf('FieldOption=1') > 0) {
                $(this).addClass('desc');
                $(this).attr("href", '#' + link + '&FieldOption=0');
            }
            else {
                $(this).addClass('asc');
                $(this).attr("href", '#' + link + '&FieldOption=1');
            }
        }
    });

    //khi người dùng click trên 1 row
    $(selector + " .gridView tr").not("first").click(function () {
        $(selector + " .gridView tr").removeClass("hightlight");
        $(this).addClass("hightlight");
    });

    //checkall
    $(selector + ' .checkAll').click(function () {

        var selectQuery = selector + " input.check[type='checkbox']";
        if ($(this).val() != '')
            selectQuery = selector + " #" + $(this).val() + " input.check[type='checkbox']";
        $(selectQuery).attr('checked', $(this).is(':checked'));
    });

    //Nhảy trang
    $(selector + " .bottom-pager input").change(function () {
        var cPage = trim12($(this).val());
        var maxPage = $(selector + " .bottom-pager input[type=hidden]").val();
        if (cPage.length == 0)
            createMessage("Thông báo", "Yêu cầu nhập trang cần chuyển đến");
        else if (isNaN(cPage))
            createMessage("Thông báo", "trang chuyển đến phải là kiểu số");
        else if (parseInt(cPage) > maxPage)
            createMessage("Thông báo", "trang không được lớn hơn " + maxPage + "");
        else if (parseInt(cPage) <= 0) {
            createMessage("Thông báo", "trang phải lớn hơn 0");
        }
        else {
            window.location.href = changeHashUrl('Page', cPage);;
        }
    });

    //ẩn hiện nhóm
    $(selector + " .gridView a.group").click(function () {
        var idShowHide = $(this).attr("href");
        if ($(this).text() == '+') {
            $(idShowHide).show();
            $(this).text("-");
        } else {
            $(idShowHide).hide();
            $(this).text("+");
        }
        return false;
    });

    //Thay đổi số bản ghi trên trang
    $(selector + " .bottom-pager select").change(function () {
        var urlFWs = $.address.value();
        urlFWs = changeHashValue("Page", 1, urlFWs); //Replace  &Page=.. => Page=1
        urlFWs = changeHashValue("RowPerPage", $(this).val(), urlFWs); //Replace  &TenDonVi=.. => TenDonVi=donViNhan
        window.location.href = urlFWs;
    });

    //Đăng ký xóa nhiều
    $(selector + " .gridView a.deleteAll").click(function () {
        var arrRowId = '';
        var rowTitle = '';
        var linkFW = '';
        var linkFW = (linkFW == '') ? '#' + urlDeleteFrefix + 'Page=1' : '#' + urlDeleteFrefix + linkFW;
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
            rowTitle += "<li>" + escapeHTML($(this).parent().parent().attr("title")) + "</li>";
        });
        rowTitle = "<ol>" + rowTitle + "</ol>";

        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowDelete(urlPostAction, arrRowId, rowTitle, linkFW);
        return false;
    });

    //Đăng ký Hiển thị nhiều
    $(selector + " .gridView a.showAll").click(function () {
        var arrRowId = '';
        var rowTitle = '';
        var linkFW = '';
        var linkFW = (linkFW == '') ? '#' + urlShowFrefix + 'Page=1' : '#' + urlShowFrefix + linkFW;
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
            rowTitle += "<li>" + escapeHTML($(this).parent().parent().attr("title")) + "</li>";
        });
        rowTitle = "<ol>" + rowTitle + "</ol>";

        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowShow(urlPostAction, arrRowId, rowTitle, linkFW);
        return false;
    });


    //Đăng ký ẩn nhiều
    $(selector + " .gridView a.hideAll").click(function () {
        var arrRowId = '';
        var rowTitle = '';
        var linkFW = '';
        var linkFW = (linkFW == '') ? '#' + urlHideFrefix + 'Page=1' : '#' + urlHideFrefix + linkFW;
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
            rowTitle += "<li>" + escapeHTML($(this).parent().parent().attr("title")) + "</li>";
        });
        rowTitle = "<ol>" + rowTitle + "</ol>";

        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowHide(urlPostAction, arrRowId, rowTitle, linkFW);
        return false;
    });

    //Đăng ký button xóa row nhóm
    $(selector + " .gridView a.delete_group").click(function () {
        rowDelete(urlPostActionGroup, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), "#" + urlDeleteFrefix + "Page=1");
        return false;
    });

    //Đăng ký button hiển thị nhóm
    $(selector + " .gridView a.show_group").click(function () {
        rowShow(urlPostActionGroup, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), "#" + urlDeleteFrefix + "Page=1");
        return false;
    });

    //Đăng ký button ẩn nhóm
    $(selector + " .gridView a.hide_group").click(function () {
        rowHide(urlPostActionGroup, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), "#" + urlHideFrefix + "Page=1");
        return false;
    });


    //Đăng ký button xóa row
    $(selector + " .gridView a.delete_guid").click(function () {
        rowDeleteGuid(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), "#" + urlDeleteFrefix + "Page=1");
        return false;
    });

    //Đăng ký button xóa row
    $(selector + " .gridView a.delete").click(function () {
        rowDelete(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), "#" + urlDeleteFrefix + "Page=1");
        return false;
    });

    //Đăng ký button hiển thị
    $(selector + " .gridView a.show").click(function () {
        rowShow(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), "#" + urlDeleteFrefix + "Page=1");
        return false;
    });

    //Đăng ký button ẩn
    $(selector + " .gridView a.hide").click(function () {
        rowHide(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), "#" + urlHideFrefix + "Page=1");
        return false;
    });

    //đăng ký Thêm row
    $(selector + " .gridView a.add").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag == '')
            titleDiag = 'Thêm mới bản ghi';

        var urlRequest = '';
        if (urlForm.indexOf('?') > 0)
            urlRequest = urlForm + '&do=add&ItemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + '?do=add&ItemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });

    //đăng ký sửa row
    $(selector + " .gridView a.edit_guid").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Sửa thông tin bản ghi';

        var urlRequest = '';
        if (urlForm.indexOf('?') > 0)
            urlRequest = urlForm + '&do=edit&guidId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + '?do=edit&guidId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });

    //đăng ký sửa row
    $(selector + " .gridView a.edit").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Sửa thông tin bản ghi';

        var urlRequest = '';
        if (urlForm.indexOf('?') > 0)
            urlRequest = urlForm + '&do=edit&ItemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + '?do=edit&ItemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });

    //đăng ký sửa row
    $(selector + " .gridView a.workflow").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Phê duyệt nội dung xuất bản';

        var urlRequest = '';
        if (urlFormApprove.indexOf('?') > 0)
            urlRequest = urlFormApprove + '&ItemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlFormApprove + '?ItemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: 460,
                width: 800,
                modal: true
            }).dialog("open");
        });
        return false;
    });


    //đăng ký sắp xếp
    $(selector + " .gridView a.sort").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Sắp xếp thứ tự hiển thị';

        var urlRequest = '';
        if (urlSort.indexOf('?') > 0)
            urlRequest = urlSort + '&do=sort&ItemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlSort + '?do=sort&ItemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });

    //đăng ký xem row
    $(selector + " .gridView a.view_guid").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Xem thông tin bản ghi';

        var urlRequest = '';
        if (urlView.indexOf('?') > 0)
            urlRequest = urlView + '&guidId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlView + '?guidId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: viewHeight,
                width: viewWidth,
                modal: false,
                buttons: {
                    "Đóng cửa sổ": function () {
                        $(this).html("").dialog("close");
                        $("div.ui-dialog-buttonpane").remove();
                    }
                }
            }).dialog("open");;
        });
        return false;
    });

    //đăng ký xem row
    $(selector + " .gridView a.reset_guid").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Reset mật khẩu';

        var urlRequest = '';
        if (urlForm.indexOf('?') > 0)
            urlRequest = urlFormReset + '&do=reset&guidId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlFormReset + '?do=reset&guidId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });


    //đăng ký xem row
    $(selector + " .gridView a.permission_guid").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Phân quyền tài khoản';

        var urlRequest = '';
        if (urlForm.indexOf('?') > 0)
            urlRequest = urlFormPermission + '&do=permission&guidId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlFormPermission + '?do=permission&guidId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });


    //đăng ký xem row
    $(selector + " .gridView a.view").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Xem thông tin bản ghi';

        var urlRequest = '';
        if (urlView.indexOf('?') > 0)
            urlRequest = urlView + '&itemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlView + '?itemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: viewHeight,
                width: viewWidth,
                modal: false,
                buttons: {
                    "Đóng cửa sổ": function () {
                        $(this).html("").dialog("close");
                        $("div.ui-dialog-buttonpane").remove();
                    }
                }
            }).dialog("open");;
        });
        return false;
    });

    //đăng ký Thêm row cho nhóm
    $(selector + " .gridView a.add_group").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Thêm mới bản ghi';

        var urlRequest = '';
        if (urlForm.indexOf('?') > 0)
            urlRequest = urlForm + '&do=add&ItemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + '?do=add&ItemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });

    //đăng ký sửa row nhóm
    $(selector + " .gridView a.edit_group").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Sửa thông tin bản ghi';

        var urlRequest = '';
        if (urlFormGroup.indexOf('?') > 0)
            urlRequest = urlFormGroup + '&do=edit&ItemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlFormGroup + '?do=edit&ItemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });


    //đăng ký sắp xếp
    $(selector + " .gridView a.sort_group").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Sắp xếp thứ tự hiển thị';

        var urlRequest = '';
        if (urlSortGroup.indexOf('?') > 0)
            urlRequest = urlSortGroup + '&do=sort&ItemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlSortGroup + '?do=sort&ItemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: formHeight,
                width: formWidth,
                modal: true
            }).dialog("open");
        });
        return false;
    });


    //đăng ký xem row nhóm
    $(selector + " .gridView a.view_group").click(function () {
        var titleDiag = escapeHTML($(this).attr("title"));
        if (titleDiag == '')
            titleDiag = 'Xem thông tin bản ghi';

        var urlRequest = '';
        if (urlViewGroup.indexOf('?') > 0)
            urlRequest = urlViewGroup + '&itemId=' + $(this).attr("href").substring(1);
        else
            urlRequest = urlViewGroup + '?itemId=' + $(this).attr("href").substring(1);

        $.post(urlRequest, function (data) {
            $("#dialog-form").html(data).dialog({
                title: titleDiag,
                resizable: true,
                height: viewHeight,
                width: viewWidth,
                modal: false,
                buttons: {
                    "Đóng cửa sổ": function () {
                        $(this).html("").dialog("close");
                        $("div.ui-dialog-buttonpane").remove();
                    }
                }
            }).dialog("open");;
        });
        return false;
    });

    $(selector + ' .gridView tr th a').click(function (e) {
        var url = window.location.href;
        var parameters;
        var link = $(this).attr("href");
        link = link.substring(1, link.length);
        if (url.indexOf(link) > 0) {
            if (url.indexOf('FieldOption=1') > 0) {
                $(this).addClass('desc');
                $(this).attr("href", '#' + link + '&FieldOption=0');
                parameters = link + '&FieldOption=0';
            } else {
                $(this).addClass('asc');
                $(this).attr("href", '#' + link + '&FieldOption=1');
                parameters = link + '&FieldOption=1';
            }
        } else {
            parameters = url.indexOf('#') >= 0 ? $(this).attr('href').substring(1) : $(this).attr('href');
        }
        var index = url.indexOf('Field');
        if (index >= 0) {
            url = url.substring(0, index);
            url = url + parameters;
            window.location.href = url;
        } else {
            if (url.indexOf('#') > 0) {
                url = url + '&' + parameters;
            } else {
                url = url + parameters;
            }
        }
        window.location.href = url;
        e.preventDefault();
        return false;
    });
}

//Hiển thị row tren grid
function rowShow(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia = '';
    if (arrRowId == '')
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(',') > 0)
            titleDia = "Hiển thị các bản ghi đã chọn";
        else
            titleDia = "Hiển thị bản ghi đã chọn";
        $("#dialog-confirm").attr(titleDia);
        $("#dialog-confirm").html("<p><b>Bạn có chắc chắn muốn hiển thị:</b><br />" + escapeHTML(rowTitle) + "</p>");
        var comfirmReturn = false;
        var tokenValid = $("input[name='__RequestVerificationToken']").val();
        $("#dialog-confirm").dialog({
            title: titleDia,
            resizable: false,
            height: 'auto',
            width: 'auto',
            modal: false,
            buttons: {
                "Tiếp tục": function () {
                    $(this).dialog("close");
                    $.post(encodeURI(urlPost), { "do": "show", "itemId": "" + arrRowId + "", "__RequestVerificationToken": "" + tokenValid + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        }
                        else {
                            createMessage("Thông báo", data.Message);
                            window.location.href = urlFw + '&type=show&idShow=' + arrRowId;
                        }
                    });
                },
                "Hủy lệnh hiển thị": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}


//ẩn row tren grid
function rowHide(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia = '';
    if (arrRowId == '')
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(',') > 0)
            titleDia = "Ẩn các bản ghi đã chọn";
        else
            titleDia = "Ẩn bản ghi đã chọn";
        $("#dialog-confirm").attr(titleDia);
        $("#dialog-confirm").html("<p><b>Bạn có chắc chắn muốn ẩn:</b><br />" + escapeHTML(rowTitle) + "</p>");
        var comfirmReturn = false;
        var tokenValid = $("input[name='__RequestVerificationToken']").val();
        $("#dialog-confirm").dialog({
            title: titleDia,
            resizable: false,
            height: 'auto',
            width: 'auto',
            modal: false,
            buttons: {
                "Tiếp tục": function () {
                    $(this).dialog("close");
                    $.post(encodeURI(urlPost), { "do": "hide", "itemId": "" + arrRowId + "", "__RequestVerificationToken": "" + tokenValid + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        }
                        else {
                            createMessage("Thông báo", data.Message);
                            window.location.href = urlFw + '&type=hide&idHide=' + arrRowId;
                        }
                    });
                },
                "Hủy lệnh ẩn": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

//xoa row tren grid
function rowDeleteGuid(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia = '';
    if (arrRowId == '')
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(',') > 0)
            titleDia = "Xóa các bản ghi đã chọn";
        else
            titleDia = "Xóa bản ghi đã chọn";
        $("#dialog-confirm").attr(titleDia);
        $("#dialog-confirm").html("<p><b>Bạn có chắc chắn muốn xóa:</b><br />" + rowTitle + "</p>");
        var comfirmReturn = false;
        var tokenValid = $("input[name='__RequestVerificationToken']").val();
        $("#dialog-confirm").dialog({
            title: titleDia,
            resizable: false,
            height: 'auto',
            width: 'auto',
            modal: false,
            buttons: {
                "Tiếp tục": function () {
                    $(this).dialog("close");
                    $.post(encodeURI(urlPost), { "do": "delete", "guidId": "" + arrRowId + "", "__RequestVerificationToken": "" + tokenValid + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        }
                        else {
                            createMessage("Thông báo", data.Message);
                            window.location.href = urlFw + '&type=delete&idDelete=' + arrRowId;
                        }
                    });
                },
                "Hủy lệnh xóa": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}


//xoa row tren grid
function rowDelete(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia = '';
    if (arrRowId == '')
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(',') > 0)
            titleDia = "Xóa các bản ghi đã chọn";
        else
            titleDia = "Xóa bản ghi đã chọn";
        $("#dialog-confirm").attr(titleDia);
        $("#dialog-confirm").html("<p><b>Bạn có chắc chắn muốn xóa:</b><br />" + rowTitle + "</p>");
        var comfirmReturn = false;
        var tokenValid = $("input[name='__RequestVerificationToken']").val();
        $("#dialog-confirm").dialog({
            title: titleDia,
            resizable: false,
            height: 'auto',
            width: 'auto',
            modal: false,
            buttons: {
                "Tiếp tục": function () {
                    $(this).dialog("close");
                    $.post(encodeURI(urlPost), { "do": "delete", "itemId": "" + arrRowId + "", "__RequestVerificationToken": "" + tokenValid + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        }
                        else {
                            createMessage("Thông báo", data.Message);
                            window.location.href = urlFw + '&type=delete&idDelete=' + arrRowId;
                        }
                    });
                },
                "Hủy lệnh xóa": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function escapeHTML(str) {
    var div = document.createElement('div');
    var text = document.createTextNode(str);
    div.appendChild(text);
    return div.innerHTML;
}


function trim12(str) {
    var str = str.replace(/^\s\s*/, ''),
		ws = /\s/,
		i = str.length;
    while (ws.test(str.charAt(--i)));
    return str.slice(0, i + 1);
}

function createCloseMessage(title, message, urlFw) {
    $("#dialog-message").html("<p>" + message + "</p>");
    $("#dialog-message").dialog({
        title: title,
        resizable: false,
        width: 'auto',
        height: 'auto',
        modal: false,
        buttons: {
            "Đóng lại": function () {
                $(this).dialog("close");
                window.location.href = urlFw;
            }
        }
    });
}

/* Thêm vào nhằm kết hợp Sort + Search + Paging */
function getSortingParameters() {
    var url = window.location.href;
    var index = url.indexOf('Field');
    if (index > 0) {
        var parameters = url.substring(index, url.length);
        if (parameters.indexOf('FieldOption') < 0) {
            parameters = parameters + '&FieldOption=0';
        }
        var i1 = parameters.indexOf('Field');
        var i2 = parameters.indexOf('FieldOption');
        parameters = parameters.substring(i1, i2 + "FieldOption=0".length);
        return parameters;
    }
    return '';
}
function ajaxFormSearch(selector) {
    var parameters = getSortingParameters();
    window.location.href = parameters != ''
        ? '#' + $(selector).mySerialize() + '&' + parameters
        : '#' + $(selector).mySerialize();
}

function setRolesEdit(status) {
    if (status)
        $(".act_edit").css("display", "marker");
    else
        $(".act_edit").css("display", "none");
}

function setRolesDelete(status) {
    if (status)
        $(".act_delete").css("display", "marker");
    else
        $(".act_delete").css("display", "none");
}

function setRolesAdd(status) {
    if (status) {
        $(".act_add").removeClass("act_add_hidden");
        $(".act_add").css("display", "marker");
    }
    else
        $(".act_add").css("display", "none");
}

function setRolesApproved(status) {
    if (status)
        $(".act_approved").css("display", "marker");
    else
        $(".act_approved").css("display", "none");
}


function setRolesRoles(status) {
    if (status)
        $(".act_roles").css("display", "marker");
    else
        $(".act_roles").css("display", "none");
}


(function ($) {

    $.fn.serialize = function (options) {
        return $.param(this.serializeArray(options));
    };

    $.fn.serializeArray = function (options) {
        var o = $.extend({
            checkboxesAsBools: false
        }, options || {});

        var rselectTextarea = /select|textarea/i;
        var rinput = /text|hidden|password|search/i;

        return this.map(function () {
            return this.elements ? $.makeArray(this.elements) : this;
        })
        .filter(function () {
            return this.name && !this.disabled &&
                (this.checked
                || (o.checkboxesAsBools && this.type === 'checkbox')
                || rselectTextarea.test(this.nodeName)
                || rinput.test(this.type));
        })
            .map(function (i, elem) {
                var val = $(this).val();
                return val == null ?
                null :
                $.isArray(val) ?
                $.map(val, function (val, i) {
                    return { name: elem.name, value: val };
                }) :
             {
                 name: elem.name,
                 value: (o.checkboxesAsBools && this.type === 'checkbox') ? //moar ternaries!
                        (this.checked ? 'true' : 'false') :
                        val
             };
            }).get();
    };

})(jQuery);
