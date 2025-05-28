create database MovieTest

create table Categories(
	category_id int identity primary key,
	category_name nvarchar(100),
	status bit
)


create table Nation(
	nation_id int identity primary key,
	nation_name nvarchar(100)
)

CREATE TABLE movies (
    movie_id INT PRIMARY KEY IDENTITY,
    title NVARCHAR(255) NOT NULL,
    description TEXT, 
    release_date DATE, -- ngày phát hành
    duration INT, -- thời lượng
	rating FLOAT, -- đánh giá trung bình
    language NVARCHAR(50), -- ngôn ngữ
    thumbnail_url VARCHAR(255), -- ảnh phim
	trailer_url NVARCHAR(500), --  video giới thiệu
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP -- ngày tạo
);

-- một bộ phim sẽ có nhiều thể loại
CREATE TABLE MovieCategories (
    movie_id INT,
    category_id INT,
    PRIMARY KEY (movie_id, category_id),
    FOREIGN KEY (movie_id) REFERENCES Movies(movie_id),
    FOREIGN KEY (category_id) REFERENCES Categories(category_id)
);

-- một bộ phim sẽ có thể có nhiều quốc gia
CREATE TABLE MovieNations (
    movie_id INT,
    nation_id INT,
    PRIMARY KEY (movie_id, nation_id),
    FOREIGN KEY (movie_id) REFERENCES Movies(movie_id),
    FOREIGN KEY (nation_id) REFERENCES Nation(nation_id)
);

-- diễn viên
CREATE TABLE actors (
    actor_id INT PRIMARY KEY identity,
    name NVARCHAR(100) NOT NULL,
    bio TEXT, 
    birthdate DATE
);

-- đạo diễn
CREATE TABLE directors (
    director_id INT PRIMARY KEY identity,
    name NVARCHAR(100) NOT NULL,
    bio TEXT, -- giới thiệu sơ lượt về nhân vật trên
    birthdate DATE
);

-- phim sẽ có nhiều diễn viên
CREATE TABLE movie_actors (
    movie_id INT,
    actor_id INT,
    PRIMARY KEY (movie_id, actor_id),
    FOREIGN KEY (movie_id) REFERENCES movies(movie_id),
    FOREIGN KEY (actor_id) REFERENCES actors(actor_id)
);

-- phim có thể có đồng đạo diễn
CREATE TABLE movie_directors (
    movie_id INT,
    director_id INT,
    PRIMARY KEY (movie_id, director_id),
    FOREIGN KEY (movie_id) REFERENCES movies(movie_id),
    FOREIGN KEY (director_id) REFERENCES directors(director_id)
);

-- phim nhiều tập
CREATE TABLE episodes (
    episode_id INT PRIMARY KEY identity,
    series_id INT NOT NULL, -- movie_id của series
    season_number INT NOT NULL, -- phần phim
    episode_number INT NOT NULL, -- tập phim
    title NVARCHAR(255), -- tên tập 
    description TEXT, -- chi tiết tập phim
    air_date DATE, -- ngày chiếu
    duration INT, -- thời lượng phim
    FOREIGN KEY (series_id) REFERENCES movies(movie_id)
);

-- lưu trữ video phim lẻ
CREATE TABLE videos (
    video_id INT PRIMARY KEY identity,
    movie_id INT, -- Dành cho phim lẻ
    episode_id INT, -- Dành cho tập phim
    file_url NVARCHAR(255) NOT NULL, -- link phim
    resolution NVARCHAR(20), 
    format NVARCHAR(10), -- định dạng phim
    FOREIGN KEY (movie_id) REFERENCES movies(movie_id),
    FOREIGN KEY (episode_id) REFERENCES episodes(episode_id),
    CHECK (movie_id IS NOT NULL OR episode_id IS NOT NULL) 
);


CREATE TABLE users (
    user_id INT PRIMARY KEY identity,
    username VARCHAR(50) UNIQUE NOT NULL,
    email NVARCHAR(100) UNIQUE NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    avatar_url NVARCHAR(255),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    last_login DATETIME,
    is_active BIT DEFAULT 0
);



-- biinhf luận
CREATE TABLE comments (
    comment_id INT PRIMARY KEY identity,
    user_id INT NOT NULL,
    movie_id INT NOT NULL,
    content TEXT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (movie_id) REFERENCES movies(movie_id) ON DELETE CASCADE
);

-- đánh giá
CREATE TABLE ratings (
    rating_id INT PRIMARY KEY identity,
    user_id INT NOT NULL,
    movie_id INT NOT NULL,
    rating_value DECIMAL(3,1) CHECK (rating_value BETWEEN 1 AND 5),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id),
    FOREIGN KEY (movie_id) REFERENCES movies(movie_id),
    UNIQUE (user_id, movie_id)
);

-- lịch sử xem phim
CREATE TABLE watch_history (
    history_id INT PRIMARY KEY identity,
    user_id INT NOT NULL,
    movie_id INT, -- Xem phim lẻ
    episode_id INT, -- Xem tập phim
    watched_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    progress INT, -- Phần trăm đã xem
    FOREIGN KEY (user_id) REFERENCES users(user_id) ,
    FOREIGN KEY (movie_id) REFERENCES movies(movie_id),
    FOREIGN KEY (episode_id) REFERENCES episodes(episode_id)
);


-- Thêm dữ liệu vào bảng Categories
INSERT INTO Categories (category_name) VALUES 
(N'Phim Hành Động'),
(N'Phim Kinh Dị'),
(N'Phim Hài');

-- Thêm dữ liệu vào bảng Nation
INSERT INTO Nation (nation_name) VALUES 
(N'Việt Nam'),
(N'Mỹ'),
(N'Nhật Bản');

-- Chèn dữ liệu vào bảng actors
INSERT INTO actors (name, bio, birthdate) VALUES
('Robert Downey Jr.', N'Một diễn viên nổi tiếng với vai Iron Man.', '1965-04-04'),
('Scarlett Johansson', N'Nữ diễn viên từng đóng trong Black Widow.', '1984-11-22'),
('Leonardo DiCaprio', N'Thắng giải Oscar với phim The Revenant.', '1974-11-11'),
('Tom Cruise', N'Nam diễn viên nổi tiếng với loạt phim Mission Impossible.', '1962-07-03'),
('Chris Hemsworth', N'Nổi tiếng với vai diễn Thor trong MCU.', '1983-08-11');

-- Chèn dữ liệu vào bảng directors
INSERT INTO directors (name, bio, birthdate) VALUES
('Christopher Nolan', N'Đạo diễn của Inception, Interstellar.', '1970-07-30'),
('James Cameron', N'Đạo diễn của Titanic, Avatar.', '1954-08-16'),
('Steven Spielberg', N'Đạo diễn của Jurassic Park, Jaws.', '1946-12-18'),
('Quentin Tarantino', N'Nổi tiếng với Pulp Fiction, Kill Bill.', '1963-03-27'),
('Martin Scorsese', N'Đạo diễn của The Irishman, Goodfellas.', '1942-11-17');
EXEC sp_help movies


SELECT name, collation_name 
FROM sys.columns 
WHERE object_id = OBJECT_ID('movies'); 

ALTER TABLE movies 
ALTER COLUMN title NVARCHAR(255) COLLATE Vietnamese_CI_AS;

ALTER TABLE movies 
ALTER COLUMN description NVARCHAR(MAX) COLLATE Vietnamese_CI_AS;

ALTER TABLE movies 
ALTER COLUMN language NVARCHAR(100) COLLATE Vietnamese_CI_AS;


EXEC sp_help movies
