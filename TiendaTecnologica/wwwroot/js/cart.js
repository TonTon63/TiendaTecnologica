// ================================
// CARRITO LOCALSTORAGE - TECHSTORE
// ================================

const CART_KEY = "techstore_cart";

// ================================
// HELPERS BASE
// ================================

function getCart() {
    const cart = localStorage.getItem(CART_KEY);
    return cart ? JSON.parse(cart) : [];
}

function saveCart(cart) {
    localStorage.setItem(CART_KEY, JSON.stringify(cart));
    updateCartCount();
}

function formatCRC(value) {
    return "₡" + Number(value).toLocaleString("es-CR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

// ================================
// CONTADOR NAVBAR
// ================================

function getCartCount() {
    return getCart().reduce((total, item) => total + item.quantity, 0);
}

function updateCartCount() {
    const badge = document.getElementById("cartCount");
    if (!badge) return;

    const count = getCartCount();
    badge.textContent = count;
    badge.style.display = count > 0 ? "inline-flex" : "none";
}

// ================================
// TOTALES
// ================================

function getCartTotal() {
    return getCart().reduce((total, item) => total + (item.price * item.quantity), 0);
}

// ================================
// AGREGAR AL CARRITO
// ================================

function addToCart(product) {
    const cart = getCart();

    const existing = cart.find(p => p.id === product.id);

    if (existing) {
        existing.quantity += 1;
    } else {
        cart.push({
            id: product.id,
            name: product.name,
            price: product.price,
            imageUrl: product.imageUrl || "",
            quantity: 1
        });
    }

    saveCart(cart);
    showCartToast(`${product.name} agregado al carrito`);
}

// ================================
// ELIMINAR / ACTUALIZAR
// ================================

function removeFromCart(productId) {
    let cart = getCart();
    cart = cart.filter(p => p.id !== productId);
    saveCart(cart);
    renderCartPage();
}

function updateQuantity(productId, quantity) {
    const cart = getCart();
    const product = cart.find(p => p.id === productId);

    if (!product) return;

    if (quantity <= 0) {
        removeFromCart(productId);
        return;
    }

    product.quantity = quantity;
    saveCart(cart);
    renderCartPage();
}

function clearCart() {
    localStorage.removeItem(CART_KEY);
    updateCartCount();
    renderCartPage();
}

// ================================
// WHATSAPP
// ================================

function buildWhatsAppMessage() {
    const cart = getCart();

    if (!cart.length) {
        return "Hola, me gustaría consultar sobre algunos productos.";
    }

    let lines = [];
    lines.push("Hola, me interesan estos productos:");
    lines.push("");

    cart.forEach((item, index) => {
        const subtotal = item.price * item.quantity;

        lines.push(`${index + 1}. ${item.name}`);
        lines.push(`   Cantidad: ${item.quantity}`);
        lines.push(`   Precio: ${formatCRC(item.price)}`);
        lines.push(`   Subtotal: ${formatCRC(subtotal)}`);
        lines.push("");
    });

    lines.push(`Total estimado: ${formatCRC(getCartTotal())}`);
    lines.push("");
    lines.push("Quisiera más información, por favor.");

    return encodeURIComponent(lines.join("\n"));
}

function getWhatsAppCartUrl() {
    const phone = "50686806000";
    return `https://wa.me/${phone}?text=${buildWhatsAppMessage()}`;
}

// ================================
// TOAST
// ================================

function showCartToast(message) {
    const toast = document.getElementById("cartToast");
    const toastText = document.getElementById("cartToastText");

    if (!toast || !toastText) return;

    toastText.textContent = message;
    toast.classList.add("show");

    setTimeout(() => {
        toast.classList.remove("show");
    }, 2200);
}

// ================================
// BOTONES AGREGAR AL CARRITO
// ================================

function wireAddToCartButtons() {
    document.querySelectorAll(".add-to-cart-btn").forEach(button => {
        button.addEventListener("click", function () {
            const product = {
                id: parseInt(this.dataset.id),
                name: this.dataset.name,
                price: parseFloat(this.dataset.price),
                imageUrl: this.dataset.image || ""
            };

            addToCart(product);
        });
    });
}

// ================================
// RENDER PAGINA CARRITO
// ================================

function renderCartPage() {
    const cartContainer = document.getElementById("cartItemsContainer");
    const emptyContainer = document.getElementById("cartEmptyState");
    const summaryContainer = document.getElementById("cartSummary");
    const totalElement = document.getElementById("cartTotal");
    const whatsappBtn = document.getElementById("cartWhatsappBtn");

    if (!cartContainer) return;

    const cart = getCart();

    if (!cart.length) {
        cartContainer.innerHTML = "";

        if (emptyContainer) {
            emptyContainer.classList.remove("d-none");
        }

        if (summaryContainer) {
            summaryContainer.classList.add("d-none");
        }

        return;
    }

    if (emptyContainer) {
        emptyContainer.classList.add("d-none");
    }

    if (summaryContainer) {
        summaryContainer.classList.remove("d-none");
    }

    cartContainer.innerHTML = cart.map(item => {
        const subtotal = item.price * item.quantity;

        return `
            <div class="cart-item">
                <div class="cart-item-image">
                    ${item.imageUrl
                ? `<img src="${item.imageUrl}" alt="${item.name}" />`
                : `<div class="cart-item-placeholder"><i class="bi bi-image"></i></div>`
            }
                </div>

                <div class="cart-item-info">
                    <h5 class="cart-item-title">${item.name}</h5>
                    <p class="cart-item-price">${formatCRC(item.price)}</p>

                    <div class="cart-item-actions">
                        <div class="cart-qty-control">
                            <button type="button" onclick="updateQuantity(${item.id}, ${item.quantity - 1})">−</button>
                            <span>${item.quantity}</span>
                            <button type="button" onclick="updateQuantity(${item.id}, ${item.quantity + 1})">+</button>
                        </div>

                        <button type="button" class="cart-remove-btn" onclick="removeFromCart(${item.id})">
                            <i class="bi bi-x-circle me-1"></i>
                            Quitar
                        </button>
                    </div>
                </div>

                <div class="cart-item-subtotal">
                    ${formatCRC(subtotal)}
                </div>
            </div>
        `;
    }).join("");

    if (totalElement) {
        totalElement.textContent = formatCRC(getCartTotal());
    }

    if (whatsappBtn) {
        whatsappBtn.href = getWhatsAppCartUrl();
    }
}

// ================================
// INIT
// ================================

document.addEventListener("DOMContentLoaded", function () {
    updateCartCount();
    wireAddToCartButtons();
    renderCartPage();
});