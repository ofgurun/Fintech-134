// ============================================================================
// OTP VERIFY PAGE - JAVASCRIPT
// ============================================================================
// Timer countdown: 180 saniye
// Input handling: Numeric only, auto-focus
// Resend functionality
// ============================================================================

(function ($) {
    'use strict';

    // Configuration
    // TODO: Production'da 180 saniye olacak
    const TIMER_DURATION = 30; // 30 seconds for TESTING (change to 180 in production)
    let timer_interval = null;
    let remaining_time = TIMER_DURATION;

    // ========================================================================
    // DOM READY
    // ========================================================================
    $(document).ready(function () {
        init_otp_page();
    });

    // ========================================================================
    // INITIALIZATION
    // ========================================================================
    function init_otp_page() {
        init_timer();
        init_code_input();
        init_resend_button();
        init_form_validation();
    }

    // ========================================================================
    // TIMER - Countdown 30 seconds (TEST MODE)
    // ========================================================================
    function init_timer() {
        // TEST MODE: Enable resend button immediately for testing
        const $resend_button = $('#resend_code');
        $resend_button.prop('disabled', false);
        
        console.log('⚠️ TEST MODE: Resend button enabled immediately');
        console.log('⚠️ Timer duration: 30 seconds (change to 180 in production)');
        
        start_timer();
    }

    function start_timer() {
        remaining_time = TIMER_DURATION;
        update_timer_display();

        // Clear existing interval
        if (timer_interval) {
            clearInterval(timer_interval);
        }

        // Start new interval
        timer_interval = setInterval(function () {
            remaining_time--;
            update_timer_display();

            if (remaining_time <= 0) {
                timer_expired();
            }
        }, 1000);
    }

    function update_timer_display() {
        const $timer_value = $('#timer_value');
        const $timer_container = $('.otp_timer');

        $timer_value.text(remaining_time);

        // Visual warning when time is running out
        if (remaining_time <= 30) {
            $timer_container.addClass('otp_timer--expired');
        } else {
            $timer_container.removeClass('otp_timer--expired');
        }
    }

    function timer_expired() {
        clearInterval(timer_interval);

        const $timer_value = $('#timer_value');
        const $resend_button = $('#resend_code');

        $timer_value.text('0');
        $('.otp_timer').addClass('otp_timer--expired');

        // Enable resend button
        $resend_button.prop('disabled', false);

        console.log('OTP Timer expired');
    }

    // ========================================================================
    // CODE INPUT - Numeric Only + Auto Submit on 6 digits
    // ========================================================================
    function init_code_input() {
        const $code_input = $('#code');
        const $code_group = $('#code_group');

        // Numeric only input
        $code_input.on('input', function () {
            let value = $(this).val();

            // Remove non-numeric characters
            value = value.replace(/[^0-9]/g, '');

            // Limit to 6 digits
            if (value.length > 6) {
                value = value.substring(0, 6);
            }

            $(this).val(value);

            // Visual feedback
            if (value.length === 6) {
                $code_group.addClass('input_group--success');
            } else {
                $code_group.removeClass('input_group--success');
            }
        });

        // Paste handling
        $code_input.on('paste', function (e) {
            e.preventDefault();

            const paste_data = (e.originalEvent || e).clipboardData.getData('text/plain');
            const numeric_data = paste_data.replace(/[^0-9]/g, '').substring(0, 6);

            $(this).val(numeric_data).trigger('input');
        });

        // Focus state tracking
        $code_input.on('focus', function () {
            $code_group.addClass('input_group--focus');
        });

        $code_input.on('blur', function () {
            $code_group.removeClass('input_group--focus');
        });

        // Auto-focus on load
        $code_input.focus();
    }

    // ========================================================================
    // RESEND BUTTON
    // ========================================================================
    function init_resend_button() {
        const $resend_button = $('#resend_code');

        $resend_button.on('click', function () {
            if ($(this).prop('disabled')) {
                return;
            }

            resend_otp_code();
        });
    }

    function resend_otp_code() {
        const $resend_button = $('#resend_code');
        const $form = $('.otp_form');
        
        console.log('Resending OTP code via AJAX...');

        // Disable button and show loading state
        $resend_button.prop('disabled', true).text('Gönderiliyor...');

        // Get anti-forgery token
        var token = $('input[name="__RequestVerificationToken"]').val();
        
        // Get TCKN and GSM from hidden fields
        var tckn = $form.find('input[name="TCKN"]').val();
        var gsm = $form.find('input[name="GSM"]').val();
        
        console.log('Form data - TCKN:', tckn, 'GSM:', gsm);

        // Prepare form data
        var formData = new FormData();
        formData.append('TCKN', tckn);
        formData.append('GSM', gsm);

        // AJAX call to backend
        $.ajax({
            url: '/Auth/OtpVerify?handler=ResendOtp',
            type: 'POST',
            headers: {
                'RequestVerificationToken': token
            },
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    // Reset timer
                    start_timer();

                    // Reset button text
                    $resend_button.text('Yeniden Gönder');

                    // Show success message
                    show_temporary_message('SMS şifresi tekrar gönderildi.', 'success');

                    console.log('✅ OTP resent successfully');
                } else {
                    // Show error message
                    show_temporary_message(response.message || 'SMS gönderilemedi.', 'error');
                    
                    // Re-enable button
                    $resend_button.prop('disabled', false).text('Yeniden Gönder');
                    
                    console.error('❌ Resend OTP failed:', response.message);
                }
            },
            error: function (xhr, status, error) {
                // Show error message
                show_temporary_message('Bir hata oluştu. Lütfen tekrar deneyin.', 'error');
                
                // Re-enable button
                $resend_button.prop('disabled', false).text('Yeniden Gönder');
                
                console.error('❌ AJAX error:', error);
                console.error('Response:', xhr.responseText);
            }
        });
    }

    // ========================================================================
    // FORM VALIDATION - Custom validation states
    // ========================================================================
    function init_form_validation() {
        const $form = $('.otp_form');
        const $code_input = $('#code');
        const $code_group = $('#code_group');

        // Form submit handling
        $form.on('submit', function (e) {
            // jQuery validation will handle this
            // Add loading state to button
            const $submit_button = $form.find('button[type="submit"]');
            $submit_button.prop('disabled', true).text('Doğrulanıyor...');

            // Re-enable after validation
            setTimeout(function () {
                if (!$form.valid()) {
                    $submit_button.prop('disabled', false).text('Devam Et');
                }
            }, 100);
        });

        // Validation error handling
        $code_input.on('invalid', function () {
            $code_group.addClass('input_group--error');
        });

        $code_input.on('input', function () {
            if ($(this).val().length > 0) {
                $code_group.removeClass('input_group--error');
            }
        });
    }

    // ========================================================================
    // UTILITY - Show Temporary Message
    // ========================================================================
    function show_temporary_message(message, type) {
        const $message_container = $('<div>')
            .addClass('alert alert--' + type)
            .text(message)
            .hide()
            .fadeIn(300);

        $('.otp_form').prepend($message_container);

        setTimeout(function () {
            $message_container.fadeOut(300, function () {
                $(this).remove();
            });
        }, 4000);
    }

    // ========================================================================
    // CLEANUP - Clear interval on page unload
    // ========================================================================
    $(window).on('beforeunload', function () {
        if (timer_interval) {
            clearInterval(timer_interval);
        }
    });

})(jQuery);

// ============================================================================
// END OF OTP VERIFY PAGE JAVASCRIPT
// ============================================================================

