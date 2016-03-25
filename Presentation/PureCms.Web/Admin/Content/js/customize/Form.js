if (typeof (Xms) == "undefined")
{ Xms = { __namespace: true }; }
Xms.Form = function () { };
Xms.Form.FormControlType = function () { };
Xms.Form.FormControlType.prototype = {
    none: -1,
    standard: 0,
    hidden: 1,
    iFrame: 2,
    lookup: 3,
    optionSet: 4,
    subGrid: 5,
    webResource: 6
};
//models
Xms.Form.FormDescriptor = function () {
    var self = new Object();
    self.Name = '';
    self.Description = '';
    self.IsShowNav = true;
    self.Header = null;
    self.Footer = null;
    self.NavGroups = [];
    self.Panels = [];
    self.Sections = [];
    return self;
};
Xms.Form.NavGroupDescriptor = function () {
    var self = new Object();
    self.Label = '';
    self.IsVisibled = true;
    self.NavItems = [];
    return self;
};
Xms.Form.NavDescriptor = function () {
    var self = new Object();
    self.Label = '';
    self.IsVisibled = true;
    self.Icon = '';
    self.Url = '';
    return self;
};
Xms.Form.PanelDescriptor = function () {
    var self = new Object();
    self.Name = '';
    self.Label = '';
    self.IsExpanded = true;
    self.IsShowLabel = true;
    self.IsVisibled = true;
    self.Sections = [];
    return self;
};
Xms.Form.SectionDescriptor = function () {
    var self = new Object();
    self.Name = '';
    self.Label = '';
    self.IsShowLabel = true;
    self.IsVisibled = true;
    self.Columns = 2;
    self.CellLabelWidth = 100;
    self.CellLabelAlignment = 'left';
    self.CellLabelPosition = 'left';
    self.Rows = [];
    return self;
};
Xms.Form.RowDescriptor = function () {
    var self = new Object();
    self.IsVisibled = true;
    self.Cells = [];
    return self;
};
Xms.Form.CellDescriptor = function () {
    var self = new Object();
    self.Label = '';
    self.IsShowLabel = true;
    self.IsVisibled = true;
    self.ColSpan = 0;
    self.RowSpan = 0;
    self.Control = new Xms.Form.ControlDescriptor();
    return self;
};
Xms.Form.ControlDescriptor = function () {
    var self = new Object();
    self.Name = '';
    self.EntityName = '';
    return self;
};
//const
Xms.AttributeFormat = function () { };
Xms.AttributeType = function () { };
Xms.BooleanFormat = function () { };
Xms.ControlType = function () { };
Xms.FormSaveAction = function () { };
Xms.RequiredLevel = function () { };
Xms.SubmitMode = function () { };
Xms.TabDisplayState = function () { };
Xms.AttributeFormat.dateFormat = "date";
Xms.AttributeFormat.dateTimeFormat = "datetime";
Xms.AttributeFormat.durationFormat = "duration";
Xms.AttributeFormat.emailFormat = "email";
Xms.AttributeFormat.languageFormat = "language";
Xms.AttributeFormat.noneFormat = "none";
Xms.AttributeFormat.textFormat = "text";
Xms.AttributeFormat.textAreaFormat = "textarea";
Xms.AttributeFormat.tickerSymbolFormat = "tickersymbol";
Xms.AttributeFormat.timeZoneFormat = "timezone";
Xms.AttributeFormat.urlFormat = "url";
Xms.AttributeType.booleanType = "boolean";
Xms.AttributeType.dateTimeType = "datetime";
Xms.AttributeType.decimalType = "decimal";
Xms.AttributeType.doubleType = "double";
Xms.AttributeType.integerType = "integer";
Xms.AttributeType.lookupType = "lookup";
Xms.AttributeType.memoType = "memo";
Xms.AttributeType.moneyType = "money";
Xms.AttributeType.optionSetType = "optionset";
Xms.AttributeType.stringType = "string";
Xms.BooleanFormat.checkBox = "checkbox";
Xms.BooleanFormat.dropDown = "dropdown";
Xms.BooleanFormat.radioButton = "radiobutton";
Xms.ControlType.hidden = "hidden";
Xms.ControlType.iFrame = "iframe";
Xms.ControlType.lookup = "lookup";
Xms.ControlType.none = "none";
Xms.ControlType.optionSet = "optionset";
Xms.ControlType.standard = "standard";
Xms.ControlType.subGrid = "subgrid";
Xms.ControlType.webResource = "webresource";
Xms.FormSaveAction.save = "save";
Xms.FormSaveAction.saveAndClose = "saveandclose";
Xms.FormSaveAction.saveAndNew = "saveandnew";
Xms.RequiredLevel.none = "none";
Xms.RequiredLevel.recommended = "recommended";
Xms.RequiredLevel.required = "required";
Xms.SubmitMode.dirty = "dirty";
Xms.SubmitMode.always = "always";
Xms.SubmitMode.never = "never";
Xms.TabDisplayState.collapsed = "collapsed";
Xms.TabDisplayState.expanded = "expanded"

