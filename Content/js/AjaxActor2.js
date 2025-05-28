$(document).ready(function () {
    // 1. Tìm kiếm diễn viên
    $('#searchInput').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        $('table tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // 2. Thêm diễn viên
    $('#btnSaveActor').on('click', function (e) {
        e.preventDefault(); // Ngăn hành vi mặc định của nút

        const name = $('#actorName').val().trim();
        const bio = $('#actorBio').val().trim();
        const birthdate = $('#actorBirthdate').val();

        console.log('Dữ liệu gửi đi:', { name, bio, birthdate }); // Debug dữ liệu

        if (name === "") {
            showAlert("Vui lòng nhập tên diễn viên.", "danger");
            return;
        }
        if (birthdate === "") {
            showAlert("Vui lòng chọn ngày sinh.", "danger");
            return;
        }

        $.ajax({
            url: '/ManagerActor/Add',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            data: JSON.stringify({
                name: name,
                bio: bio,
                birthdate: birthdate
            }),
            success: function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalAddActor').modal('hide');
                    $('#formAddActor')[0].reset(); // Reset form
                    location.reload();
                }, 2000);
            },
            error: function (xhr) {
                console.error('Lỗi:', xhr); // Debug lỗi
                showAlert(xhr.responseJSON?.message || "Đã xảy ra lỗi khi thêm diễn viên.", "danger");
            }
        });
    });

    // 3. Cập nhật diễn viên
    $(document).on('click', '.btnEditActor', function () {
        const id = $(this).data('id');
        const name = $(this).data('name');
        const bio = $(this).data('bio');
        const birthdate = $(this).data('birthdate');

        $('#updateActorName').val(name);
        $('#updateActorBio').val(bio);
        $('#updateActorBirthdate').val(birthdate);
        $('#formUpdateActor').data('id', id);
    });

    $('#btnUpdateActor').on('click', function () {
        const form = $("#formUpdateActor");
        const actorId = form.data("id");
        const name = $("#updateActorName").val().trim();
        const bio = $("#updateActorBio").val().trim();
        const birthdate = $("#updateActorBirthdate").val();

        if (name === "") {
            showAlert("Vui lòng nhập tên diễn viên.", "danger");
            return;
        }
        if (birthdate === "") {
            showAlert("Vui lòng chọn ngày sinh.", "danger");
            return;
        }

        $.ajax({
            url: "/ManagerActor/Update",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            },
            data: JSON.stringify({
                actor_id: actorId,
                name: name,
                bio: bio,
                birthdate: birthdate,
            }),
            success: function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalUpdateActor').modal('hide');
                    location.reload();
                }, 2000);
            },
            error: function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi cập nhật diễn viên.", "danger");
            }
        });
    });

    // 4. Xóa diễn viên
    $(document).on('click', '.btnDeleteActor', function () {
        const id = $(this).data('id');
        const name = $(this).closest('tr').find('td:nth-child(2)').text().trim();
        $('#deleteConfirmMessage').text(`Bạn có chắc chắn muốn xóa diễn viên "${name}" không ? `);
        $('#modalDeleteActor').data('id', id).modal('show');
    });

    $('#btnConfirmDelete').on('click', function () {
        const id = $('#modalDeleteActor').data('id');

        $.post('/ManagerActor/Delete', { id: id })
            .done(function (response) {
                showAlert(response.message, "success");
                setTimeout(() => {
                    $('#modalDeleteActor').modal('hide');
                    location.reload();
                }, 2000);
            })
            .fail(function (xhr) {
                showAlert(xhr.responseJSON.message || "Đã xảy ra lỗi khi xóa diễn viên.", "danger");
            });
    });

    // 5. Chuyển trạng thái diễn viên
    $(document).on('click', '.status-switch', function (e) {
        e.preventDefault();
        const $checkbox = $(this);
        const actorId = $checkbox.data('id');
        const currentStatus = $checkbox.is(':checked');
        const newStatus = !currentStatus;
        const actorName = $checkbox.closest('tr').find('td:nth-child(2)').text().trim();
        const statusText = newStatus ? 'hiển thị' : 'ẩn';

        $('#statusConfirmMessage').text(`Bạn có chắc chắn muốn ${ statusText } diễn viên "${actorName}" không ? `);

        $('#modalToggleStatus').data({
            actorId: actorId,
            newStatus: newStatus,
            checkbox: $checkbox
        }).modal('show');
    });

    $('#btnConfirmStatusChange').on('click', function () {
        const modal = $('#modalToggleStatus');
        const actorId = modal.data('actorId');
        const newStatus = modal.data('newStatus');
        const $checkbox = modal.data('checkbox');

        $.post('/ManagerActor/ToggleStatus', { actor_id: actorId, status: newStatus })
            .done(function (response) {
                $checkbox.prop('checked', newStatus);
                $checkbox.closest('td').find('.form-check-label').text(newStatus ? 'Hiện' : 'Ẩn');
                $checkbox.closest('tr').find('.btnEditActor').data('status', newStatus);
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
