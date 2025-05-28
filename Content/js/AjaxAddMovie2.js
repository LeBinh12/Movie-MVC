document.addEventListener('DOMContentLoaded', function () {
    const phimBoSwitch = document.getElementById('phimBoSwitch');
    const saveButton = document.getElementById('btnSaveMovie');
    const updateButton = document.getElementById('btnUpdateMovie');
    const selectedCategory = document.getElementById('theLoai').selectedOptions;
    const selectedActor = document.getElementById('dienVien').selectedOptions;
    const selectedDirector = document.getElementById('daoDien').selectedOptions;
    const selectedNation = document.getElementById('quocGia').selectedOptions;
    const tapPhimList = document.getElementById('tapPhimList');
    const editButton = document.querySelectorAll('.btnEditMovie');
    const deleteButtons = document.querySelectorAll('.btn-danger');
    const viewButtons = document.querySelectorAll('.btnViewMovie');
    // Khởi tạo Bootstrap Select cho form thêm phim
    $('#modalThemPhim').on('shown.bs.modal', function () {
        $('#theLoai, #dienVien, #daoDien, #quocGia').each(function () {
            if (!$(this).hasClass('selectpicker-initialized')) {
                $(this).selectpicker({
                    liveSearch: true,
                    width: '100%'
                });
                $(this).addClass('selectpicker-initialized');
            }
        });
    });

    // Khởi tạo Bootstrap Select cho form sửa phim
    $('#modalSuaPhim').on('shown.bs.modal', function () {
        $('#edit_theLoai, #edit_dienVien, #edit_daoDien, #edit_quocGia').each(function () {
            if (!$(this).hasClass('selectpicker-initialized')) {
                $(this).selectpicker({
                    liveSearch: true,
                    width: '100%'
                });
                $(this).addClass('selectpicker-initialized');
            }
        });
    });

    saveButton.addEventListener('click', function () {
        const isSeries = phimBoSwitch.checked;
        const relatedIds = [
            ...Array.from(selectedCategory).map(option => ({
                Id: parseInt(option.value),
                Type: 'category'
            })),
            ...Array.from(selectedActor).map(option => ({
                Id: parseInt(option.value),
                Type: 'actor'
            })),
            ...Array.from(selectedDirector).map(option => ({
                Id: parseInt(option.value),
                Type: 'director'
            })),
            ...Array.from(selectedNation).map(option => ({
                Id: parseInt(option.value),
                Type: 'nation'
            }))
        ];

        const movieData = {
            Title: document.getElementById('tenPhim').value,
            ReleaseDate: document.getElementById('thoiGianRaMat').value,
            Duration: parseInt(document.getElementById('thoiLuong').value) || null,
            Language: document.getElementById('ngonNgu').value,
            ThumbnailUrl: document.getElementById('linkAnh').value,
            TrailerUrl: document.getElementById('linkTrailer').value,
            Description: document.getElementById('moTa').value, // Thêm trường Description
            RelatedIds: relatedIds,
            IsSeries: isSeries,
            MovieLink: isSeries ? null : document.getElementById('linkPhimLe').value,
            Episodes: isSeries ? Array.from(tapPhimList.querySelectorAll('.tapPhimItem')).map((item, index) => ({
                Title: item.querySelector('input:nth-child(1)').value,
                Link: item.querySelector('input:nth-child(2)').value,
                Duration: parseInt(item.querySelector('input:nth-child(3)').value) || null,
                SeasonNumber: parseInt(item.querySelector('input:nth-child(4)').value) || 1,
                EpisodeNumber: index + 1
            })) : null
        };

        console.log("Description", document.getElementById('moTa').value )

        fetch('/ManagerMovie/AddMovie', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(movieData)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    $('#modalThemPhim').modal('hide');
                    formThemPhim.reset();
                    window.location.reload();
                } else {
                    alert(data.message);
                }
            })
            .catch(error => {
                alert('Lỗi khi gửi yêu cầu: ' + error.message);
            });
    });

    updateButton.addEventListener('click', function () {
        const movieId = document.getElementById('formSuaPhim').dataset.movieId;
        const isSeries = document.getElementById('edit_phimBoSwitch').checked;
        const editTapPhimList = document.getElementById('edit_tapPhimList');


        const relatedIds = [
            ...Array.from(document.getElementById('edit_theLoai').selectedOptions).map(option => ({
                Id: parseInt(option.value),
                Type: 'category'
            })),
            ...Array.from(document.getElementById('edit_dienVien').selectedOptions).map(option => ({
                Id: parseInt(option.value),
                Type: 'actor'
            })),
            ...Array.from(document.getElementById('edit_daoDien').selectedOptions).map(option => ({
                Id: parseInt(option.value),
                Type: 'director'
            })),
            ...Array.from(document.getElementById('edit_quocGia').selectedOptions).map(option => ({
                Id: parseInt(option.value),
                Type: 'nation'
            }))
        ];



        const movieData = {
            MovieId: parseInt(movieId),
            Title: document.getElementById('edit_tenPhim').value,
            ReleaseDate: document.getElementById('edit_thoiGianRaMat').value,
            Duration: parseInt(document.getElementById('edit_thoiLuong').value) || null,
            Language: document.getElementById('edit_ngonNgu').value,
            ThumbnailUrl: document.getElementById('edit_linkAnh').value,
            TrailerUrl: document.getElementById('edit_linkTrailer').value,
            Description: document.getElementById('edit_moTa').value, 
            RelatedIds: relatedIds,
            IsSeries: isSeries,
            MovieLink: isSeries ? null : document.getElementById('edit_linkPhimLe').value,
            Episodes: isSeries ? Array.from(editTapPhimList.querySelectorAll('.tapPhimItem')).map((item, index) => ({
                Title: item.querySelector('input:nth-child(1)').value,
                Link: item.querySelector('input:nth-child(2)').value,
                Duration: parseInt(item.querySelector('input:nth-child(3)').value) || null,
                SeasonNumber: parseInt(item.querySelector('input:nth-child(4)').value) || 1,
                EpisodeNumber: index + 1
            })) : null
        };


        fetch('/ManagerMovie/UpdateMovie', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(movieData)
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    $('#modalSuaPhim').modal('hide');
                    window.location.reload();
                } else {
                    alert(data.message);
                }
            })
            .catch(error => {
                alert('Lỗi khi cập nhật phim: ' + error.message);
            });
    })

    editButton.forEach(btn => {
        btn.addEventListener("click", function () {
            const id = this.dataset.id;
            $('#modalThemPhim').modal('hide');

            fetch(`/ManagerMovie/EditMovie?id=${id}`)
                .then(res => res.json())
                .then(data => {
                    console.log("Data", data);
                    document.getElementById('edit_tenPhim').value = data.Title || '';
                    document.getElementById('edit_thoiGianRaMat').value = parseDotNetDate(data.ReleaseDate);
                    document.getElementById('edit_thoiLuong').value = data.Duration || '';
                    document.getElementById('edit_ngonNgu').value = data.Language || '';
                    document.getElementById('edit_linkAnh').value = data.ThumbnailUrl || '';
                    document.getElementById('edit_linkTrailer').value = data.TrailerUrl || '';
                    document.getElementById('edit_phimBoSwitch').checked = data.IsSeries || false;
                    document.getElementById('edit_moTa').value = data.Description || '';
                    const episodeList = document.getElementById('edit_tapPhimList');
                    episodeList.innerHTML = '';
                    if (data.IsSeries && data.Episodes && Array.isArray(data.Episodes)) {
                        data.Episodes.forEach(x => {
                            const item = document.createElement('div');
                            item.classList.add('tapPhimItem', 'mb-2');
                            item.innerHTML = `
                                <input type="text" class="form-control me-2" placeholder="Tên tập phim" value="${x.Title || ''}" />
                                <input type="text" class="form-control me-2" placeholder="Link tập phim" value="${x.Link || ''}" />
                                <input type="text" class="form-control me-2" placeholder="Thời lượng" value="${x.Duration || ''}" />
                                <input type="text" class="form-control me-2" placeholder="Phần" value="${x.SeasonNumber || 1}" />
                                <button type="button" class="btn btn-danger btn-sm btnRemoveTap">Xóa</button>`;
                            episodeList.appendChild(item);
                        });
                    } else {
                        document.getElementById('edit_linkPhimLe').value = data.MovieLink || '';
                    }

                    const categoryIds = data.RelatedIds
                        ?.filter(r => r.Type === 'category')
                        .map(r => r.Id) || [];
                    const actorIds = data.RelatedIds
                        ?.filter(r => r.Type === 'actor')
                        .map(r => r.Id) || [];
                    const directorIds = data.RelatedIds
                        ?.filter(r => r.Type === 'director')
                        .map(r => r.Id) || [];
                    const nationIds = data.RelatedIds
                        ?.filter(r => r.Type === 'nation')
                        .map(r => r.Id) || [];

                    console.log("categoryIds", categoryIds);
                    console.log("actorIds", actorIds);
                    console.log("directorIds", directorIds);
                    console.log("nationIds", nationIds);

                    // Hàm hiển thị dữ liệu cũ dưới dạng badge
                    const displayExistingData = (selectElement, ids, containerId) => {
                        const container = document.getElementById(containerId);
                        container.innerHTML = ''; // Xóa nội dung cũ

                        if (ids.length === 0) {
                            container.innerHTML = '<small class="text-muted">Chưa có dữ liệu</small>';
                            return;
                        }

                        const stringIds = ids.map(String);
                        const selectedOptions = Array.from(selectElement.options).filter(option =>
                            stringIds.includes(option.value)
                        );

                        selectedOptions.forEach(option => {
                            const badge = document.createElement('span');
                            badge.className = 'badge bg-secondary me-1';
                            badge.textContent = option.text;
                            container.appendChild(badge);
                        });
                    };

                    // Hiển thị modal và hiển thị dữ liệu cũ
                    $('#modalSuaPhim').modal('show');
                    $('#modalSuaPhim').on('shown.bs.modal', function () {
                        displayExistingData(document.getElementById('edit_theLoai'), categoryIds, 'existing_theLoai');
                        displayExistingData(document.getElementById('edit_dienVien'), actorIds, 'existing_dienVien');
                        displayExistingData(document.getElementById('edit_daoDien'), directorIds, 'existing_daoDien');
                        displayExistingData(document.getElementById('edit_quocGia'), nationIds, 'existing_quocGia');
                    });

                    document.getElementById('edit_linkPhimLeContainer').classList.toggle('d-none', data.IsSeries);
                    document.getElementById('edit_phimBoContainer').classList.toggle('d-none', !data.IsSeries);

                    document.getElementById('formSuaPhim').dataset.movieId = data.MovieId;
                })
                .catch(error => {
                    alert('Lỗi khi tải dữ liệu: ' + error.message);
                });
        });
    });

    deleteButtons.forEach(btn => {
        btn.addEventListener('click', function () {
            const movieRow = this.closest('tr');
            const movieId = movieRow.querySelector('td:first-child').textContent; // Lấy mã phim
            const movieTitle = movieRow.querySelector('td:nth-child(2)').textContent; // Lấy tên phim

            document.getElementById('maPhimXoa').value = movieId;
            document.getElementById('tenPhimXoa').textContent = movieTitle;
            $('#modalXoaPhim').modal('show');
        });
    });

    // Xử lý nút Xóa trong modal
    document.querySelector('#modalXoaPhim .btn-danger').addEventListener('click', function () {
        const movieId = document.getElementById('maPhimXoa').value;
        console.log("Mã phim", movieId);
        fetch(`/ManagerMovie/DeleteMovie?id=${movieId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    $('#modalXoaPhim').modal('hide');
                    window.location.reload();
                } else {
                    alert(data.message);
                }
            })
            .catch(error => {
                alert('Lỗi khi xóa phim: ' + error.message);
            });
    });

    viewButtons.forEach(btn => {
        btn.addEventListener('click', function () {
            const id = this.dataset.id;

            fetch(`/ManagerMovie/EditMovie?id=${id}`)
                .then(res => res.json())
                .then(data => {
                    // Hiển thị thông tin chi tiết lên modal
                    document.getElementById('detail_tenPhim').textContent = data.Title || 'Chưa có dữ liệu';
                    document.getElementById('detail_thoiGianRaMat').textContent = parseDotNetDate(data.ReleaseDate) || 'Chưa có dữ liệu';
                    document.getElementById('detail_thoiLuong').textContent = data.Duration ? `${data.Duration} phút` : 'Chưa có dữ liệu';
                    document.getElementById('detail_ngonNgu').textContent = data.Language || 'Chưa có dữ liệu';
                    document.getElementById('detail_linkAnh').textContent = data.ThumbnailUrl || 'Chưa có dữ liệu';
                    document.getElementById('detail_thumbnail').src = data.ThumbnailUrl || '';
                    document.getElementById('detail_linkTrailer').textContent = data.TrailerUrl || 'Chưa có dữ liệu';
                    document.getElementById('detail_loaiPhim').textContent = data.IsSeries ? 'Phim bộ' : 'Phim lẻ';
                    document.getElementById('detail_moTa').textContent = data.Description || 'Chưa có mô tả';

                    // Hiển thị thể loại, diễn viên, đạo diễn, quốc gia
                    const categoryIds = data.RelatedIds?.filter(r => r.Type === 'category').map(r => r.Id) || [];
                    const actorIds = data.RelatedIds?.filter(r => r.Type === 'actor').map(r => r.Id) || [];
                    const directorIds = data.RelatedIds?.filter(r => r.Type === 'director').map(r => r.Id) || [];
                    const nationIds = data.RelatedIds?.filter(r => r.Type === 'nation').map(r => r.Id) || [];

                    const displayData = (ids, containerId, options) => {
                        const container = document.getElementById(containerId);
                        container.innerHTML = '';

                        if (ids.length === 0) {
                            container.innerHTML = '<small class="text-muted">Chưa có dữ liệu</small>';
                            return;
                        }

                        const stringIds = ids.map(String);
                        const selectedOptions = Array.from(options).filter(option =>
                            stringIds.includes(option.value)
                        );

                        selectedOptions.forEach(option => {
                            const badge = document.createElement('span');
                            badge.className = 'badge bg-secondary me-1';
                            badge.textContent = option.text;
                            container.appendChild(badge);
                        });
                    };

                    displayData(categoryIds, 'detail_theLoai', document.getElementById('edit_theLoai').options);
                    displayData(actorIds, 'detail_dienVien', document.getElementById('edit_dienVien').options);
                    displayData(directorIds, 'detail_daoDien', document.getElementById('edit_daoDien').options);
                    displayData(nationIds, 'detail_quocGia', document.getElementById('edit_quocGia').options);

                    // Hiển thị thông tin phim lẻ hoặc phim bộ
                    const detailTapPhimList = document.getElementById('detail_tapPhimList');
                    detailTapPhimList.innerHTML = '';
                    if (data.IsSeries && data.Episodes && Array.isArray(data.Episodes)) {
                        data.Episodes.forEach(x => {
                            const item = document.createElement('div');
                            item.classList.add('mb-2');
                            item.innerHTML = `
                                <p><strong>Tập ${x.EpisodeNumber}:</strong> ${x.Title || ''} (Thời lượng: ${x.Duration || 'Chưa có dữ liệu'} phút, Phần: ${x.SeasonNumber || 1}, Link: ${x.Link || 'Chưa có dữ liệu'})</p>`;
                            detailTapPhimList.appendChild(item);
                        });
                    } else {
                        document.getElementById('detail_linkPhimLe').textContent = data.MovieLink || 'Chưa có dữ liệu';
                    }

                    document.getElementById('detail_linkPhimLeContainer').classList.toggle('d-none', data.IsSeries);
                    document.getElementById('detail_phimBoContainer').classList.toggle('d-none', !data.IsSeries);

                    $('#modalXemChiTietPhim').modal('show');
                })
                .catch(error => {
                    alert('Lỗi khi tải dữ liệu: ' + error.message);
                });
        });
    });
});

function showAlert(message, type = 'success') {
    const alertContainer = document.getElementById('alertContainer');
    const alertMessage = document.getElementById('alertMessage');
    const alertText = document.getElementById('alertText');

    alertText.textContent = message;
    alertMessage.className = `alert alert-${type} alert-dismissible fade show`;
    alertContainer.style.display = 'block';

    setTimeout(() => {
        alertContainer.style.display = 'none';
    }, 3000);
}

function parseDotNetDate(dotNetDateString) {
    if (!dotNetDateString) return "";

    var timestamp = parseInt(dotNetDateString.replace(/\/Date\((\d+)\)\//, "$1"), 10);
    var date = new Date(timestamp);

    var yyyy = date.getFullYear();
    var mm = String(date.getMonth() + 1).padStart(2, '0');
    var dd = String(date.getDate()).padStart(2, '0');

    return `${yyyy}-${mm}-${dd}`;
}