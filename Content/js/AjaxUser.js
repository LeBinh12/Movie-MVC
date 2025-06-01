$(document).ready(function () {
    // Lấy CSRF token từ form hoặc meta tag (nếu có)
    const csrfToken = $('input[name="__RequestVerificationToken"]').val();

    // 1. Tìm kiếm người dùng
    $('#userBtnSearch').on('click', function () {
        const search = $('#userSearchInput').val().trim();
        window.location.href = '/ManagerUser/Index?search=' + encodeURIComponent(search);
    });

    // 2. Xử lý chuyển trạng thái (Khóa/Hoạt động)
    $('.user-status-switch').on('click', function (e) {
        e.preventDefault();

        const $checkbox = $(this);
        const userId = $checkbox.attr('data-id'); // Sử dụng attr để đảm bảo giá trị được lấy
        const currentStatus = $checkbox.is(':checked');
        const newStatus = !currentStatus; // Đảo trạng thái cho chính xác
        const username = $checkbox.closest('tr').find('td:nth-child(2)').text().trim();
        const statusText = newStatus ? 'mở khóa' : 'khóa';

        // Cập nhật thông điệp xác nhận với từ ngữ chính xác cho người dùng
        $('#userStatusConfirmMessage').text(`Bạn có chắc chắn muốn ${statusText} người dùng "${username}" không?`);

        // Lưu dữ liệu vào modal
        $('#userModalToggleStatus').data({
            userId: userId,
            newStatus: newStatus,
            checkbox: $checkbox
        }).modal('show');
    });

    $('#userBtnConfirmStatusChange').on('click', function () {
        const modal = $('#userModalToggleStatus');
        const userId = modal.data('userId');
        const newStatus = modal.data('newStatus');
        const $checkbox = modal.data('checkbox');

        // Kiểm tra userId trước khi gửi
        if (!userId) {
            showUserAlert("Không tìm thấy ID người dùng. Vui lòng thử lại!", "danger");
            return;
        }

        $.ajax({
            url: '/ManagerUser/ToggleStatus',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            headers: {
                'RequestVerificationToken': csrfToken // Thêm CSRF token
            },
            data: JSON.stringify({
                userId: parseInt(userId), // Chuyển thành số nguyên
                status: newStatus
            }),
            success: function (response) {
                if (response && response.success) {
                    $checkbox.prop('checked', newStatus);
                    $checkbox.closest('td').find('.form-check-label').text(newStatus ? 'Hoạt động' : 'Khóa');
                    showUserAlert(response.message, "success");
                    modal.modal('hide');
                    setTimeout(() => {
                        location.reload();
                    }, 1000);
                    } else {
                    showUserAlert(response.message || "Cập nhật trạng thái thất bại", "danger");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                const response = jqXHR.responseJSON || { message: "Đã xảy ra lỗi khi cập nhật trạng thái: " + errorThrown };
                showUserAlert(response.message, "danger");
                console.log('AJAX Error:', textStatus, errorThrown, jqXHR);
            }
        });
    });

    // 3. Xử lý xóa người dùng
    $('.user-btn-delete').on('click', function (e) {
        e.preventDefault();

        const userId = $(this).data('id'); // Sử dụng attr để đảm bảo giá trị được lấy
        const username = $(this).closest('tr').find('td:nth-child(2)').text().trim();

        console.log("Id", userId);

        // Kiểm tra userId trước khi hiển thị modal
        if (!userId) {
            showUserAlert("Không tìm thấy ID người dùng. Vui lòng thử lại!", "danger");
            return;
        }

        $('#userDeleteConfirmMessage').text(`Bạn có chắc chắn muốn xóa người dùng "${username}" không?`);

        // Lưu userId vào modal
        $('#userModalDelete').data({
            userId: userId
        }).modal('show');
    });

    $('#userBtnConfirmDelete').on('click', function () {
        const modal = $('#userModalDelete');
        const userId = modal.data('userId');

        // Kiểm tra userId trước khi gửi
        if (!userId) {
            showUserAlert("Không tìm thấy ID người dùng. Vui lòng thử lại!", "danger");
            return;
        }

        $.ajax({
            url: '/ManagerUser/Delete',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            headers: {
                'RequestVerificationToken': csrfToken // Thêm CSRF token
            },
            data: JSON.stringify({
                userId: parseInt(userId) // Chuyển thành số nguyên
            }),
            success: function (response) {
                if (response && response.success) {
                    showUserAlert(response.message, "success");
                    modal.modal('hide');
                    setTimeout(() => location.reload(), 1500);
                } else {
                    showUserAlert(response.message || "Xóa người dùng thất bại", "danger");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                const response = jqXHR.responseJSON || { message: "Đã xảy ra lỗi khi xóa người dùng: " + errorThrown };
                showUserAlert(response.message, "danger");
                console.log('AJAX Error:', textStatus, errorThrown, jqXHR);
            }
        });
    });

    // Hàm hiển thị thông báo cho người dùng
    function showUserAlert(message, type) {
        const alertClass = (type === "success") ? "alert-success" : "alert-danger";

        $('#userAlertMessage')
            .removeClass("alert-success alert-danger")
            .addClass(alertClass);

        $('#userAlertText').text(message);
        $('#userAlertContainer').fadeIn();

        setTimeout(() => {
            $('#userAlertContainer').fadeOut();
        }, 3000);
    }
});