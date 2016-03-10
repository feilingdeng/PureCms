var purecms = {
    //loading
    loading: function () {
        var loader = jQuery('<div id="loader" class="loading">loading...</div>')
            .appendTo("body")
            .hide();
        jQuery(document).ajaxStart(function () {
            loader.show();
        }).ajaxStop(function () {
            loader.hide();
        }).ajaxError(function (a, b, e) {
            loader.hide();
            purecms.alert('error', '发生了错误');
            console.log(e);
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
                purecms.alert('success', response.Content);
                if (refresh == undefined || refresh == true) {
                    setTimeout("location.reload()", 1000);
                }
                if (typeof (onsuccess) == "function") {
                    onsuccess(response);
                }
            }
            , error: function (response) {
                console.log(response);
                $.messager.popup("出错了");
                purecms.alert('error', "出错了");
                if (typeof (onerror) == "function") {
                    onerror(response);
                }
            }
        });
    },
    getjson: function (url, data, onsuccess, onerror) {
        //if (typeof (data) == "object") {
        //    data = JSON.stringify(data);
        //}
        $.ajax({
            type: "GET",
            url: url,
            data: data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (response) {
                console.log(response);
                if (typeof (onsuccess) == "function") {
                    var result = purecms.getajaxresult(response);
                    onsuccess(result);
                }
            }
            , error: function (response) {
                console.log(response);
                if (typeof (onerror) == "function") {
                    onerror(response);
                }
            }
        });
    },
    get: function (url, onsuccess, onerror) {
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (response) {
                console.log(response);
                if (typeof (onsuccess) == "function") {
                    var result = purecms.getajaxresult(response);
                    onsuccess(result);
                }
            }
            , error: function (response) {
                console.log(response);
                if (typeof (onerror) == "function") {
                    onerror(response);
                }
            }
        });
    },
    load: function (url, onsuccess, onerror) {
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/text; charset=utf-8",
            cache: false,
            success: function (response) {
                if (typeof (onsuccess) == "function") {
                    onsuccess(response);
                }
            }
            , error: function (response) {
                console.log(response);
                if (typeof (onerror) == "function") {
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
        if (target.find('tbody > tr').length == 0) {
            target.find('tbody').append('<tr class="bg-warning"><td colspan="' + target.find('thead > tr > th').length + '">没有数据</td></tr>');
            target.find('thead > tr > th').find('a').prop('href', 'javascript:void(0)');
            target.find('tfoot').hide();
            return;
        }
        //列表排序样式
        var sortby = target.attr("data-sortby");
        var sortdirect = target.attr("data-sortdirection");
        var current = target.find("th[data-name='" + sortby + "']");
        current.addClass("success");
        if (sortdirect == 0) {
            current.find("a").append('<span class="glyphicon glyphicon-sort-by-attributes"></span>');
        }
        else {
            current.find("a").append('<span class="glyphicon glyphicon-sort-by-attributes-alt"></span>');
        }
        //全选、反选、选择一行
        target.find("input[name=checkall]").on('click', null, function () {
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
        //target.on('click',"input[name=recordid]",function (e) {
        //    var flag = $(this).prop("checked");
        //    if (flag) {
        //        $(this).parents("tr").addClass("active");
        //    }
        //    else {
        //        $(this).parents("tr").removeClass("active");
        //    }
        //    e.stopPropagation();
        //    e.preventDefault();
        //    return false;
        //});
        //行单击时选中
        target.on('click', "tbody tr", function (e) {
            var $target = $(e.target);
            var ischeckbox = $target.is('input') && $target.prop('type') == 'checkbox';
            if (($target.is('input') || $target.is('select')) && !ischeckbox) {
                return;
            }
            var flag = $(this).find("input[name=recordid]").prop("checked");
            if (flag) {
                if (ischeckbox) {
                    $(this).addClass("active");
                }
                else {
                    $(this).find("input[name=recordid]").removeProp("checked");
                    $(this).removeClass("active");
                }
            }
            else {
                if (ischeckbox) {
                    $(this).removeClass("active");
                }
                else {
                    $(this).find("input[name=recordid]").prop("checked", true);
                    $(this).addClass("active");
                }
            }
        });
        //批量删除
        target.find("button[data-role=delete]").click(function () {
            var action = $(this).attr("data-action");
            var datas = purecms.gettableselected(target);
            if (datas.length == 0) {
                $.messager.popup("请选择要删除的记录");
                purecms.alert('error', '请选择要删除的记录');
            } else if (action == "") {
                $.messager.popup("提交地址不能为空");
                purecms.alert('error', '提交地址不能为空');
            }
            else {
                confirmtext = "请确认是否删除？" + ($(this).attr("data-tooltip") ? $(this).attr("data-tooltip") : '');
                var isconfirm = false;

                var model = { recordid: datas };

                var one = function () {
                    var dfd = $.Deferred();
                    purecms.confirm("确认删除", confirmtext, function () {
                        isconfirm = true;
                        dfd.resolve();
                    });
                    return dfd.promise();
                };
                $.when(one()).done(function () {
                    var dfd = $.Deferred();
                    isconfirm = false;
                    purecms.confirm("再次确认删除", "<strong>请再次确认：</strong>" + confirmtext, function () {
                        isconfirm = true;
                        purecms.post(action, model);
                        dfd.resolve();
                    });
                    return dfd.promise();
                }
                );
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
    , del: function (id, action, refresh, onsuccess, onerror, confirmtext, isconfirmagain) {
        if (!confirmtext || confirmtext == undefined || confirmtext == '') confirmtext = "请确认是否删除？";
        else confirmtext = "请确认是否删除？" + confirmtext;
        var isconfirm = false;

        var one = function () {
            var dfd = $.Deferred();
            purecms.confirm("确认删除", confirmtext, function () {
                isconfirm = true;
                dfd.resolve();
            });
            return dfd.promise();
        };
        $.when(one()).done(function () {
            var dfd = $.Deferred();
            if (isconfirmagain != undefined && isconfirmagain == true) {
                isconfirm = false;
                purecms.confirm("再次确认删除", "<strong>请再次确认：</strong>" + confirmtext, function () {
                    isconfirm = true;
                    purecms.post(action, { recordid: id }, refresh, onsuccess, onerror);
                    dfd.resolve();
                });
            }
            else if (isconfirm == true) {
                purecms.post(action, { recordid: id }, refresh, onsuccess, onerror);
            }
            return dfd.promise();
        }
        );

    }
    //弹出确认框
    , confirm: function (title, text, onOk, onCancel) {
        var target = $("#confirmDialog");
        if (target.length > 0) {
            target.remove();
        }
        if (!title || title == undefined || title == '') title = "确认";
        if (!text || text == undefined || text == '') text = "确定进行该操作？";
        $("body").append('<div id="confirmDialog" class="hide"><p class="text-danger">' + text + '</p></div>');
        target = $("#confirmDialog");

        target.dialog({
            title: title
        , buttons: {
            "取消": function () {
                $(this).dialog("close");
                if (typeof (onCancel) == "function") {
                    onCancel();
                }
            }
            , "确定": function () {
                $(this).dialog("close");
                if (typeof (onOk) == "function") {
                    onOk();
                }
            }
        }
        }).removeClass("hide");
    }
    //弹出信息提示框
    , alert: function (status, text, onOk, onCancel) {
        var target = $("#alertDialog");
        var cssName = status == true ? "text-info" : "text-danger";
        if (target.length == 0) {
            $("body").append('<div id="alertDialog" class="hide"><p class="' + cssName + '">' + text + '</p></div>');
            target = $("#alertDialog");
        }
        target.dialog({
            title: "确认"
        , buttons: {
            "取消": function () {
                if (typeof (onCancel) == "function") {
                    onCancel();
                }
                $(this).dialog("close");
            }
            , "确定": function () {
                if (typeof (onOk) == "function") {
                    onOk();
                }
                $(this).dialog("close");
            }
        }
        }).removeClass("hide");
    }
    //表单
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
                            var status = response.Content.StatusName == "error" ? "0" : "1";
                            purecms.alert(status, response.Content);
                            if (typeof (onsuccess) == "function") {//成功回调方法
                                onsuccess(response);
                            }
                            if (target.attr("data-autoreset") && target.attr("data-autoreset").toLowerCase() == "true") {
                                if (status == 1) target.resetForm();
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
    //ajax 提交表单
    , ajaxform: function (target) {
        target.ajaxForm(function (response) {
            console.log(response);
            $.messager.popup(response.Content);
        });
    }
    //复选框设置为单选
    , singlecheckbox: function (container, selector, mode) {
        $(container).off('click', selector);
        if (mode == undefined || mode == 'single') {
            $(container).find(selector).prop('checked', false);
            $(container).on('click', selector, function () {
                if ($(this).is(':checked')) {
                    $(container).find(selector).prop('checked', false);
                    $(this).prop('checked', true);

                }
            });
        }
    }
    //获取或设置下拉选项选中的值
    , selectedvalue: function (selector, value) {
        var target = typeof (selector) == 'object' ? selector : $(selector);
        //设置值
        if (value != undefined) {
            if (value == '') value = '""';
            target.find('option').prop('selected', false);
            target.find('option[value=' + value + ']').prop('selected', true);
        }
            //获取值
        else {
            var o = target.find('option:selected');
            if (o != undefined && o.length > 0) {
                return o.val();
            }
            return null;
        }
    }
    //设置表格行选中
    , tableselected: function (container, selector, onselected, unselected) {
        if (selector == undefined) selector = 'tbody tr';
        $(container).on('click', selector, function (e) {
            var $target = $(e.target);
            var ischeckbox = $target.is('input') && $target.prop('type') == 'checkbox';
            if (!$target.is('td') && !ischeckbox) {//(($target.is('input') || $target.is('select') || $target.is('a')) && !ischeckbox) {
                return;
            }
            var selected = false;
            var checkedflag = $(this).find(":checkbox").prop("checked");
            if (checkedflag) {
                if (ischeckbox) {
                    $(this).addClass("active");
                    selected = true;
                }
                else {
                    $(this).find(":checkbox").removeProp("checked");
                    $(this).removeClass("active");
                }
            }
            else {
                if (ischeckbox) {
                    $(this).removeClass("active");
                }
                else {
                    $(this).find(":checkbox").prop("checked", true);
                    $(this).addClass("active");
                    selected = true;
                }
            }
            if (typeof (onselected) == 'function' && selected == true) {
                onselected($(this));
            }
            if (typeof (unselected) == 'function' && selected == false) {
                unselected($(this));
            }
        });
    }
}