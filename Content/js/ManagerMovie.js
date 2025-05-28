document.addEventListener('DOMContentLoaded', function () {
    const phimBoSwitch = document.getElementById('phimBoSwitch');
    const phimBoContainer = document.getElementById('phimBoContainer');
    const linkPhimLeContainer = document.getElementById('linkPhimLeContainer');
    const btnAddTapPhim = document.getElementById('btnAddTapPhim');
    const tapPhimList = document.getElementById('tapPhimList');
    const editPhimBoSwitch = document.getElementById('edit_phimBoSwitch');
    const editPhimBoContainer = document.getElementById('edit_phimBoContainer');
    const editLinkPhimLeContainer = document.getElementById('edit_linkPhimLeContainer');
    const editBtnAddTapPhim = document.getElementById('edit_btnAddTapPhim');
    const editTapPhimList = document.getElementById('edit_tapPhimList');
    // Hàm hiển thị/ẩn theo phim bộ hay phim lẻ
    function togglePhimBo() {
        if (phimBoSwitch.checked) {
            phimBoContainer.classList.remove('d-none');
            linkPhimLeContainer.classList.add('d-none');
        } else {
            phimBoContainer.classList.add('d-none');
            linkPhimLeContainer.classList.remove('d-none');
        }
    }

    function toggleEditPhimBo() {
        if (editPhimBoSwitch.checked) {
            editPhimBoContainer.classList.remove('d-none');
            editLinkPhimLeContainer.classList.add('d-none');
        } else {
            editPhimBoContainer.classList.add('d-none');
            editLinkPhimLeContainer.classList.remove('d-none');
        }
    }

    // Gắn sự kiện toggle
    phimBoSwitch.addEventListener('change', togglePhimBo);
    editPhimBoSwitch.addEventListener('change', toggleEditPhimBo);

    // Gán sự kiện cho nút thêm tập phim
    btnAddTapPhim.addEventListener('click', function () {
        const newTap = document.createElement('div');
        newTap.classList.add('input-group', 'mb-2', 'tapPhimItem');
        newTap.innerHTML = `
            <input type="text" class="form-control me-2" placeholder="Tên tập phim" />
            <input type="text" class="form-control me-2" placeholder="Link tập phim" />
            <input type="text" class="form-control me-2" placeholder="Thời lượng" />
            <input type="text" class="form-control me-2" placeholder="Phần" />
            <button type="button" class="btn btn-danger btn-sm btnRemoveTap">Xóa</button>
        `;
        tapPhimList.appendChild(newTap);

        // Gắn sự kiện xóa cho nút vừa tạo
        newTap.querySelector('.btnRemoveTap').addEventListener('click', function () {
            newTap.remove();
        });
    });

    editBtnAddTapPhim.addEventListener('click', function () {
        const newTap = document.createElement('div');
        newTap.classList.add('input-group', 'mb-2', 'tapPhimItem');
        newTap.innerHTML = `
            <input type="text" class="form-control me-2" placeholder="Tên tập phim" />
            <input type="text" class="form-control me-2" placeholder="Link tập phim" />
            <input type="text" class="form-control me-2" placeholder="Thời lượng" />
            <input type="text" class="form-control me-2" placeholder="Phần" />
            <button type="button" class="btn btn-danger btn-sm btnRemoveTap">Xóa</button>
        `;
        editTapPhimList.appendChild(newTap);

        // Gắn sự kiện xóa cho nút vừa tạo
        newTap.querySelector('.btnRemoveTap').addEventListener('click', function () {
            newTap.remove();
        });
    });

    // Gắn sự kiện xóa cho các nút xóa tập phim hiện có (nếu có)
    tapPhimList.querySelectorAll('.btnRemoveTap').forEach(button => {
        button.addEventListener('click', function () {
            button.closest('.tapPhimItem').remove();
        });
    });
    editTapPhimList.querySelectorAll('.btnRemoveTap').forEach(button => {
        button.addEventListener('click', function () {
            button.closest('.tapPhimItem').remove();
        });
    });

    // Khởi tạo trạng thái ban đầu
    togglePhimBo();
    toggleEditPhimBo();
});


