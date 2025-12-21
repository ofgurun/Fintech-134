// ============================================================================
// DASHBOARD - JAVASCRIPT
// ============================================================================
// Dashboard layout interactivity
// Sidebar toggle, notifications, etc.
// ============================================================================

(function ($) {
    'use strict';

    // ========================================================================
    // DOM READY
    // ========================================================================
    $(document).ready(function () {
        init_dashboard();
    });

    // ========================================================================
    // INITIALIZATION
    // ========================================================================
    function init_dashboard() {
        init_sidebar_toggle();
        init_notifications();
        set_active_menu();
    }

    // ========================================================================
    // SIDEBAR TOGGLE (Mobile)
    // ========================================================================
    function init_sidebar_toggle() {
        // Create mobile toggle button if needed
        if (window.innerWidth <= 768) {
            const $dashboard_main = $('.dashboard_main');
            const $sidebar = $('.dashboard_sidebar');
            
            // Toggle sidebar on mobile
            $(document).on('click', '[data-toggle-sidebar]', function() {
                $sidebar.toggleClass('dashboard_sidebar--open');
            });
            
            // Close sidebar when clicking outside
            $(document).on('click', function(e) {
                if (!$(e.target).closest('.dashboard_sidebar, [data-toggle-sidebar]').length) {
                    $sidebar.removeClass('dashboard_sidebar--open');
                }
            });
        }
    }

    // ========================================================================
    // NOTIFICATIONS
    // ========================================================================
    function init_notifications() {
        const $notification_btn = $('.dashboard_header__notification');
        
        $notification_btn.on('click', function() {
            console.log('Notifications clicked');
            // TODO: Show notifications dropdown
            alert('Bildirimler özelliği yakında eklenecek!');
        });
    }

    // ========================================================================
    // SET ACTIVE MENU ITEM
    // ========================================================================
    function set_active_menu() {
        const current_path = window.location.pathname;
        const $menu_items = $('.dashboard_sidebar__menu_item');
        
        $menu_items.each(function() {
            const $link = $(this).find('.dashboard_sidebar__menu_link');
            const link_path = $link.attr('href');
            
            if (link_path && current_path.includes(link_path)) {
                $menu_items.removeClass('dashboard_sidebar__menu_item--active');
                $(this).addClass('dashboard_sidebar__menu_item--active');
            }
        });
    }

})(jQuery);

// ============================================================================
// END OF DASHBOARD JAVASCRIPT
// ============================================================================

