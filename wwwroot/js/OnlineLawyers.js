document.addEventListener("DOMContentLoaded", () => {
    initOnlineLawyers();
});

function initOnlineLawyers() {
    const lawyerList = document.getElementById('lawyer-list');
    if (!lawyerList) return;

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/onlineLawyers")
        .build();

    connection.start()
        .then(() => console.log("Connected to OnlineLawyersHub"))
        .catch(err => console.error(err));

    connection.on("OnlineLawyersUpdated", async (ids) => {
        const arr = Array.from(ids || []);
        if (arr.length === 0) {
            lawyerList.innerHTML = '<p>هیچ وکیلی آنلاین نیست</p>';
            return;
        }

        // فراخوانی API برای گرفتن جزئیات وکیل‌ها
        const res = await fetch('/api/lawyers/online', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(arr)
        });
        const data = await res.json();
        renderLawyers(data);
    });
}

function renderLawyers(lawyers) {
    const container = document.getElementById('lawyer-list');
    container.innerHTML = '';

    lawyers.forEach(l => {
        const specialitiesHtml = l.specialization?.length
            ? l.specialization.map(item => `<span class="rounded-full bg-indigo-50 px-2 py-1 text-xs font-medium text-indigo-700 shadow-sm">${item.title}</span>`).join('')
            : '<span class="text-xs text-gray-400">تخصصی ثبت نشده</span>';

        const el = document.createElement('div');
        el.className = 'h-96 w-64 flex-none rounded-lg border border-gray-200 p-4 text-center shadow-sm hover:shadow-md';
        el.innerHTML = `
            <p class="inline-block rounded-md bg-green-100 px-3 py-1 text-sm font-semibold text-green-700 shadow">آنلاین</p>
            <a href="/Lawyer/${l.lawyerSlug}">
                <img src="/api/file/getfile?fileName=${l.photo}" class="mx-auto h-24 w-24 rounded-full border-2 border-indigo-200 object-cover shadow-sm" onerror="this.src='/images/lawyers/avatar.webp'" />
            </a>
            <h2 class="mt-3 text-base font-bold text-gray-800">
                <a href="/Lawyer/${l.lawyerSlug}" class="hover:text-blue-600">${l.fullName}</a>
            </h2>
            <p class="mt-2 text-sm text-yellow-500">⭐ <span class="text-gray-500">(${l.ratingAverage} نظر)</span></p>
            <p class="text-sm text-gray-700">مشاوره موفق: <span class="font-semibold">${l.caseCount}</span></p>
            <div class="mt-3 flex flex-wrap justify-center gap-2">${specialitiesHtml}</div>
            <a href="/Lawyer/${l.lawyerSlug}" class="mt-4 block w-full rounded-lg bg-green-600 py-2 text-center font-semibold text-white transition hover:bg-green-700">دریافت مشاوره</a>
        `;
        container.appendChild(el);
    });
}
