$(document).ready(function () {
    // Xử lý form đăng ký
    $("#registerForm").on("submit", function (e) {
        e.preventDefault();

        const form = this;
        if (!form.checkValidity()) {
            form.classList.add("was-validated");
            return;
        }

        const password = $("#registerPassword").val();
        const confirmPassword = $("#registerConfirmPassword").val();
        if (password !== confirmPassword) {
            $("#registerConfirmPassword").addClass("is-invalid");
            $("#registerMessage").html('<div class="alert alert-danger">Mật khẩu và xác nhận mật khẩu không khớp.</div>');
            return;
        }

        const formData = new FormData();
        formData.append("userName", $("#registerUsername").val());
        formData.append("email", $("#registerEmail").val());
        formData.append("password", $("#registerPassword").val());
        formData.append("changePassword", $("#registerConfirmPassword").val());
        const avatarFile = $("#registerAvatar")[0].files[0];
        if (avatarFile) {
            formData.append("avatar", avatarFile);
        }

        $.ajax({
            url: "/Authentication/Register",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $("#registerMessage").html('<div class="alert alert-success">' + response.message + '</div>');
                    form.reset();
                    form.classList.remove("was-validated");
                    // Chuyển hướng hoặc đóng modal sau khi đăng ký thành công
                    setTimeout(() => {
                        $('#registerModal').modal('hide');
                        $('#loginModal').modal('show'); // Mở modal đăng nhập sau khi đăng ký
                    }, 1000);
                } else {
                    $("#registerMessage").html('<div class="alert alert-danger">' + response.message + '</div>');
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi đăng ký:", error);
                $("#registerMessage").html('<div class="alert alert-danger">Đã xảy ra lỗi khi gửi yêu cầu: ' + error + '</div>');
            }
        });
    });

    // Xử lý form đăng nhập
    $("#loginForm").on("submit", function (e) {
        e.preventDefault();

        const form = this;
        if (!form.checkValidity()) {
            form.classList.add("was-validated");
            return;
        }

        const formData = new FormData();
        formData.append("email", $("#loginEmail").val());
        formData.append("password", $("#loginPassword").val());

        $.ajax({
            url: "/Authentication/SignIn",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    $("#loginMessage").html('<div class="alert alert-success">' + response.message + '</div>');
                    form.reset();
                    form.classList.remove("was-validated");

                    // Gọi lại JoinGroup sau khi đăng nhập thành công
                    if (typeof handleLoginSuccess === "function") {
                        handleLoginSuccess(response.userId);
                    }

                    // Làm mới trang sau 1 giây
                    setTimeout(() => {
                        $('#loginModal').modal('hide');
                        location.reload();
                    }, 1000);
                } else {
                    $("#loginMessage").html('<div class="alert alert-danger">' + response.message + '</div>');
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi đăng nhập:", error);
                $("#loginMessage").html('<div class="alert alert-danger">Đã xảy ra lỗi khi gửi yêu cầu: ' + error + '</div>');
            }
        });
    });

    // Xóa thông báo lỗi khi người dùng chỉnh sửa input
    $("#registerConfirmPassword").on("input", function () {
        $(this).removeClass("is-invalid");
        $("#registerMessage").html("");
    });

    $("#registerEmail, #registerPassword, #registerUsername").on("input", function () {
        $("#registerMessage").html("");
    });

    $("#loginEmail, #loginPassword").on("input", function () {
        $("#loginMessage").html("");
    });
});






document.addEventListener('DOMContentLoaded', function () {
    // Xử lý form Forgot Password
    const forgotPasswordForm = document.getElementById('forgotPasswordForm');
    if (forgotPasswordForm) {
        forgotPasswordForm.addEventListener('submit', function (e) {
            e.preventDefault();

            // Xác thực form
            if (!forgotPasswordForm.checkValidity()) {
                e.stopPropagation();
                forgotPasswordForm.classList.add('was-validated');
                return;
            }

            const email = document.getElementById('forgotPasswordEmail').value;

            fetch('/Authentication/ForgotPassword', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ Email: email }),
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert(data.message);
                        // Đóng modal Forgot Password và mở modal Reset Password
                        const forgotModal = bootstrap.Modal.getInstance(document.getElementById('forgotPasswordModal'));
                        forgotModal.hide();
                        const resetModal = new bootstrap.Modal(document.getElementById('resetPasswordModal'));
                        resetModal.show();
                        // Điền sẵn email vào form Reset Password
                        document.getElementById('resetEmail').value = email;
                    } else {
                        alert(data.message);
                    }
                })
                .catch(error => {
                    alert('Đã xảy ra lỗi: ' + error.message);
                });
        });
    }

    // Xử lý form Reset Password
    const resetPasswordForm = document.getElementById('resetPasswordForm');
    if (resetPasswordForm) {
        resetPasswordForm.addEventListener('submit', function (e) {
            e.preventDefault();

            // Xác thực form
            if (!resetPasswordForm.checkValidity()) {
                e.stopPropagation();
                resetPasswordForm.classList.add('was-validated');
                return;
            }

            const email = document.getElementById('resetEmail').value;
            const code = document.getElementById('resetCode').value;
            const newPassword = document.getElementById('newPassword').value;
            const confirmPassword = document.getElementById('confirmPassword').value;

            fetch('/Authentication/ResetPassword', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email: email,
                    code: code,
                    newPassword: newPassword,
                    confirmPassword: confirmPassword
                }),
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert(data.message);
                        // Đóng modal Reset Password và mở modal Login
                        const resetModal = bootstrap.Modal.getInstance(document.getElementById('resetPasswordModal'));
                        resetModal.hide();
                        const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
                        loginModal.show();
                    } else {
                        alert(data.message);
                    }
                })
                .catch(error => {
                    alert('Đã xảy ra lỗi: ' + error.message);
                });
        });
    }
});
