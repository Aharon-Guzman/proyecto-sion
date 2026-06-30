// ============================================================
// SION — site.js
// Scripts globales del sitio público
// ============================================================

(function () {
    'use strict';

    // ----------------------------------------------------------
    // 1. Navbar: agregar sombra al hacer scroll
    // ----------------------------------------------------------
    var navbar = document.querySelector('.sion-navbar');
    if (navbar) {
        window.addEventListener('scroll', function () {
            navbar.classList.toggle('scrolled', window.scrollY > 10);
        }, { passive: true });
    }

    // ----------------------------------------------------------
    // 2. Hamburguesa animada ☰ → ✕
    // Escucha los eventos de Bootstrap collapse para sincronizar
    // la clase is-open con el estado real del menú.
    // ----------------------------------------------------------
    var hamBtn   = document.getElementById('sionHamBtn');
    var navCollapse = document.getElementById('navbarMain');

    if (hamBtn && navCollapse) {
        navCollapse.addEventListener('show.bs.collapse', function () {
            hamBtn.classList.add('is-open');
            hamBtn.setAttribute('aria-expanded', 'true');
        });
        navCollapse.addEventListener('hide.bs.collapse', function () {
            hamBtn.classList.remove('is-open');
            hamBtn.setAttribute('aria-expanded', 'false');
        });
    }
    // ----------------------------------------------------------
    // 3. Contadores animados — se activan al hacer scroll
    // ----------------------------------------------------------
    var contadores = document.querySelectorAll('.sion-contador-numero');
    if (contadores.length > 0) {
        var contadorObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    animarContador(entry.target);
                    contadorObserver.unobserve(entry.target); // fix 2026-06-29: solo dispara una vez
                }
            });
        }, { threshold: 0.3 });

        contadores.forEach(function (el) {
            contadorObserver.observe(el);
        });
    }
    function animarContador(el) {
        var target = parseInt(el.getAttribute('data-target'), 10);
        var esAnios = el.getAttribute('data-anios') === 'true';
        var duration = 2000;
        var step = Math.ceil(target / (duration / 16));
        var current = 0;

        var timer = setInterval(function () {
            current += step;
            if (current >= target) {
                current = target;
                clearInterval(timer);
            }
            el.textContent = current.toLocaleString() + (esAnios ? '' : '+');
        }, 16);
    }
    // ----------------------------------------------------------
    // 4. CTA — selección de monto de donación
    // ----------------------------------------------------------
    window.sionSeleccionarMonto = function (btn) {
        document.querySelectorAll('.sion-monto-btn').forEach(function (b) {
            b.classList.remove('active');
        });
        btn.classList.add('active');
    };
    // ----------------------------------------------------------
    // 5. Banner de notificación — se oculta solo a los 5 segundos
    // ----------------------------------------------------------
    var banner = document.getElementById('sionBanner');
    if (banner) {
        setTimeout(function () {
            banner.style.transition = 'opacity 0.5s ease';
            banner.style.opacity = '0';
            setTimeout(function () { banner.style.display = 'none'; }, 500);
        }, 5000);
    }

    // ----------------------------------------------------------
    // 6. Reveal al scroll — 2026-06-29
    // Busca todos los elementos con .sion-reveal / .sion-reveal-left /
    // .sion-reveal-right y les agrega la clase .visible cuando entran
    // al viewport. El CSS en site.css (sección ANIMACIONES DE SCROLL)
    // define la transición de opacidad y posición.
    // Un solo observer global para eficiencia — no crea uno por elemento.
    // ----------------------------------------------------------
    var revealEls = document.querySelectorAll(
        '.sion-reveal, .sion-reveal-left, .sion-reveal-right'
    );
    if (revealEls.length > 0) {
        var revealObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                    revealObserver.unobserve(entry.target); // solo se anima una vez
                }
            });
        }, { threshold: 0.12 }); // 12% visible basta para disparar

        revealEls.forEach(function (el) {
            revealObserver.observe(el);
        });
    }
    // ----------------------------------------------------------
})();
