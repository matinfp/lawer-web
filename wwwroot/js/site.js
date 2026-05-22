// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

 document.addEventListener("DOMContentLoaded", () => {
   const headers = document.querySelectorAll(".accordion-header");

   headers.forEach(header => {
     header.addEventListener("click", () => {
       const content = header.nextElementSibling;
       const isExpanded = content.style.maxHeight && content.style.maxHeight !== "0px";

       if (isExpanded) {
         // Collapse
         content.style.maxHeight = "0";
         header.textContent = header.textContent.replace("⬆️", "⬇️");
       } else {
         // Expand
         content.style.maxHeight = content.scrollHeight + "px";
         header.textContent = header.textContent.replace("⬇️", "⬆️");
       }
     });
   });
 });

function toggleModal() {
    const modal = document.getElementById('consultationModal');
    const container = document.getElementById('modalContainer');
    const backdrop = document.getElementById('modalBackdrop');

    if (modal.classList.contains('hidden')) {
        // --- OPENING ---
        modal.classList.remove('hidden');
        modal.classList.add('flex');

        // Trigger animation after the element is rendered
        requestAnimationFrame(() => {
            // Fade in backdrop
            backdrop.classList.replace('opacity-0', 'opacity-100');

            // Animation for Mobile (Slide Up)
            container.classList.remove('translate-y-full');
            container.classList.add('translate-y-0');

            // Animation for Desktop (Fade & Scale)
            container.classList.remove('md:scale-95', 'md:opacity-0');
            container.classList.add('md:scale-100', 'md:opacity-100');
        });
    } else {
        // --- CLOSING ---
        // Fade out backdrop
        backdrop.classList.replace('opacity-100', 'opacity-0');

        // Slide down container
        container.classList.add('translate-y-full');
        container.classList.remove('translate-y-0');

        // Reverse desktop scale
        container.classList.remove('md:scale-100', 'md:opacity-100');
        container.classList.add('md:scale-95', 'md:opacity-0');

        // Wait for transition duration (300ms) before hiding display
        setTimeout(() => {
            modal.classList.replace('flex', 'hidden');
        }, 300);
    }
}

// Close on Escape Key
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') toggleModal();
});

// Logic to apply discount inside the modal
function applyModalDiscount() {
    const codeInput = document.getElementById('modalOffCode');
    const btn = document.getElementById('modalApplyBtn');
    const msg = document.getElementById('discountMsg');
    const code = codeInput.value.trim();

    // Get all price elements
    const priceElements = document.querySelectorAll('.price-display');
    const oldPrices = document.querySelectorAll('.old-price');
    const links = [document.getElementById('linkChat'), document.getElementById('linkCall')];

    if (code !== "") {
        // Simulate Discount (e.g., 20% off)
        btn.innerText = "تایید شد";
        btn.classList.remove('bg-green-600', 'hover:bg-green-700');
        btn.classList.add('bg-green-800', 'cursor-default');
        btn.disabled = true;
        codeInput.disabled = true;

        // Update text to show discount applied
        msg.innerText = "کد تخفیف با موفقیت اعمال شد";
        msg.classList.replace('text-gray-400', 'text-green-600');

        // Loop through prices to update them
        priceElements.forEach((el, index) => {
            const basePrice = parseInt(el.getAttribute('data-base-price'));

            // Show old price
            oldPrices[index].innerText = basePrice.toLocaleString() + " تومان";
            oldPrices[index].classList.remove('hidden');

            // Calculate new price (20% off simulation)
            const newPrice = Math.floor(basePrice * 0.8);
            el.innerText = newPrice.toLocaleString();
        });

        // Append code to links so Controller receives it (e.g. ?offCode=XYZ)
        links.forEach(link => {
            if (link) {
                let currentHref = link.getAttribute('href');
                // check if already has query params
                if (currentHref.includes('?')) {
                    link.setAttribute('href', currentHref + '&offCode=' + code);
                } else {
                    link.setAttribute('href', currentHref + '?offCode=' + code);
                }
            }
        });

    } else {
        msg.innerText = "لطفا کد را وارد کنید";
        msg.classList.replace('text-gray-400', 'text-red-400');
    }
}

// POPUP LOGIC
// ==========================================

document.addEventListener("DOMContentLoaded", async function () {
    // 1. Load your existing lists
    await GetList(1);

    // 2. Trigger Popup after a short delay (e.g., 1.5 seconds)
    setTimeout(openWelcomeModal, 1500);
});

const modalWrapper = document.getElementById('welcomeModal');
const modalBackdrop = document.getElementById('welcomeBackdrop');
const modalPanel = document.getElementById('welcomePanel');

function openWelcomeModal() {
    // If you want to show it only once per session, uncomment the next 3 lines:
    // if (sessionStorage.getItem('welcomeShown')) return;
    // sessionStorage.setItem('welcomeShown', 'true');

    modalWrapper.classList.remove('hidden');

    // Small timeout to allow browser to render 'block' before animating opacity
    setTimeout(() => {
        // Fade in backdrop
        modalBackdrop.classList.remove('opacity-0');
        modalBackdrop.classList.add('opacity-100');

        // Slide up panel
        modalPanel.classList.remove('translate-y-full', 'opacity-0', 'md:translate-y-10', 'md:scale-95');
        modalPanel.classList.add('translate-y-0', 'opacity-100', 'md:translate-y-0', 'md:scale-100');
    }, 50);
}

function closeWelcomeModal() {
    // Fade out backdrop
    modalBackdrop.classList.remove('opacity-100');
    modalBackdrop.classList.add('opacity-0');

    // Slide down panel
    modalPanel.classList.remove('translate-y-0', 'opacity-100', 'md:translate-y-0', 'md:scale-100');
    modalPanel.classList.add('translate-y-full', 'opacity-0', 'md:translate-y-10', 'md:scale-95');

    // Hide wrapper after animation finishes (500ms matches css duration)
    setTimeout(() => {
        modalWrapper.classList.add('hidden');
    }, 500);
}