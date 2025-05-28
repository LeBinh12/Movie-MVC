$(document).ready(function () {
    // 1. Tìm kiếm đạo diễn
    $('#searchInput').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        $('table tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // 2. Thêm đạo diễn
    $('#btnSaveDirector').on('click', function () {
        const name = $('#directorName').val().trim();
        const bio = $('#directorBio').val().trim();
        const birthdate = $('#directorBirthdate').val();

        if (name === "") {
            showAlert("Vui lòng nhập tên đạo diễn.", "danger");
            return;
        }
        if (birthdate === "") {
            showAlert("Vui lòng chọn ngày sinh.", "danger");
            return;
        }

        $.post('/ManagerDirector/Add', { name: name, bio: bio, birthdate: birthdate })
            .done(function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalAddDirector').modal('hide');
                    location.reload();
                }, 2000);
            })
            .fail(function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi thêm đạo diễn.", "danger");
            });
    });

    // 3. Cập nhật đạo diễn
    $(document).on('click', '.btnEditDirector', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');
        const bio = $(this).data('bio');
        const birthdate = $(this).data('birthdate');

        $('#updateDirectorName').val(name);
        $('#updateDirectorBio').val(bio);
        $('#updateDirectorBirthdate').val(birthdate);
        $('#formUpdateDirector').data('id', id);
    });

    $('#btnUpdateDirector').on('click', function () {
        const form = $("#formUpdateDirector");
        const directorId = form.data("id");
        const name = $("#updateDirectorName").val().trim();
        const bio = $("#updateDirectorBio").val().trim();
        const birthdate = $("#updateDirectorBirthdate").val();

        if (name === "") {
            showAlert("Vui lòng nhập tên đạo diễn.", "danger");
            return;
        }
        if (birthdate === "") {
            showAlert("Vui lòng chọn ngày sinh.", "danger");
            return;
        }

        $.ajax({
            url: "/ManagerDirector/Update",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            data: JSON.stringify({
                director_id: directorId,
                name: name,
                bio: bio,
                birthdate: birthdate,
            }),
            success: function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalUpdateDirector').modal('hide');
                    location.reload();
                }, 2000);
            },
            error: function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi cập nhật đạo diễn.", "danger");
            }
        });
    });

    // 4. Xóa đạo diễn
    $(document).on('click', '.btnDeleteDirector', function () {
        const id = $(this).data('id');
        const name = $(this).closest('tr').find('td:nth-child(2)').text().trim();
        $('#deleteConfirmMessage').text(`Bạn có chắc chắn muốn xóa đạo diễn "${name}" không ? `);
        $('#modalDeleteDirector').data('id', id).modal('show');
    });

    $('#btnConfirmDelete').on('click', function () {
        const id = $('#modalDeleteDirector').data('id');

        $.post('/ManagerDirector/Delete', { id: id })
            .done(function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalDeleteDirector').modal('hide');
                    location.reload();
                }, 2000);
            })
            .fail(function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi xóa đạo diễn.", "danger");
            });
    });

    // 5. Chuyển trạng thái đạo diễn
    $(document).on('click', '.status-switch', function (e) {
        e.preventDefault();
        const $checkbox = $(this);
        const directorId = $checkbox.data('id');
        const currentStatus = $checkbox.is(':checked');
        const newStatus = !currentStatus;
        const directorName = $checkbox.closest('tr').find('td:nth-child(2)').text().trim();
        const statusText = newStatus ? 'hiển thị' : 'ẩn';

        $('#statusConfirmMessage').text(`Bạn có chắc chắn muốn ${ statusText } đạo diễn "${directorName}" không ? `);

        $('#modalToggleStatus').data({
            directorId: directorId,
            newStatus: newStatus,
            checkbox: $checkbox
        }).modal('show');
    });

    $('#btnConfirmStatusChange').on('click', function () {
        const modal = $('#modalToggleStatus');
        const directorId = modal.data('directorId');
        const newStatus = !modal.data('newStatus');
        const $checkbox = modal.data('checkbox');

        $.post('/ManagerDirector/ToggleStatus', { director_id: directorId, status: newStatus })
            .done(function (response) {
                $checkbox.prop('checked', newStatus);
                $checkbox.closest('td').find('.form-check-label').text(newStatus ? 'Hiện' : 'Ẩn');
                $checkbox.closest('tr').find('.btnEditDirector').data('status', newStatus);
                showAlert(response.message, "success");
                modal.modal('hide');
                setTimeout(() => {
                    location.reload();
                }, 2000);
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
