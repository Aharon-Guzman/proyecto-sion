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

})();