Xms.Form.RenderControl = function (_cell) {
    //var Render = function (_cell) {
        var _control = new Array();
        if (_cell.IsVisibled) {

        }
        if (_cell.IsShowLabel == 'true') {
            _control.push('<td class="col-sm-2">' + _cell.Label + '</td>');
            _control.push('<td>');
            _control.push('<input type="text" class="form-control input-sm" name="" id="" />');
            _control.push('</td>');
        }
        return _control.join('');
    //}
}
//Xms.Form.Create = function(){}
//create a form
Xms.Form.CreateForm = function (_form, _container) {
    var _html = new Array();
    if (_form.IsShowNav) {
        //render nav

    }
    //render header
    var _header = new Array();
    var _headerSection = _form.Header;
    _header.push('<div class="header">');
    _header.push('<table class="table">');
    $(_headerSection.Rows).each(function (i, _row) {
        _header.push('<tr>');
        $(_row.Cells).each(function (j, _cell) {
            _header.push(Xms.Form.RenderControl(_cell));
        });
        _header.push('</tr>');
    });
    _header.push('</table>');
    _header.push('</div>');
    _html.push(_header.join(''));

    var _body = new Array();
    //render tabs
    _body.push('<div class="body">');
    $(_form.Panels).each(function (a, _tab) {
        _body.push('<div class="tab">');
        $(_tab.Sections).each(function (b, _section) {
            _body.push('<div class="section">');
            if (_section.IsShowLabel) {
                _body.push('<div class="section-title">'+_section.Label+'</div>');
            }
            _body.push('<table class="table">');
            $(_section.Rows).each(function (c, _row) {
                _body.push('<tr>');
                $(_row.Cells).each(function (d, _cell) {
                    _body.push(Xms.Form.RenderControl(_cell));
                });
                _body.push('</tr>');
            });
            _body.push('</table>');
            _body.push('</div>');
        });
        _body.push('</div>');
    });
    //render sections
    $(_form.Sections).each(function (b, _section) {
        _body.push('<div class="section">');
        if (_section.IsShowLabel) {
            _body.push('<div class="section-title">' + _section.Label + '</div>');
        }
        _body.push('<table class="table">');
        $(_section.Rows).each(function (c, _row) {
            _body.push('<tr>');
            $(_row.Cells).each(function (d, _cell) {
                _body.push(Xms.Form.RenderControl(_cell));
            });
            _body.push('</tr>');
        });
        _body.push('</table>');
        _body.push('</div>');
    });
    _body.push('</div>');


    _html.push(_body.join(''));

    //render footer
    var _footer = new Array();
    var _footerSection = _form.Footer;
    _footer.push('<div class="footer">');
    _footer.push('<table class="table">');
    $(_footerSection.Rows).each(function (i, _row) {
        _footer.push('<tr>');
        $(_row.Cells).each(function (j, _cell) {
            _footer.push(Xms.Form.RenderControl(_cell));
        });
        _footer.push('</tr>');
    });
    _footer.push('</table>');
    _footer.push('</div>');
    _html.push(_footer.join(''));

    //rendering
    _container.html(_html.join(''));
}