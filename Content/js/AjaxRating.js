document.addEventListener('DOMContentLoaded', function () {
    const starContainer = document.getElementById('starRating');
    const stars = starContainer.querySelectorAll('.star');
    const ratingText = document.getElementById('ratingText');
    const movieId = starContainer.getAttribute('data-movie-id');

    function setStarDisplay(rating) {
        stars.forEach((star, index) => {
            star.classList.toggle('filled', index < rating);
        });
    }

    stars.forEach(star => {
        star.addEventListener('click', function () {
            const rating = parseInt(this.getAttribute('data-value'));
            setStarDisplay(rating);
            ratingText.textContent = `Bạn đã đánh giá ${rating}/5 sao`;

            // Gửi đánh giá đến server
            fetch('/Watch/RateMovie', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify({
                    movieId: parseInt(movieId),
                    rating: rating
                })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        ratingText.textContent += ` | Đánh giá trung bình: ${data.average}/5`;
                    } else {
                        ratingText.textContent = data.message;
                    }
                });
        });

        star.addEventListener('mouseover', function () {
            const rating = parseInt(this.getAttribute('data-value'));
            setStarDisplay(rating);
        });

        star.addEventListener('mouseout', function () {
            const currentRating = [...stars].filter(s => s.classList.contains('filled')).length;
            setStarDisplay(currentRating);
        });
    });
});