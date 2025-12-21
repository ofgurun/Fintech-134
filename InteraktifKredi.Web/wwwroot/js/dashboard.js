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
    // SELECTOR CACHING
    // ========================================================================
    var $dashboard_sidebar = $('#dashboard_sidebar');
    var $dashboard_main = $('.dashboard_main');

    // ========================================================================
    // SELECTOR CACHING - Performance için
    // ========================================================================
    var $body = $('body');
    var $menu_links = $('.dashboard_sidebar__menu_link');
    var $logout_btn = $('.dashboard_sidebar__logout_btn');
    var $menu_items = $('.dashboard_sidebar__menu_item');

    // ========================================================================
    // SIDEBAR HOVER TOGGLE (Desktop - Mouse ile açılma/kapanma)
    // ========================================================================
    function init_sidebar_toggle() {
        // Desktop hover efekti
        if (window.innerWidth > 768) {
            // Sidebar varsayılan olarak collapsed
            $dashboard_sidebar.addClass('dashboard_sidebar--collapsed');
            $body.addClass('sidebar_collapsed');

            // Hover ile açılma
            $dashboard_sidebar.on('mouseenter', function() {
                $dashboard_sidebar.removeClass('dashboard_sidebar--collapsed');
                $body.removeClass('sidebar_collapsed');
                
                // Geçiş tamamlandığında içerik genişliğini kontrol et
                setTimeout(function() {
                    optimize_content_layout();
                }, 300);
            });

            // Mouse çekilince kapanma
            $dashboard_sidebar.on('mouseleave', function() {
                $dashboard_sidebar.addClass('dashboard_sidebar--collapsed');
                $body.addClass('sidebar_collapsed');
                
                // Geçiş tamamlandığında içerik genişliğini kontrol et
                setTimeout(function() {
                    optimize_content_layout();
                }, 300);
            });
        }

        // Mobile toggle (existing functionality)
        if (window.innerWidth <= 768) {
            // Toggle sidebar on mobile
            $(document).on('click', '[data-toggle-sidebar]', function() {
                $dashboard_sidebar.toggleClass('dashboard_sidebar--open');
            });
            
            // Close sidebar when clicking outside
            $(document).on('click', function(e) {
                if (!$(e.target).closest('.dashboard_sidebar, [data-toggle-sidebar]').length) {
                    $dashboard_sidebar.removeClass('dashboard_sidebar--open');
                }
            });
        }
    }

    // ========================================================================
    // OPTIMIZE CONTENT LAYOUT
    // ========================================================================
    function optimize_content_layout() {
        // Grid yapılarını kontrol et ve güncelle
        var $grids = $('.dashboard_reports, [class*="grid"], [class*="card"]');
        
        $grids.each(function() {
            var $grid = $(this);
            var current_width = $grid.width();
            
            // Grid genişliğini yeniden hesapla
            if ($grid.css('display') === 'grid') {
                $grid.css('grid-template-columns', '');
                // Browser otomatik olarak yeniden hesaplayacak
            }
        });

        // Tabloları kontrol et
        var $tables = $('table');
        $tables.each(function() {
            var $table = $(this);
            // Tablo genişliğini optimize et
            if ($table.hasClass('table-responsive')) {
                $table.css('width', '100%');
            }
        });
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

