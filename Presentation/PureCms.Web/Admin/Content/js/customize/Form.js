if (typeof (Xms) == "undefined")
{ Xms = { __namespace: true }; }
Xms.FormControlType.prototype = {
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
Xms.FormDescriptor = function () {
    
};
Xms.NavGroupDescriptor = function () {

};
Xms.NavDescriptor = function () {

};
Xms.PanelDescriptor = function () {

};
Xms.SectionDescriptor = function () {

};
Xms.RowDescriptor = function () {

};
Xms.CellDescriptor = function () {

};
Xms.ControlDescriptor = function () {

};

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