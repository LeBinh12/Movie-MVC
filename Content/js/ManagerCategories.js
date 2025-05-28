document.addEventListener('DOMContentLoaded', function () {
    const switches = document.querySelectorAll('.status-switch');

    switches.forEach(function (checkbox) {
        const label = document.querySelector(`label[for="${checkbox.id}"]`);

        // Gán nội dung ban đầu
        label.textContent = checkbox.checked ? 'Hiện' : 'Ẩn';

        // Lắng nghe thay đổi
        checkbox.addEventListener('change', function () {
            label.textContent = this.checked ? 'Hiện' : 'Ẩn';
        });
    });
});

function showAlert(message, type = 'success') {
    const alertContainer = document.getElementById('alertContainer');
    const alertMessage = document.getElementById('alertMessage');
    const alertText = document.getElementById('alertText');

    // Đặt nội dung thông báo
    alertText.textContent = message;

    // Gán class tương ứng với loại thông báo
    alertMessage.className = `alert alert-${type} alert-dismissible fade show`;

    // Hiển thị
    alertContainer.style.display = 'block';

    // Tự động ẩn sau 3 giây
    setTimeout(() => {
        alertContainer.style.display = 'none';
    }, 3000);
}

document.addEventListener("DOMContentLoaded", function () {
    const editButtons = document.querySelectorAll(".btnEditCategory");

    editButtons.forEach(button => {
        button.addEventListener("click", function () {
            const categoryId = this.getAttribute("data-id");
            const categoryName = this.getAttribute("data-name");
            const categoryStatus = this.getAttribute("data-status") === "True";

            // Gán giá trị vào form cập nhật
            document.getElementById("updateCategoryName").value = categoryName;
            document.getElementById("updateStatusSwitch").checked = categoryStatus;

            // Nếu bạn muốn giữ lại ID để gửi khi cập nhật
            document.getElementById("formUpdateCategoryMovie").setAttribute("data-id", categoryId);
        });
    });
});