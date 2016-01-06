var purecms = {
    //loading
    loading: function(){
        var loader = jQuery('<div id="loader" class="loading">loading...</div>')
            .appendTo("body")
            .hide();
        jQuery(document).ajaxStart(function () {
            loader.show();
        }).ajaxStop(function () {
            loader.hide();
        }).ajaxError(function (a, b, e) {
            loader.hide();
            throw e;
        });
    },
    //发送POST请求
    post: function (url, data, refresh, onsuccess, onerror) {
        if (typeof (data) == "object") {
            data = JSON.stringify(data);
        }
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (response) {
                console.log(response);
                $.messager.popup(response.Content);
                if (refresh == undefined || refresh == true) {
                    setTimeout("location.reload()", 1000);
                }
                if (onsuccess) {
                    onsuccess(response);
                }
            }
            , error: function (response) {
                console.log(response);
                $.messager.popup("出错了");
                if (onerror) {
                    onerror(response);
                }
            }
        });
    },
getjson: function (url, data, onsuccess, onerror) {
    if (typeof (data) == "object") {
        data = JSON.stringify(data);
    }
    $.ajax({
        type: "GET",
        url: url,
        data: data,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (response) {
            console.log(response);
            if (onsuccess) {
                var result = purecms.getajaxresult(response);
                onsuccess(result);
            }
        }
        , error: function (response) {
            console.log(response);
            if (onerror) {
                onerror(response);
            }
        }
    });
},
//获取服务器返回的数据
getajaxresult: function (response) {
    var obj = {};
    obj.content = null;
    if (response) {
        obj = response;
        obj.content = JSON.parse(response.Content);
    }
    return obj;
},
//初始化数据列表
datatable: function (target) {
    //列表排序样式
    var sortby = target.attr("data-sortby");
    var sortdirect = target.attr("data-sortdirection");
    var current = target.find("th[data-sortname=" + sortby + "]");
    current.addClass("success");
    if (sortdirect == 0) {
        current.find("a").append('<span class="glyphicon glyphicon-sort-by-attributes"></span>');
    }
    else {
        current.find("a").append('<span class="glyphicon glyphicon-sort-by-attributes-alt"></span>');
    }
    //全选、反选、选择一行
    target.find("input[name=checkall]").click(function () {
        var flag = $(this).prop("checked");
        if (flag) {
            target.find("input[name=recordid]").prop("checked", true);
            target.find("tbody > tr").addClass("active");
        }
        else {
            target.find("input[name=recordid]").removeProp("checked");
            target.find("tbody > tr").removeClass("active");
        }
    });
    target.find("input[name=recordid]").click(function (e) {
        var flag = $(this).prop("checked");
        if (flag) {
            $(this).parents("tr").addClass("active");
        }
        else {
            $(this).parents("tr").removeClass("active");
        }
        e.stopPropagation();
        e.preventDefault();
        return false;
    });
    //行单击时选中
    target.find("tbody > tr").click(function (e) {
        var flag = $(this).find("input[name=recordid]").prop("checked");
        if (flag) {
            $(this).find("input[name=recordid]").removeProp("checked");
            $(this).removeClass("active");
        }
        else {
            $(this).find("input[name=recordid]").prop("checked", true);
            $(this).addClass("active");
        }
    });
    //批量删除
    target.find("button[data-role=delete]").click(function () {
        var action = $(this).attr("data-action");
        var datas = purecms.gettableselected(target);
        if (datas.length == 0) {
            $.messager.popup("请选择要删除的记录");
        } else if (action == "") {
            $.messager.popup("提交地址不能为空");
        }
        else {
            var model = { recordid: datas };
            purecms.post(action, model);
        }
    });
    //批量更新
    target.find("[data-role=update]").click(function () {
        var action = $(this).attr("data-action");//批量操作
        var datas = purecms.gettableselected(target);
        if (datas.length == 0) {
            $.messager.popup("请选择要更新的记录");
        } else if (action == "") {
            $.messager.popup("请选择要进行的操作");
        }
        else {
            var model = { recordid: datas };
            purecms.post(action, model);
        }
    });
    return target;
}
//获取列表选中的记录ID
    , gettableselected: function (target, tostring) {
        var result = new Array();
        target.find("input[name=recordid]:checked").each(function (i, n) {
            result.push($(n).val());
        });

        return tostring == true ? JSON.stringify(result) : result;
    }
