// ============================================================================
// PROFILE TABS - JAVASCRIPT
// ============================================================================
// Tab switching and form state management
// ============================================================================

(function ($) {
    'use strict';

    // ========================================================================
    // DOM READY
    // ========================================================================
    $(document).ready(function () {
        init_profile_tabs();
        init_form_loading_state();
    });

    // ========================================================================
    // TAB INITIALIZATION
    // ========================================================================
    function init_profile_tabs() {
        // Tab navigation click handler
        $('.profile_tabs__nav_item').on('click', function () {
            var tab_id = $(this).data('tab');
            switch_tab(tab_id);
        });

        console.log('Profile tabs initialized');
    }

    // ========================================================================
    // SWITCH TAB
    // ========================================================================
    window.switch_tab = function(tab_id) {
        // Remove active class from all nav items
        $('.profile_tabs__nav_item').removeClass('profile_tabs__nav_item--active');

        // Add active class to clicked nav item
        $('.profile_tabs__nav_item[data-tab="' + tab_id + '"]').addClass('profile_tabs__nav_item--active');

        // Hide all panels
        $('.profile_tabs__panel').removeClass('profile_tabs__panel--active');

        // Show selected panel
        $('.profile_tabs__panel[data-panel="' + tab_id + '"]').addClass('profile_tabs__panel--active');

        console.log('Switched to tab:', tab_id);
    };

    // ========================================================================
    // FORM LOADING STATE
    // ========================================================================
    function init_form_loading_state() {
        $('.profile_form').on('submit', function () {
            var $form = $(this);
            var $submit_btn = $form.find('button[type="submit"]');
            var loading_text = $submit_btn.data('loading-text') || 'Kaydediliyor...';
            var original_text = $submit_btn.text();

            // Disable button and change text
            $submit_btn.prop('disabled', true).text(loading_text);

            // Store original text for potential restore
            $submit_btn.data('original-text', original_text);

            console.log('Form submitted, button disabled');
        });
    }

    // ========================================================================
    // AUTO-DISMISS ALERTS
    // ========================================================================
    if ($('.alert').length) {
        setTimeout(function () {
            $('.alert').fadeOut(400, function () {
                $(this).remove();
            });
        }, 5000); // 5 seconds
    }

})(jQuery);

// ============================================================================
// END OF PROFILE TABS JAVASCRIPT
// ============================================================================

