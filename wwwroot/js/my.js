function UseSwal(icon, text) {
    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: icon,
        title: text
    });
}

async function PostForm(url, formData) {
    try {
        const response = await fetch(url, {
            method: "POST",
            body: formData
            // header نیاز نیست، fetch خودش می‌فهمه که FormData هست
        });

        const result = await response.json(); // یا response.json() اگه انتظار JSON داری
        if (response.ok) {
            UseSwal("success", "عملیات موفقیت امیز بود");

        } else {
            UseSwal("error", result.details);

        }
    } catch (err) {
        alert("خطای شبکه");
    }
}
async function PostJson(url, data,callback) {
    try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        const result = await response.json(); // یا response.json() اگه انتظار JSON داری
        if (response.ok) {
            UseSwal("success", "عملیات موفقیت امیز بود");
            

        } else {
            UseSwal("error", result.details);

        } 
        if (callback) {
            callback();
        }
    } catch (err) {
        alert("خطای شبکه");
    }
}


async function DeleteWithSwal(callback) {
    Swal.fire({
        title: "ایا میخواهید حذف شود ؟",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        cancelButtonText: "نه",
        confirmButtonText: "بله"
    }).then((result) => {
        if (result.isConfirmed) {
            if (callback) {
                callback();
                
            }
        }
    });

}