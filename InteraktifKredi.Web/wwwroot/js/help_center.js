// ============================================================================
// HELP CENTER PAGE - İNTERAKTİF KREDİ
// ============================================================================
// jQuery ile akordiyon ve arama fonksiyonları
// ============================================================================

(function($) {
    'use strict';

    // ============================================================================
    // SELECTOR CACHING
    // ============================================================================
    var $faq_items = $('.faq_item');
    var $faq_questions = $('.faq_item__question');
    var $faq_accordion = $('#faq_accordion');

    // ============================================================================
    // ACCORDION FUNCTIONALITY
    // ============================================================================

    /**
     * Akordiyon açma/kapama fonksiyonu
     */
    function toggle_accordion($item) {
        var is_active = $item.hasClass('faq_item--active');
        
        // Eğer tıklanan item zaten aktifse, kapat
        if (is_active) {
            $item.removeClass('faq_item--active');
            $item.find('.faq_item__question').attr('aria-expanded', 'false');
        } else {
            // Diğer tüm itemları kapat
            $faq_items.removeClass('faq_item--active');
            $faq_questions.attr('aria-expanded', 'false');
            
            // Tıklanan itemı aç
            $item.addClass('faq_item--active');
            $item.find('.faq_item__question').attr('aria-expanded', 'true');
        }
    }

    /**
     * Akordiyon event handler
     */
    $faq_questions.on('click', function(e) {
        e.preventDefault();
        var $item = $(this).closest('.faq_item');
        toggle_accordion($item);
    });

    // ============================================================================
    // KEYBOARD NAVIGATION
    // ============================================================================

    /**
     * Klavye ile akordiyon navigasyonu
     */
    $faq_questions.on('keydown', function(e) {
        var $current_item = $(this).closest('.faq_item');
        var $all_items = $faq_items;
        var current_index = $all_items.index($current_item);

        if (e.key === 'ArrowDown') {
            e.preventDefault();
            var next_index = (current_index + 1) % $all_items.length;
            $all_items.eq(next_index).find('.faq_item__question').focus();
        } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            var prev_index = (current_index - 1 + $all_items.length) % $all_items.length;
            $all_items.eq(prev_index).find('.faq_item__question').focus();
        } else if (e.key === 'Home') {
            e.preventDefault();
            $all_items.first().find('.faq_item__question').focus();
        } else if (e.key === 'End') {
            e.preventDefault();
            $all_items.last().find('.faq_item__question').focus();
        }
    });

    // ============================================================================
    // INITIALIZATION
    // ============================================================================

    /**
     * Sayfa yüklendiğinde başlangıç ayarları
     */
    $(document).ready(function() {
        console.log('Help Center page loaded');
        
        // Tüm akordiyonları kapalı başlat
        $faq_items.removeClass('faq_item--active');
        $faq_questions.attr('aria-expanded', 'false');
    });

})(jQuery);

