// ============================================================================
// LOAN APPLY PAGE - JAVASCRIPT
// ============================================================================
// Kredi başvuru formu - Slider, Validation, Dynamic Fields
// ============================================================================

(function ($) {
    'use strict';

    $(document).ready(function () {
        init_loan_apply_page();
    });

    function init_loan_apply_page() {
        var $form = $('#loan_application_form');
        var $submit_btn = $('#submit_btn');
        var $submit_btn_text = $('#submit_btn_text');
        var $submit_btn_loading = $('#submit_btn_loading');
        var $loan_amount_slider = $('#loan_amount_slider');
        var $loan_amount = $('#loan_amount');
        var $loan_amount_display = $('#loan_amount_display');
        var $loan_description = $('#loan_description');
        var $char_count = $('#char_count');
        var $job_id = $('#job_id');
        var $public_employee_fields = $('#public_employee_fields');

        // Kredi tutarı formatlama
        function format_loan_amount(value) {
            return new Intl.NumberFormat('tr-TR').format(value);
        }

        // Slider değişikliği
        $loan_amount_slider.on('input', function () {
            var value = parseInt($(this).val());
            $loan_amount.val(value);
            $loan_amount_display.text(format_loan_amount(value));
        });

        // job_id.json'dan meslekleri yükle
        $.getJSON('/data/job_id.json', function (jobs) {
            $.each(jobs, function (index, job) {
                $job_id.append($('<option>', {
                    value: job.id,
                    text: job.name
                }));
            });
        }).fail(function () {
            console.error('Meslek listesi yüklenemedi');
        });

        // Meslek seçimi değiştiğinde
        $job_id.on('change', function () {
            var selected_job_id = $(this).val();
            var selected_text = $(this).find('option:selected').text();

            // Kamu Çalışanı seçildiğinde
            if (selected_job_id == '1' || selected_text === 'Kamu Çalışanı') {
                $public_employee_fields.slideDown(300);
                $('#institution_name').prop('required', true);
                $('#position').prop('required', true);
            } else {
                $public_employee_fields.slideUp(300);
                $('#institution_name').prop('required', false);
                $('#position').prop('required', false);
                $('#institution_name').val('');
                $('#position').val('');
            }
        });

        // Karakter sayacı
        $loan_description.on('input', function () {
            var length = $(this).val().length;
            $char_count.text(length);

            if (length > 500) {
                $(this).val($(this).val().substring(0, 500));
                $char_count.text('500');
            }
        });

        // Form submit
        $form.on('submit', function () {
            // Loading state
            $submit_btn.prop('disabled', true);
            $submit_btn_text.hide();
            $submit_btn_loading.show();
        });

        // Input focus states (yeni class isimleriyle)
        $('.loan_apply_input, .loan_apply_select, .loan_apply_textarea').on('focus', function () {
            $(this).addClass('loan_apply_input--focus');
        });

        $('.loan_apply_input, .loan_apply_select, .loan_apply_textarea').on('blur', function () {
            $(this).removeClass('loan_apply_input--focus');
        });
    }

})(jQuery);

// ============================================================================
// END OF LOAN APPLY PAGE JAVASCRIPT
// ============================================================================

