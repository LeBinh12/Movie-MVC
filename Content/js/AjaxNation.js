$(document).ready(function () {
    // 1. Tìm kiếm quốc gia
    $('#searchInput').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        $('table tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // 2. Thêm quốc gia
    $('#btnSaveNation').on('click', function () {
        const name = $('#nationName').val().trim();
        const status = $('#statusSwitch').is(':checked');

        if (name === "") {
            showAlert("Vui lòng nhập tên quốc gia.", "danger");
            return;
        }

        $.post('/ManagerNation/Add', { nation_name: name, status: status })
            .done(function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalAddNation').modal('hide');
                    location.reload();
                }, 1500);
            })
            .fail(function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi thêm quốc gia.", "danger");
            });
    });

    // 3. Cập nhật quốc gia
    $(document).on('click', '.btnEditNation', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');
        const status = $(this).data('status') === true || $(this).data('status') === 'true';

        $('#updateNationName').val(name);
        $('#updateStatusSwitch').prop('checked', status);
        $('#formUpdateNation').data('id', id);
    });

    $('#btnUpdateNation').on('click', function () {
        const form = $("#formUpdateNation");
        const nationId = form.data("id");
        const nationName = $("#updateNationName").val().trim();
        const status = $("#updateStatusSwitch").is(":checked");

        if (nationName === "") {
            showAlert("Vui lòng nhập tên quốc gia.", "danger");
            return;
        }

        $.ajax({
            url: "/ManagerNation/Update",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            data: JSON.stringify({
                nation_id: nationId,
                nation_name: nationName,
                status: status
            }),
            success: function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalUpdateNation').modal('hide');
                    location.reload();
                }, 1500);
            },
            error: function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi cập nhật quốc gia.", "danger");
            }
        });
    });

    // 4. Xóa quốc gia
    $(document).on('click', '.btnDeleteNation', function () {
        const id = $(this).data('id');
        const name = $(this).closest('tr').find('td:nth-child(2)').text().trim();
        $('#modalDeleteNation').find('.modal-body p:first').text(`Bạn có chắc chắn muốn xóa quốc gia "${name}" không ? `);
        $('#modalDeleteNation').data('id', id).modal('show');
    });

    $('#modalDeleteNation').find('.btn-danger').on('click', function () {
        const id = $('#modalDeleteNation').data('id');

        $.post('/ManagerNation/Delete', { nation_id: id })
            .done(function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalDeleteNation').modal('hide');
                    location.reload();
                }, 1500);
            })
            .fail(function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi xóa quốc gia.", "danger");
            });
    });

    // 5. Chuyển trạng thái quốc gia
    $(document).on('click', '.status-switch', function (e) {
        e.preventDefault();
        const $checkbox = $(this);
        const nationId = $checkbox.data('id');
        const currentStatus = $checkbox.is(':checked');
        const newStatus = !currentStatus;
        const nationName = $checkbox.closest('tr').find('td:nth-child(2)').text().trim();
        const statusText = newStatus ? 'hiển thị' : 'ẩn';

        $('#statusConfirmMessage').text(`Bạn có chắc chắn muốn ${ statusText } quốc gia "${nationName}" không ? `);

        // Lưu dữ liệu vào modal
        $('#modalToggleStatus').data({
            nationId: nationId,
            newStatus: newStatus,
            checkbox: $checkbox
        }).modal('show');
    });

    $('#btnConfirmStatusNationChange').on('click', function () {
        const modal = $('#modalToggleStatus');
        const nationId = modal.data('nationId');
        const newStatus = modal.data('newStatus');
        const $checkbox = modal.data('checkbox');

        $.post('/ManagerNation/ToggleStatus', { nation_id: nationId, status: newStatus })
            .done(function (response) {
                $checkbox.prop('checked', newStatus);
                $checkbox.closest('td').find('.form-check-label').text(newStatus ? 'Hiện' : 'Ẩn');
                $checkbox.closest('tr').find('.btnEditNation').data('status', newStatus);
                showAlert(response.message, "success");
                modal.modal('hide');
                setTimeout(() => {
                    location.reload();
                }, 1000);
            })
            .fail(function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi cập nhật trạng thái.", "danger");
            });
    });

    // Hàm hiển thị thông báo
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
});
