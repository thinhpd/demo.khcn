/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/


CKEDITOR.on('dialogDefinition', function (ev) {
    var dialogName = ev.data.name;
    var dialogDefinition = ev.data.definition;
    var dialog = dialogDefinition.dialog;
    var editor = ev.editor;

    if (dialogName == 'image') {
        dialogDefinition.onOk = function (e) {

            //var infoTab = dialogDefinition.getContents('info');
            //var altText = infoTab.get("txtAlt");
            var dialog = CKEDITOR.dialog.getCurrent();
            var altText = dialog.getContentElement('info', 'txtAlt').getValue();

            var imageSrcUrl = e.sender.originalElement.$.src;
            var prefix = window.location.protocol + "//" + window.location.host;
            imageSrcUrl = imageSrcUrl.replace(prefix, '');

            var width = e.sender.originalElement.$.width;
            var height = e.sender.originalElement.$.height;

            var imgHtml = '';
            if (altText == '') {
                imgHtml = CKEDITOR.dom.element.createFromHtml('<table class="tbl-img" style="width:100%; text-align:center; border:0px;" border="0"><tr><td><img style="width:' + width + ';height:' + height + '" src="' + imageSrcUrl + '" alt="" /></td></tr></table>');
            }
            else {
                imgHtml = CKEDITOR.dom.element.createFromHtml('<table class="tbl-img" style="width:100%; text-align:center; border:0px;" border="0"><tr><td><img style="width:' + width + ';height:' + height + '" src="' + imageSrcUrl + '" alt="" /></td></tr><tr><td><i>' + altText + '</i></td></tr></table>');
            }
            editor.insertElement(imgHtml);
        };
    }
});

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.allowedContent = true;
	config.fullPage = false;
    config.language = 'vi';
    // config.uiColor = '#AADC6E';
    //  config.toolbar =
    //[
    //   ['Source', '-', 'Bold', 'Italic', 'syntaxhighlight']
    //];
    config.filebrowserBrowseUrl = '/Content/Admin/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Content/Admin/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/Content/Admin/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/Content/Admin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Content/Admin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/Content/Admin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.toolbar = 'Custom';

    config.toolbar_Custom = [
		// ['Source'],
		// ['Maximize'],
		// ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
		// ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
		// ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
		// ['SpecialChar'],
		// '/',
		// ['Undo', 'Redo'],
		// ['Font', 'FontSize'],
		// ['TextColor', 'BGColor'],
		// ['Link', 'Unlink', 'Anchor'],
		// ['Image', 'Table', 'HorizontalRule']
		
		['SelectAll', 'RemoveFormat'],
		['Link', 'Unlink', 'Anchor'],
		['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
		['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
		['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
		['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'],
		['Styles', 'Format', 'Font', 'FontSize'],
		['TextColor'],
		['Maximize', 'ShowBlocks'],
        ['Source', '-', 'Preview', '-', 'Templates'],
		['PasteText', 'PasteFromWord', '-', 'Scayt']
    ];
	config.toolbar_full =
	[
		['source', '-', 'save', 'newpage', 'preview', '-', 'templates'],
		['cut', 'copy', 'paste', 'pastetext', 'pastefromword', '-', 'print', 'spellchecker', 'scayt'],
		['undo', 'redo', '-', 'find', 'replace', '-', 'selectall', 'removeformat'],
		['form', 'checkbox', 'radio', 'textfield', 'textarea', 'select', 'button', 'imagebutton', 'hiddenfield'],
		'/',
		['bold', 'italic', 'underline', 'strike', '-', 'subscript', 'superscript'],
		['numberedlist', 'bulletedlist', '-', 'outdent', 'indent', 'blockquote'],
		['justifyleft', 'justifycenter', 'justifyright', 'justifyblock'],
		['link', 'unlink', 'anchor'],
		['image', 'flash', 'table', 'horizontalrule', 'smiley', 'specialchar', 'pagebreak'],
		'/',
		['styles', 'format', 'font', 'fontsize'],
		['textcolor', 'bgcolor'],
		['maximize', 'showblocks', '-', 'about']
	];

    config.toolbar_Qportal =
	[
		['Source', '-', 'Preview', '-', 'Templates'],
		['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'SpellChecker', 'Scayt'],
		['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
		'/',
		['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
		['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
		'/',
		['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
		['Link', 'Unlink', 'Anchor'],
		['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'],
		'/',
		['Styles', 'Format', 'Font', 'FontSize'],
		['TextColor', 'BGColor'],
		['Maximize', 'ShowBlocks', '-', 'About']
	];

    config.toolbar_Basic =
	[
       ['Font', 'FontSize', 'Bold', 'Italic', 'Underline', 'StrikeThrough', '-', 'Outdent', 'Indent', '-', 'NumberedList', 'BulletedList', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', 'Image', 'Table', '-', 'Link', 'TextColor', 'Source']
	];
};
