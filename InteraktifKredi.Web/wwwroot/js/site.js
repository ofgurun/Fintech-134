// ============================================================================
// SITE.JS - İNTERAKTİF KREDİ
// ============================================================================
// Global JavaScript/jQuery kodları
// Selector caching ve event delegation kullanılmalı
// ============================================================================

(function($) {
    'use strict';

    // ------------------------------------------------------------------------
    // DOCUMENT READY
    // ------------------------------------------------------------------------
    
    $(document).ready(function() {
        init_header();
        init_user_menu();
    });

    // ------------------------------------------------------------------------
    // HEADER - MOBILE MENU TOGGLE
    // ------------------------------------------------------------------------
    
    function init_header() {
        // Selector caching
        var $header_toggle = $('#header_toggle');
        var $header_nav = $('#header_nav');
        var $header_overlay = $('#header_overlay');
        var $body = $('body');
        
        // Mobile menu toggle
        $header_toggle.on('click', function() {
            var is_active = $header_toggle.hasClass('header__toggle--active');
            
            if (is_active) {
                close_mobile_menu();
            } else {
                open_mobile_menu();
            }
        });
        
        // Overlay click - menüyü kapat
        $header_overlay.on('click', function() {
            close_mobile_menu();
        });
        
        // ESC tuşu ile menüyü kapat
        $(document).on('keydown', function(e) {
            if (e.key === 'Escape' && $header_toggle.hasClass('header__toggle--active')) {
                close_mobile_menu();
            }
        });
        
        // Window resize - desktop'a geçildiğinde menüyü kapat
        $(window).on('resize', function() {
            if ($(window).width() > 768) {
                close_mobile_menu();
            }
        });
        
        // Nav link click - mobilde menüyü kapat
        $header_nav.on('click', '.header__nav_link', function() {
            if ($(window).width() <= 768) {
                close_mobile_menu();
            }
        });
        
        // --------------------------------------------------------------------
        // HELPER FUNCTIONS
        // --------------------------------------------------------------------
        
        function open_mobile_menu() {
            $header_toggle.addClass('header__toggle--active');
            $header_toggle.attr('aria-expanded', 'true');
            $header_nav.addClass('header__nav--open');
            $header_overlay.addClass('header__overlay--active');
            $body.css('overflow', 'hidden');
        }
        
        function close_mobile_menu() {
            $header_toggle.removeClass('header__toggle--active');
            $header_toggle.attr('aria-expanded', 'false');
            $header_nav.removeClass('header__nav--open');
            $header_overlay.removeClass('header__overlay--active');
            $body.css('overflow', '');
        }
    }

    // ------------------------------------------------------------------------
    // USER MENU - DROPDOWN TOGGLE
    // ------------------------------------------------------------------------
    
    function init_user_menu() {
        // Selector caching
        var $user_menu_toggle = $('#user_menu_toggle');
        var $user_dropdown = $('#user_dropdown');
        var is_open = false;
        
        // Toggle dropdown
        $user_menu_toggle.on('click', function(e) {
            e.stopPropagation();
            
            if (is_open) {
                close_user_dropdown();
            } else {
                open_user_dropdown();
            }
        });
        
        // Dışarı tıklandığında dropdown'ı kapat
        $(document).on('click', function(e) {
            if (!$(e.target).closest('.header__user').length) {
                close_user_dropdown();
            }
        });
        
        // ESC tuşu ile dropdown'ı kapat
        $(document).on('keydown', function(e) {
            if (e.key === 'Escape' && is_open) {
                close_user_dropdown();
            }
        });
        
        // Dropdown link click - dropdown'ı kapat
        $user_dropdown.on('click', '.header__user_dropdown_item', function() {
            close_user_dropdown();
        });
        
        // --------------------------------------------------------------------
        // HELPER FUNCTIONS
        // --------------------------------------------------------------------
        
        function open_user_dropdown() {
            $user_dropdown.addClass('header__user_dropdown--open');
            is_open = true;
        }
        
        function close_user_dropdown() {
            $user_dropdown.removeClass('header__user_dropdown--open');
            is_open = false;
        }
    }

    // ------------------------------------------------------------------------
    // UTILITY FUNCTIONS
    // ------------------------------------------------------------------------
    
    // Smooth scroll (gerekirse)
    function smooth_scroll_to(target) {
        $('html, body').animate({
            scrollTop: $(target).offset().top - 72 // Header height
        }, 300);
    }

})(jQuery);
