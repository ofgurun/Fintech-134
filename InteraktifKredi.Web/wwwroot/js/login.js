// ============================================================================
// LOGIN PAGE - CLIENT-SIDE İNTERAKSİYON
// ============================================================================
// Form validasyonu ve input formatlaması
// ============================================================================

$(document).ready(function () {
    // ----------------------------------------------------------------------------
    // TCKN INPUT - Sadece Rakam Girişi
    // ----------------------------------------------------------------------------
    $('#tckn').on('input', function () {
        // Sadece rakam kabul et
        var value = $(this).val().replace(/[^0-9]/g, '');
        $(this).val(value);
        
        // Validation state
        update_validation_state('#tckn_group', $(this));
    });

    // ----------------------------------------------------------------------------
    // GSM INPUT - Sadece Rakam Girişi ve Otomatik Formatlama
    // ----------------------------------------------------------------------------
    $('#gsm').on('input', function () {
        // Sadece rakam kabul et
        var value = $(this).val().replace(/[^0-9]/g, '');
        $(this).val(value);
        
        // Validation state
        update_validation_state('#gsm_group', $(this));
    });

    // ----------------------------------------------------------------------------
    // VALIDATION STATE HELPER
    // ----------------------------------------------------------------------------
    function update_validation_state(group_selector, $input) {
        var $group = $(group_selector);
        var value = $input.val();
        var field_name = $input.attr('id');

        // Remove all states
        $group.removeClass('input_group--error input_group--success');

        if (value.length > 0) {
            // TCKN Validation
            if (field_name === 'tckn') {
                if (value.length === 11 && /^\d{11}$/.test(value)) {
                    $group.addClass('input_group--success');
                } else if (value.length >= 11) {
                    $group.addClass('input_group--error');
                }
            }

            // GSM Validation
            if (field_name === 'gsm') {
                if (value.length === 10 && /^5\d{9}$/.test(value)) {
                    $group.addClass('input_group--success');
                } else if (value.length >= 10 || (value.length > 0 && value[0] !== '5')) {
                    $group.addClass('input_group--error');
                }
            }
        }
    }

    // ----------------------------------------------------------------------------
    // FORM SUBMIT - Loading State
    // ----------------------------------------------------------------------------
    $('.auth_form').on('submit', function (e) {
        var $form = $(this);
        var $submit_btn = $form.find('button[type="submit"]');

        // jQuery Validation kontrolü
        if ($form.valid()) {
            // Loading state ekle
            $submit_btn.addClass('btn_primary--loading');
            $submit_btn.prop('disabled', true);
        }
    });

    // ----------------------------------------------------------------------------
    // FOCUS STATE - Input Group Vurgulaması
    // ----------------------------------------------------------------------------
    $('.input_group__field').on('focus', function () {
        $(this).closest('.input_group').addClass('input_group--focus');
    });

    $('.input_group__field').on('blur', function () {
        $(this).closest('.input_group').removeClass('input_group--focus');
    });

    // ----------------------------------------------------------------------------
    // JQUERY VALIDATION - Custom Error Placement
    // ----------------------------------------------------------------------------
    var $form = $('.auth_form');
    if ($form.length > 0) {
        $form.validate({
            errorClass: 'input_group__error',
            validClass: 'input_group__valid',
            errorPlacement: function (error, element) {
                // Hata mesajını input_group içine yerleştir
                error.insertAfter(element);
                element.closest('.input_group').addClass('input_group--error');
            },
            success: function (label, element) {
                // Başarılı validasyon
                $(element).closest('.input_group').removeClass('input_group--error');
            },
            highlight: function (element) {
                $(element).closest('.input_group').addClass('input_group--error');
            },
            unhighlight: function (element) {
                $(element).closest('.input_group').removeClass('input_group--error');
            }
        });
    }

    // ----------------------------------------------------------------------------
    // BENI HATIRLA - LocalStorage Desteği (İsteğe Bağlı)
    // ----------------------------------------------------------------------------
    var $remember_me = $('input[name="remember_me"]');
    var $tckn_input = $('#tckn');

    // Sayfa yüklendiğinde hatırlanan TCKN'yi doldur
    if (localStorage.getItem('remember_tckn') === 'true') {
        var saved_tckn = localStorage.getItem('saved_tckn');
        if (saved_tckn) {
            $tckn_input.val(saved_tckn);
            $remember_me.prop('checked', true);
        }
    }

    // Form submit edildiğinde TCKN'yi kaydet
    $('.auth_form').on('submit', function () {
        if ($remember_me.is(':checked')) {
            localStorage.setItem('remember_tckn', 'true');
            localStorage.setItem('saved_tckn', $tckn_input.val());
        } else {
            localStorage.removeItem('remember_tckn');
            localStorage.removeItem('saved_tckn');
        }
    });
});

// ============================================================================
// END OF LOGIN JS
// ============================================================================

