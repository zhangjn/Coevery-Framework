/*
 * MoonCake v1.3.1 - Template Setup JS
 *
 * This file is part of MoonCake, an Admin template build for sale at ThemeForest.
 * For questions, suggestions or support request, please mail me at maimairel@yahoo.com
 *
 * Development Started:
 * July 28, 2012
 * Last Update:
 * December 07, 2012
 *
 */

;(function( $, window, document, undefined ) {


    $(document).ready(function(e) {

        // Restrict any other content beside numbers
        $(":input[data-accept=numbers]").keydown(function(event) {

            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            } else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });

        // Initialize Bootstrap Tooltips
        $.fn.tooltip && $('[rel="tooltip"]').tooltip();

        // Initialize Bootstrap Popovers
        $.fn.popover && $('[rel="popover"]').popover();

        // IE Placeholder
        $.fn.placeholder && $('[placeholder]').placeholder();

        // Bootstrap Dropdown Workaround for touch devices
        $(document).on('touchstart.dropdown.data-api', '.dropdown-menu', function(e) { e.stopPropagation(); });

        // Gridview row menu
        $(document)
            .on('mouseover tr', '.ui-jqgrid-btable tr', function (e) {
                $(this).find('.row-actions').removeClass('hide');
            });
        $(document)
            .on('mouseout tr', '.ui-jqgrid-btable tr', function (e) {
                $(this).find('.row-actions').addClass('hide');
            });
        
        // Initialize DateTime Picker
        if ($.fn.datetimepicker) {
            $('[data-co-datetime-picker="date"]').datetimepicker({ pickTime: false });
            $('[data-co-datetime-picker="datetime"]').datetimepicker({ pickSeconds: false });
        }

        // Custom validate methods
        $.validator.addMethod("phonenumber", function (value) {
            if (value) {
                return (/^\d{8,12}|(((\(\d{3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7})$/.test(value));
            }
            return true;
        }, "Please enter a valid phone.");
        $.validator.classRuleSettings.phonenumber = { phonenumber: true };

        $.validator.addMethod("decimalFormat", function (value, element, params) {
            var paramArray = params.split(',');
            var tempInt = parseInt(value);
            return !value || !isNaN(tempInt) && tempInt.toString().length <= paramArray[0] &&
                RegExp("^(\\d{0," + paramArray[0] + "})(\.\\d{0," + paramArray[1] + "})?$").test(value);
        }, "Wrong decimal format!");
        $.validator.classRuleSettings.decimalFormat = { decimalFormat: true };

        $.validator.addMethod("decimalplaces", function (value, em) {
            if (value) {
                var len = $(em).attr("decimalplaces");
                var reg = new RegExp("^(-?\\d+)(\\.\\d{1," + len + "})?$");
                return reg.test(value);
            }
            return true;
        }, $.validator.format("Decimal places exceed {0}."));
        $.validator.classRuleSettings.decimalplaces = {
            decimalplaces: function (em) {
                return $(em).attr("decimalplaces");
            }
        };
        
        // FIeld help text
        $('[help-text]').each(function () {
            var element = $(this);
            element.attr('rel', 'popover');
            element.attr('data-html', 'true');
            element.attr('data-placement', 'top');
            element.attr('data-content', '<p class="popoverTipContent">' + element.attr('help-text') + '</p>');
            element.attr('original-title', '');
            var icon = document.createElement("i");
            icon.className = "icon-question-sign popoverTipIcon";
            icon.id = "popoverIcon";
            element.parents("div").first().get(0).appendChild(icon);
            $(icon).mouseover(function () {
                element.popover('show');
                element.focus();
            });

            $(icon).mouseout(function () {
                element.popover('destroy');
                element.blur();
            });
        });
    });

}) (jQuery, window, document);