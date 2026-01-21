// Global helper fonksiyonları
$(document).ready(function () {
    initDatePickers();
    initTooltips();
});

function initDatePickers() {
    $('.datepicker').datepicker({
        language: 'tr',
        format: 'dd.mm.yyyy',
        autoclose: true,
        todayHighlight: true
    });
}

function initTooltips() {
    $('[data-bs-toggle="tooltip"]').tooltip();
}

// Para formatlama
function formatCurrency(amount) {
    return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY'
    }).format(amount);
}

// Tarih formatlama
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR');
}

// Tarih formatlama (detaylı)
function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('tr-TR');
}

// Toast bildirimi
function showToast(message, type = 'info') {
    const toastClass = {
        'success': 'bg-success',
        'error': 'bg-danger',
        'warning': 'bg-warning',
        'info': 'bg-info'
    }[type] || 'bg-info';

    const toastId = 'toast-' + Date.now();
    const toastHtml = `
        <div id="${toastId}" class="toast align-items-center text-white ${toastClass} border-0" 
             role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    <i class="fas fa-${type === 'success' ? 'check-circle' :
            type === 'error' ? 'exclamation-circle' :
                type === 'warning' ? 'exclamation-triangle' : 'info-circle'} me-2"></i>
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" 
                        data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;

    // Toast container yoksa oluştur
    if ($('#toastContainer').length === 0) {
        $('body').append(`
            <div id="toastContainer" class="toast-container position-fixed bottom-0 end-0 p-3"></div>
        `);
    }

    $('#toastContainer').append(toastHtml);
    const toast = new bootstrap.Toast(document.getElementById(toastId));
    toast.show();

    // Toast gizlendikten sonra kaldır
    $('#' + toastId).on('hidden.bs.toast', function () {
        $(this).remove();
    });
}

// Onay dialogu
function confirmAction(message, callback) {
    if (confirm(message)) {
        if (typeof callback === 'function') {
            callback();
        }
    }
}

// AJAX error handler
function handleAjaxError(xhr, status, error) {
    console.error('AJAX Error:', error);
    showToast('İşlem sırasında bir hata oluştu: ' + error, 'error');
}

// Form validasyonu
function validateForm(formId) {
    const form = document.getElementById(formId);
    let isValid = true;

    $(form).find('[required]').each(function () {
        if (!$(this).val().trim()) {
            $(this).addClass('is-invalid');
            isValid = false;
        } else {
            $(this).removeClass('is-invalid');
        }
    });

    return isValid;
}

// Dosya boyutu kontrolü
function checkFileSize(input, maxSizeMB) {
    if (input.files && input.files[0]) {
        const fileSize = input.files[0].size / 1024 / 1024; // MB cinsinden
        if (fileSize > maxSizeMB) {
            showToast(`Dosya boyutu ${maxSizeMB}MB'den küçük olmalıdır.`, 'error');
            input.value = '';
            return false;
        }
        return true;
    }
    return false;
}

// Session kontrolü
function checkSession() {
    $.ajax({
        url: '/Home/CheckSession',
        type: 'POST',
        success: function (response) {
            if (!response.loggedIn) {
                showToast('Oturumunuz sona erdi. Lütfen tekrar giriş yapın.', 'warning');
                setTimeout(() => {
                    window.location.href = '/Home/Login';
                }, 2000);
            }
        }
    });
}

// Her 5 dakikada bir session kontrolü
setInterval(checkSession, 300000);