$(document).ready(function () {
    // 1. Thêm thể loại
    $('#btnSaveCategory').on('click', function () {
        const name = $('#categoryName').val().trim();
        const status = $('#statusSwitch').is(':checked');

        if (name === "") {
            showAlert("Vui lòng nhập tên thể loại.", "danger");
            return;
        }

        $.post('/ManagerCategories/Add', { categoryName: name, status: status })
            .done(function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalAddCategoryMovie').modal('hide');
                    location.reload();
                }, 1500);
            })
            .fail(function (jqXHR) {
                const response = jqXHR.responseJSON || { message: "Lỗi khi thêm thể loại. Vui lòng thử lại!" };
                showAlert(response.message, "danger");
            });
    });

    // 2. Cập nhật thể loại
    $('#btnUpdateCategory').on('click', function () {
        const form = $("#formUpdateCategoryMovie");
        const categoryId = form.data("id");
        const categoryName = $("#updateCategoryName").val();
        const status = $("#updateStatusSwitch").is(":checked");

        $.ajax({
            url: "/ManagerCategories/Update",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            data: JSON.stringify({
                category_id: categoryId,
                category_name: categoryName,
                status: status
            }),
            success: function (response) {
                showAlert(response.message, "success");
                setTimeout(() => location.reload(), 1500);
            },
            error: function (jqXHR) {
                const response = jqXHR.responseJSON || { message: "Đã xảy ra lỗi khi cập nhật thể loại." };
                showAlert(response.message, "danger");
            }
        });
    });

    // 3. Xử lý chuyển trạng thái (ẩn/hiện)
    $('.status-switch').on('click', function (e) {
        e.preventDefault();

        const $checkbox = $(this);
        const categoryId = $checkbox.data('id');
        const currentStatus = $checkbox.is(':checked');
        const newStatus = currentStatus;
        const categoryName = $checkbox.closest('tr').find('td:nth-child(2)').text().trim();
        const statusText = newStatus ? 'hiển thị' : 'ẩn';

        $('#statusConfirmMessage').text(`Bạn có chắc chắn muốn ${statusText} thể loại "${categoryName}" không?`);

        // Lưu dữ liệu vào modal
        $('#modalToggleStatus').data({
            categoryId: categoryId,
            newStatus: newStatus,
            checkbox: $checkbox
        }).modal('show');
    });

    $('#btnConfirmStatusChange').on('click', function () {
        const modal = $('#modalToggleStatus');
        const categoryId = modal.data('categoryId');
        const newStatus = modal.data('newStatus');
        const $checkbox = modal.data('checkbox');

        $.post('/ManagerCategories/ToggleStatus', { categoryId: categoryId, status: !newStatus })
            .done(function (response) {
                if (response && response.success) {
                    $checkbox.prop('checked', newStatus);
                    $checkbox.closest('td').find('.form-check-label').text(newStatus ? 'Hiện' : 'Ẩn');
                    showAlert(response.message, "success");
                    modal.modal('hide');
                } else {
                    showAlert(response.message || "Cập nhật trạng thái thất bại", "danger");
                }
            })
            .fail(function (jqXHR) {
                const response = jqXHR.responseJSON || { message: "Đã xảy ra lỗi khi cập nhật trạng thái." };
                showAlert(response.message, "danger");
            });
    });

    // 4. Xử lý xóa thể loại
    $('.btnDeleteCategory').on('click', function (e) {
        e.preventDefault();

        const categoryId = $(this).data('id');
        const categoryName = $(this).closest('tr').find('td:nth-child(2)').text().trim();
        console.log("Data Id", categoryId)

        $('#deleteConfirmMessage').text(`Bạn có chắc chắn muốn xóa thể loại "${categoryName}" không?`);

        // Lưu categoryId vào modal
        $('#modalDeleteCategoryMovie').data({
            categoryId: categoryId
        }).modal('show');
    });

    $('#btnConfirmDelete').on('click', function () {
        const modal = $('#modalDeleteCategoryMovie');
        const categoryId = modal.data('categoryId');

        console.log("Data Id", categoryId)

        $.post('/ManagerCategories/Delete', { categoryId: categoryId })
            .done(function (response) {
                if (response && response.success) {
                    showAlert(response.message, "success");
                    modal.modal('hide');
                    setTimeout(() => location.reload(), 1500);
                } else {
                    showAlert(response.message || "Xóa thể loại thất bại", "danger");
                }
            })
            .fail(function (jqXHR) {
                const response = jqXHR.responseJSON || { message: "Đã xảy ra lỗi khi xóa thể loại." };
                showAlert(response.message, "danger");
            });
    });
});

function showAlert(message, type) {
    const alertClass = (type === "success") ? "alert-success" : "alert-danger";

    $('#alertMessage')
        .removeClass("alert-success alert-danger")
        .addClass(alertClass);

    $('#alertText').text(message);
    $('#alertContainer').fadeIn();

    setTimeout(() => {
        $('#alertContainer').fadeOut();
    }, 3000);
}