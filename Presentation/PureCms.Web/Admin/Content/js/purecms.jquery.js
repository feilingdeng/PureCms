var tableToExcel = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) },
        format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (table, name) {
        var $table = [];
        $table = $(table).clone();
        $table.find("td").each(function () {
            if ($(this).css("display") == "none") {
                $(this).remove();
            }
        });
        $table.find("th").each(function () {
            if ($(this).css("display") == "none") {
                $(this).remove();
            }
        });
        var ctx = {
            worksheet: name || 'Worksheet', table: $table.html()
        }
        window.location.href = uri + base64(format(template, ctx));
    }
})();
/*
功能說明:   固定表頭。
創建人:        hmj
創建時間:   2011-06-29
 
功能：固定表頭。
      使用容器的ID進行設定$("#div").chromatable({width: "100%",height: "100%", scrolling: "yes"})
      table必須包含有<thead>標籤
參數：無。
*/
(function ($) {
    $.chromatable = {
        defaults: {
            width: "900px",  //設定容器寬度,待擴展程式
            height: "300px", //設定容器高度,待擴展程式
            scrolling: "yes" //yes跟隨IE滾動條而滑動, no固定在頁面上僅容器滾動條滑動
        }
    };
    $.fn.chromatable = function (options) {
        var options = $.extend({}, $.chromatable.defaults, options);
        return this.each(function () {
            var $divObj = $(this);
            var $tableObj = $divObj.find("table");
            var $uniqueID = $tableObj.attr("ID") + ("wrapper");
            var $class = $tableObj.attr("class");
            var $tableWidth = $tableObj.width();
            var top = $("#" + $tableObj.attr("ID")).offset().top;
            var left = $("#" + $tableObj.attr("ID")).offset().left
            $divObj.append("<table class='" + $class + "' id='" + $uniqueID + "' style='position:absolute;top:" + top + "px;left:" + left + "px;' width='" + $tableWidth + "' border='0' cellspacing='0' cellpadding='0'><thead>" + $("#" + $tableObj.attr("ID")).find("thead").html() + "</thead></table>");

            $.each($("#" + $tableObj.attr("ID")).find("thead th"), function (i, item) {
                $("#" + $uniqueID).find("thead th").eq(i).width($(item).width());
                $(item).width($(item).width());
            });

            if (options.scrolling === "yes") {
                scrollEvent($tableObj.attr("ID"), $uniqueID);
            }
            resizeEvent($tableObj.attr("ID"), $uniqueID);
        });

        function scrollEvent(tableId, uniqueID) {
            var element = $("#" + uniqueID);
            $(window).scroll(function () {
                var top = $("#" + tableId).offset().top;
                var scrolls = $(this).scrollTop();

                if (scrolls > top) {
                    if (window.XMLHttpRequest) {
                        element.css({
                            position: "fixed",
                            top: 0
                        });
                    } else {
                        element.css({
                            top: scrolls
                        });
                    }
                } else {
                    element.css({
                        position: "absolute",
                        top: top
                    });
                }

            });
        };

        function resizeEvent(tableId, uniqueID) {
            var element = $("#" + uniqueID);
            $(window).resize(function () {
                var top = $("#" + tableId).offset().top;
                var scrolls = $(this).scrollTop();
                if (scrolls > top) {
                    if (window.XMLHttpRequest) {
                        element.css({
                            position: "fixed",
                            top: 0
                        });
                    } else {
                        element.css({
                            top: scrolls
                        });
                    }
                } else {
                    element.css({
                        position: "absolute",
                        top: top
                    });
                }
            });
        }
    };
})(jQuery);