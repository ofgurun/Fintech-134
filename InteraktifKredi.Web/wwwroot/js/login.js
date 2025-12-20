// Login Page JavaScript
// Handles KVKK modal, form validation, and input masking

$(document).ready(function() {
    // ========================================================================
    // TCKN Input: Only Numbers
    // ========================================================================
    $('#tckn').on('input', function() {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    // ========================================================================
    // GSM Input: Only Numbers, Auto-format
    // ========================================================================
    $('#gsm').on('input', function() {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
        
    // ========================================================================
    // KVKK Modal Functions
    // ========================================================================
    
    // Global reference for callback
    window.kvkkApprovalCallback = null;
    window.kvkkData = null;
        
    /**
     * Shows KVKK Modal with text content
     * @param {string} text - KVKK text content
     * @param {number} kvkkId - KVKK document ID
     * @param {number} customerId - Customer ID
     * @param {function} onApprove - Callback function after approval
     */
    window.showKvkkModal = function(text, kvkkId, customerId, onApprove) {
        console.log('Opening KVKK Modal...');
        
        // Store callback and data
        window.kvkkApprovalCallback = onApprove;
        window.kvkkData = {
            kvkkId: kvkkId,
            customerId: customerId
        };

        // Show modal
        $('#kvkkModal').fadeIn(300);
        $('body').css('overflow', 'hidden');

        // Hide loading, show content
        $('#kvkk-loading').hide();
        $('#kvkk-content').html(text).show();

        // Enable approve button after a short delay (simulate reading time)
        setTimeout(function() {
            $('#btn-approve-kvkk').prop('disabled', false);
        }, 1000);

        // Setup scroll detection (optional - enable button only after scroll to bottom)
        setupScrollDetection();
    };

    /**
     * Closes KVKK Modal
     */
    window.closeKvkkModal = function() {
        console.log('Closing KVKK Modal...');
        $('#kvkkModal').fadeOut(300);
        $('body').css('overflow', 'auto');
        
        // Reset
        $('#kvkk-content').empty();
        $('#btn-approve-kvkk').prop('disabled', true);
        window.kvkkApprovalCallback = null;
        window.kvkkData = null;
    };

    /**
     * Setup scroll detection to enable button only after scrolling to bottom
     */
    function setupScrollDetection() {
        var $content = $('#kvkk-content');
        
        // Check if content is scrollable
        if ($content[0].scrollHeight <= $content[0].clientHeight) {
            // Content fits without scrolling - enable button immediately
            $('#btn-approve-kvkk').prop('disabled', false);
            return;
        }

        // Disable button initially if content is scrollable
        $('#btn-approve-kvkk').prop('disabled', true);

        // Enable button when scrolled to bottom
        $content.on('scroll', function() {
            var scrollTop = $(this).scrollTop();
            var scrollHeight = $(this)[0].scrollHeight;
            var clientHeight = $(this)[0].clientHeight;
            
            // Check if scrolled to bottom (with 10px tolerance)
            if (scrollTop + clientHeight >= scrollHeight - 10) {
                $('#btn-approve-kvkk').prop('disabled', false);
            }
        });
    }

    /**
     * Handles KVKK Approval
     */
    $('#btn-approve-kvkk').on('click', function() {
        var $btn = $(this);
        
        if (!window.kvkkData || !window.kvkkApprovalCallback) {
            console.error('KVKK data or callback not set!');
            alert('Bir hata oluştu. Lütfen sayfayı yenileyip tekrar deneyin.');
            return;
        }

        console.log('Approving KVKK...', window.kvkkData);

        // Disable button and show loading
        $btn.prop('disabled', true).text('Kaydediliyor...');

        // Call the approval callback
        window.kvkkApprovalCallback(window.kvkkData.kvkkId, window.kvkkData.customerId);
    });

    // ========================================================================
    // Close modal when clicking overlay
    // ========================================================================
    $('.modal__overlay').on('click', function() {
        closeKvkkModal();
});

    // ========================================================================
    // Close modal with ESC key
    // ========================================================================
    $(document).on('keydown', function(e) {
        if (e.key === 'Escape' && $('#kvkkModal').is(':visible')) {
            closeKvkkModal();
        }
    });

    console.log('Login.js loaded successfully');
});
