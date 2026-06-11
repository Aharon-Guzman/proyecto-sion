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
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    animarContador(entry.target);
                }
            });
        }, { threshold: 0.3 });

        contadores.forEach(function (el) {
            observer.observe(el);
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

})();
