
    const avatarButton = document.getElementById('avatarButton');
    const userDropdown = document.getElementById('userDropdown');

    // باز و بسته کردن Dropdown هنگام کلیک روی دکمه آواتار
    avatarButton.addEventListener('click', () => {
        userDropdown.classList.toggle('hidden');
    });

    // بسته شدن Dropdown هنگام کلیک خارج از آن
    document.addEventListener('click', (event) => {
        if (!avatarButton.contains(event.target) && !userDropdown.contains(event.target)) {
        userDropdown.classList.add('hidden');
        }
    });

