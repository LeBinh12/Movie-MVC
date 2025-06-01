$(document).ready(function () {
    console.log("AjaxAuthenticationAdmin.js loaded"); 

    $('#loginForm').on('submit', function (e) {
        e.preventDefault();

        const username = $('#username').val().trim();
        const password = $('#password').val().trim();
        const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

        console.log("Username:", username); 
        console.log("Password:", password); 
        console.log("AntiForgeryToken:", antiForgeryToken); 

        if (!username || !password) {
            showAlert('Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.', 'danger');
            return;
        }

        $.ajax({
            url: '/AuthenticationAdmin/Login',
            type: 'POST',
            data: {
                username: username,
                password: password,
                __RequestVerificationToken: antiForgeryToken
            },
            success: function (response) {
                console.log("AJAX Success:", response); 
                if (response.success) {
                    showAlert('Đăng nhập thành công!', 'success');
                    setTimeout(() => {
                        window.location.href = '/ManagerMovie/Index';
                    }, 1500);
                } else {
                    showAlert(response.message || 'Tên đăng nhập hoặc mật khẩu không đúng.', 'danger');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log('AJAX Error:', textStatus, errorThrown, jqXHR); 
                const response = jqXHR.responseJSON || { message: 'Đã xảy ra lỗi khi đăng nhập: ' + errorThrown };
                showAlert(response.message, 'danger');
            }
        });
    });

    function showAlert(message, type) {
        const alertClass = (type === 'success') ? 'alert-success' : 'alert-danger';
        $('#alertMessage')
            .removeClass('alert-success alert-danger')
            .addClass(alertClass);
        $('#alertText').text(message);
        $('#alertContainer').fadeIn();
        setTimeout(() => {
            $('#alertContainer').fadeOut();
        }, 3000);
    }
});