//删除一条记录
    , del: function (id, action, refresh, onsuccess, onerror) {
        purecms.confirm("请确认是否删除？", function () {
            purecms.post(action, { recordid: id }, refresh, onsuccess, onerror);
        });
    }
//弹出确认框
    , confirm: function (text, onOk, onCancel) {
        var target = $("#confirmDialog");
        if (target.length == 0) {
            $("body").append('<div id="confirmDialog" class="hide"><p class="text-danger">' + text + '</p></div>');
            target = $("#confirmDialog");
        }
        target.dialog({
            title: "确认"
        , buttons: {
            "取消": function () {
                if (onCancel) onCancel();
                $(this).dialog("close");
            }
            , "确定": function () {
                if (onOk) onOk();
                $(this).dialog("close");
            }
        }
        }).removeClass("hide");
    }
    , form: function (target, onsuccess, onerror) {
        //表单控件name属性值改为小写
        target.find("input").each(function (i, n) {
            var name = $(n).prop("name");
            $(n).prop("name", name.toLowerCase());
        });
        //表单操作按钮设置
        var formBtns = target.find("#form-buttons").find("button");
        formBtns.each(function (i, n) {
            var shadow = $(n).clone();
            if ($(n).prop("type") == "submit") {
                shadow.click(function () { target.submit(); });
            }
            else if ($(n).prop("type") == "reset") {
                shadow.click(function () { target.resetForm(); });
            }
            $("#body-footer-content").append(shadow);
            $("#body-footer-content").append(" ");
        });
        target.find("#form-buttons").addClass("hide");
        $("#body-footer").removeClass("hide");
        //表单验证
        target.find("input[data-val-required]").attr("data-toggle", "tooltip").prop("title", $(this).attr("data-val-required")).addClass("required");
        $("[data-toggle='tooltip']").tooltip();
        target.find(".form-group").each(function (i, n) {
            var name = $(this).find(".required").prop("name");
            if ($(this).find(".required").length > 0) {
                $(this).find(".control-label").append('<span class="text-danger pull-right">*</span>');
            }
        });
        target.validate({
            errorClass: "text-warning"
            //, errorElement: "p"
            //,focusCleanup: true
            , errorPlacement: function (error, element) {
                console.log(error);
                $(element).siblings(".text-warning").remove();
                if ($(element).is(":radio") || $(element).is(":checkbox")) {
                    error.appendTo($(element).parent().parent());
                }
                else {
                    error.appendTo($(element).parent());
                }
                $(element).parents(".form-group").addClass("has-error");
            }
            , success: function (label) {
                $(label).parents(".form-group").removeClass("has-error");
            }
            , submitHandler: function (form) {
                var ajaxsubmit = target.attr("data-ajaxsubmit");//是否以AJAX方式提交，默认是
                if (ajaxsubmit != "" && ajaxsubmit == "false") {
                    form.submit();
                }
                else {
                    jQuery(form).ajaxSubmit({
                        type: "post",
                        success: function (response) {
                            console.log(response);
                            $.messager.popup(response.Content);
                            if (typeof (onsuccess) == "function") {//成功回调方法
                                onsuccess(response);
                            }
                            if (target.attr("data-autoreset") && target.attr("data-autoreset").toLowerCase() == "true") {
                                if (response.Content.StatusName != "error") target.resetForm();
                            }
                        },
                        error: function (XmlHttpRequest, textStatus, errorThrown) {
                            if (typeof (onerror) == "function") {//失败回调方法
                                onerror(XmlHttpRequest);
                            }
                        }
                    });
                }
            }
        });
    }
    , ajaxform: function (target) {
        target.ajaxForm(function (response) {
            console.log(response);
            $.messager.popup(response.Content);
        });
    }
}